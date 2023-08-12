using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] public List<Portion> portions;
    float rotationAmount, targetRotation;

    [SerializeField] public int actionsRemaining, currentScore, targetScore;
    [SerializeField] Portion currentPortion;
    [SerializeField] Transform wheelToRotate;
    [SerializeField] public int spinDirection = 1;
    bool isSpinning;

    [SerializeField] Sprite spriteWheelClockwise, spriteWheelAnticlockwise;

    public bool skipNextSegment = false;

    [SerializeField] SpriteRenderer directionSpriteRenderer;
    [SerializeField] Sprite directionSpriteClockwise, directionSpriteAntiClockwise;

    private void Start()
    {
        UpdateTargetRotation();
        FindObjectOfType<GameCanvas>().UpdateCanvasText(actionsRemaining, currentScore, targetScore);
    }



    public void ToggleDirection()
    {
        spinDirection = -spinDirection;
        if(spinDirection == -1) 
        {
            wheelToRotate.GetComponent<SpriteRenderer>().sprite = spriteWheelClockwise;
            directionSpriteRenderer.sprite = directionSpriteClockwise;
        }
        else if(spinDirection == 1)
        {
            wheelToRotate.GetComponent<SpriteRenderer>().sprite = spriteWheelAnticlockwise;
            directionSpriteRenderer.sprite = directionSpriteAntiClockwise;

        }

        StartCoroutine(FlashDirectionSprite());
    }

    public void MakeNextSegmentInvisible()
    {
        int currentPortionIndex = 0, nextPortionIndex;
        for(int i = 0; i < portions.Count; i++)
        {
            if (portions[i] == currentPortion)
            {
                currentPortionIndex = i;
            }
        }

        if(spinDirection == -1)
        {
            nextPortionIndex = currentPortionIndex - 1;
        }
        else
        {
            nextPortionIndex = currentPortionIndex + 1;
        }
        if(nextPortionIndex >= portions.Count)
        {
            nextPortionIndex = 0;
        }
        if(nextPortionIndex < 0)
        {
            nextPortionIndex = portions.Count - 1;
        }

        portions[nextPortionIndex].GetComponentInChildren<Segment>().SliceSpriteInvisible();

    }

    private void UpdateTargetRotation()
    {
        //rotation amount is assigned by dividing 360 degrees by how many portion slots exist on the wheel.
        rotationAmount = 360f / portions.Count;
        //target rotation is assigned by taking the current rotation and adding the rotation amount. A check is then made to keep the target rotation within 0-360.
        targetRotation = Mathf.FloorToInt(wheelToRotate.eulerAngles.z) + Mathf.FloorToInt(rotationAmount) * spinDirection;
        if (targetRotation > 360f) { targetRotation -= 360f; }
        if (targetRotation < 0f) { targetRotation += 360f; }
    }

    IEnumerator FlashDirectionSprite()
    {
        directionSpriteRenderer.color = Color.white;
        while(directionSpriteRenderer.color.a >0)
        {
            directionSpriteRenderer.color = new Color(directionSpriteRenderer.color.r, directionSpriteRenderer.color.g, directionSpriteRenderer.color.b, directionSpriteRenderer.color.a - 0.1f);
            yield return new WaitForSeconds(.1f);
        }
    }

    public void Spin()
    {
        //This method will be used by the button in the middle of the wheel, it will only execute the spin coroutine if:
        //It is not already spinning
        //All portions on the wheel are occupied.
        if (isSpinning || !CheckAllPortionsUsed()) { return; }
        StartCoroutine(SpinCoroutine());
    }

    public bool CheckAllPortionsUsed()
    {
        //This method will list through all portions in the scene and if any one is not occuped will return false.
        bool allUsed = true;
        foreach(Portion portion in FindObjectsOfType<Portion>())
        {
            if(!portion.isOccupied) { allUsed = false; }
        }
        return allUsed;
    }

    IEnumerator SpinCoroutine()
    {
        
        //isSpinning bool set true to stop coroutine from being ran a second time.
        isSpinning = true;
        //change music
        FindObjectOfType<FMODController>().ChangeAnswerState(1);

        //Segment controll disabled to stop segments being removed from wheel while it spins.
        FindObjectOfType<SegmentController>().disabled = true;
        //first rotation is calculated manually as rotation amount needs to be halfed for the first turn.
        targetRotation = Mathf.FloorToInt(wheelToRotate.eulerAngles.z) + Mathf.FloorToInt(rotationAmount/2) * spinDirection;
        //loop runs until actions remaining reaches 0.
        while (actionsRemaining > 0)
        {
            //wheel is spun until it reaches target rotation.
            while (Mathf.FloorToInt(wheelToRotate.eulerAngles.z) != targetRotation)
            {
                FindObjectOfType<SFXManager>().PlayWheelSpin();
                wheelToRotate.eulerAngles = new Vector3(0, 0, wheelToRotate.eulerAngles.z + spinDirection);
                yield return new WaitForSeconds(.0125f);
            }
            FindObjectOfType<SFXManager>().StopWheelSpin();

            //when wheel is spun to target rotation, it checks if portion contains a segment.
            if (currentPortion.GetComponentInChildren<Segment>())
            {
                if (!skipNextSegment)
                {
                    CheckSlice();
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    currentPortion.GetComponentInChildren<Segment>().SliceSpriteVisible();
                    skipNextSegment = false;
                }
            }
            //target rotation updated using full rotation amount this time.
            UpdateTargetRotation();
        }
        //once actions remaining reaches 0, the 'SpinOver' method is run.
        yield return new WaitForSeconds(1f);
        SpinOver();
    }

    void SpinOver()
    {
        //reenable segment controller and disable isSpinning bool.
        FindObjectOfType<SegmentController>().disabled = false;
        isSpinning = false;

        //if player has completed the level
        if (currentScore == targetScore)
        {
            LevelLoader.SetProgress(LevelLoader.LoadProgress() + 1); //increment level progress 
            if (LevelLoader.LoadProgress() >= FindObjectOfType<LevelLoader>().levels.Length)
            {
                FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/FinalVictory");
            }
            else
            {
                FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Level/Level_Win");

            }
            FindObjectOfType<Menu>().FadeInBlur(); //blur screen
            FindObjectOfType<Menu>().OpenResultsScreen(); //enable results screen
            FindObjectOfType<Timer>().ToggleTimerRunning(false);
        }
        //if player has not completed the level
        else
        {
            FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Level/Level_Lose");
            FindObjectOfType<FMODController>().ChangeAnswerState(0);
            FindObjectOfType<LevelLoader>().LoadLevelInformation(); //reset current level information
            FindObjectOfType<GameCanvas>().UpdateCanvasText(actionsRemaining, currentScore, targetScore); //update ui to show current level info has been reset.
            FindObjectOfType<GameCanvas>().ResetCurrentScore();
            foreach(Segment segment in FindObjectsOfType<Segment>())
            {
                segment.SliceSpriteVisible();
            }
            wheelToRotate.transform.eulerAngles = Vector3.zero; //return wheel to original rotation
            wheelToRotate.GetComponent<SpriteRenderer>().sprite = spriteWheelAnticlockwise;
            directionSpriteRenderer.sprite = directionSpriteAntiClockwise;
            spinDirection = 1;
            skipNextSegment = false; //resets skip power incase turn ended with it enabled.
        }
    }

    void CheckSlice()
    {
        actionsRemaining--; //use an action
        currentScore = currentPortion.GetComponentInChildren<Segment>().ApplySegmentPower(currentScore); //apply segments ability.
        FindObjectOfType<GameCanvas>().UpdateCanvasText(actionsRemaining, currentScore); //update ui to display updated current level info
    }


    // a small box collider is sat at the top of the wheel to check which portion is currently at the top of the wheel.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Portion"))
        {
            currentPortion = collision.GetComponent<Portion>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Portion") && currentPortion == collision.GetComponent<Portion>())
        {
            currentPortion = null;
        }
    }


    private void OnDestroy()
    {
        StopCoroutine(SpinCoroutine());
    }
}
