using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public enum PortionPowerType
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    ReverseDirection,
    SkipNextSegment
}
public class Segment : MonoBehaviour
{
    [SerializeField] LayerMask portionLayer;
    public Transform originalParent;
    [SerializeField] public PortionPowerType powerType;
    [SerializeField] public int powerAmount = 1;
    [SerializeField] public SpriteRenderer valueSpriteRenderer;
    [SerializeField] public SpriteRenderer sliceSpriteRenderer;
    [SerializeField] Color[] colors;
    [SerializeField] Sprite[] valueSprites, sliceSprites, abilitySprites;
    
    private void Start()
    {
        originalParent = transform.parent;


        if (powerAmount > 0) { valueSpriteRenderer.sprite = valueSprites[powerAmount - 1]; }
        else 
        {
            if(powerType == PortionPowerType.ReverseDirection)
            {
                valueSpriteRenderer.sprite = abilitySprites[0];
            } 
            else if(powerType == PortionPowerType.SkipNextSegment)
            {
                valueSpriteRenderer.sprite = abilitySprites[1];
            }
        }
        sliceSpriteRenderer.sprite = GetCorrectSizeSliceSprite();
        AssignValueSpriteRendererSize();
        AssignSliceColour();

        CheckParent();

    }


    public void UpdateSpriteLayer(string layer)
    {
        sliceSpriteRenderer.sortingLayerName = layer;
        valueSpriteRenderer.sortingLayerName = layer;
    }

    private void AssignValueSpriteRendererSize()
    {
        switch (FindObjectOfType<Wheel>().portions.Count)
        {
            case 3:
                valueSpriteRenderer.transform.localScale = Vector3.one;
                break;
            case 5:
                valueSpriteRenderer.transform.localScale = Vector3.one * .75f;
                break;
            case 8:
                valueSpriteRenderer.transform.localScale = Vector3.one * .5f;
                break;
        }
    }

    private void AssignSliceColour()
    {
        switch (powerType)
        {
            case PortionPowerType.Addition:
                sliceSpriteRenderer.color = colors[0];
                break;
            case PortionPowerType.Subtraction:
                sliceSpriteRenderer.color = colors[1];
                break;
            case PortionPowerType.Multiplication:
                sliceSpriteRenderer.color = colors[2];
                break;
            case PortionPowerType.Division:
                sliceSpriteRenderer.color = colors[3];
                break;
            default:
                sliceSpriteRenderer.color = colors[4];
                break;
        }
    }

    private Sprite GetCorrectSizeSliceSprite()
    {
        Sprite spriteToReturn = null; 
        switch (FindObjectOfType<Wheel>().portions.Count)
        {
            case 3:
                spriteToReturn = sliceSprites[0];
                break;
            case 5:
                spriteToReturn = sliceSprites[1];
                break;
            case 8:
                spriteToReturn = sliceSprites[2];
                break;
        }
        return spriteToReturn;
    }

    public int ApplySegmentPower(int currentAmount)
    {

        int newAmount = currentAmount;
        switch (powerType)
        {
            case PortionPowerType.Addition:
                FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Wheel/Wheel_Addition");
                newAmount += powerAmount;
                break;
            case PortionPowerType.Subtraction:
                FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Wheel/Wheel_Subtraction");
                newAmount -= powerAmount;
                break;
            case PortionPowerType.Multiplication:
                FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Wheel/Wheel_Multiplication");
                newAmount *= powerAmount;
                break;
            case PortionPowerType.Division:
                FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Wheel/Wheel_Division");
                newAmount /= powerAmount;
                break;
            case PortionPowerType.ReverseDirection:
                FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Wheel/Wheel_FlipRotation");
                FindObjectOfType<Wheel>().ToggleDirection();
                break;
            case PortionPowerType.SkipNextSegment:
                FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Wheel/Wheel_Skip");
                FindObjectOfType<Wheel>().MakeNextSegmentInvisible();
                FindObjectOfType<Wheel>().skipNextSegment = true;
                break;

        }

        return newAmount;
    }


    private void OnMouseDown()
    {
        /*if(!isHeld)
        {
            bool playerAlreadyHoldingSegment = false;
            //check if any other segment is currently being carried by the player.
            foreach(Segment segment in FindObjectsOfType<Segment>())
            {
                if(segment != this && segment.isHeld)
                {
                    playerAlreadyHoldingSegment = true;
                }
            }
            if(!playerAlreadyHoldingSegment)
            {
                isHeld= true;
                transform.parent = null;
                if (portionHoveredOver != null)
                {
                    portionHoveredOver.isOccupied = false;
                    portionHoveredOver = null;
                }
            }
        }
        else
        {
            isHeld = false;
            if (portionHoveredOver == null || portionHoveredOver.isOccupied)
            {
                transform.parent = originalParent;
                transform.localPosition = Vector3.zero;
                transform.eulerAngles = originalRotation;
            }
            else
            {
                portionOccupying = portionHoveredOver;
                transform.parent = portionOccupying.transform;
                transform.localPosition = Vector3.zero;
                transform.eulerAngles = transform.parent.eulerAngles;
            }
        }*/
    }

    public void SliceSpriteInvisible()
    {
        sliceSpriteRenderer.color = new Color(sliceSpriteRenderer.color.r, sliceSpriteRenderer.color.g, sliceSpriteRenderer.color.b, 0);
    }
    public void SliceSpriteVisible()
    {
        StartCoroutine(SliceSpriteVisibleCoroutine());
    }

    IEnumerator SliceSpriteVisibleCoroutine()
    {
        while (sliceSpriteRenderer.color.a < 1)
        {
            sliceSpriteRenderer.color = new Color(sliceSpriteRenderer.color.r, sliceSpriteRenderer.color.g, sliceSpriteRenderer.color.b, sliceSpriteRenderer.color.a + 0.1f);
            yield return new WaitForSeconds(.05f);
        }
    }

    public void CheckParent()
    {
        originalParent.GetComponent<HotbarSlot>().CheckSegment();
        if (transform.parent == originalParent)
        {
            sliceSpriteRenderer.enabled = false;
            valueSpriteRenderer.enabled = false;
            if (GetComponent<PolygonCollider2D>() != null)
            {
                Destroy(sliceSpriteRenderer.gameObject.GetComponent<PolygonCollider2D>());
            }
            sliceSpriteRenderer.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            sliceSpriteRenderer.enabled = true;
            valueSpriteRenderer.enabled = true;

            if (GetComponent<PolygonCollider2D>() == null)
            {
                sliceSpriteRenderer.gameObject.AddComponent<PolygonCollider2D>();
            }
            sliceSpriteRenderer.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnDestroy()
    {
        originalParent.GetComponent<HotbarSlot>().EmptySlot();
    }
}
