using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] ButtonState spinButton;

    [SerializeField] TextMeshProUGUI textActionsRemaining, textCurrentScore, textTargetScore;
    [SerializeField] TextAsset hintTextFile;

    [SerializeField] Slider numberBarSlider;
    [SerializeField] RectTransform targetNumberMarker, sliderHandle;



    private void Update()
    {
        if (spinButton.GetComponent<Button>().interactable)
        {
            if (!spinButton.mouseDown && !spinButton.mouseOver) { textActionsRemaining.rectTransform.localPosition = Vector3.up * 15; }
            if (!spinButton.mouseDown && spinButton.mouseOver) { textActionsRemaining.rectTransform.localPosition = Vector3.zero; }
            if (spinButton.mouseDown && spinButton.mouseOver) { textActionsRemaining.rectTransform.localPosition = Vector3.up * -15; }
        }


    }

    public void SpinButton()
    {
        FindObjectOfType<Wheel>().Spin();
    }

    public void UpdateCanvasText(int actionsRemaining, int currentScore, int targetScore = 99999999)
    {
        if(targetScore != 99999999)
        {
            var previousParent = targetNumberMarker.GetComponent<RectTransform>().parent;
            targetNumberMarker.GetComponent<RectTransform>().parent = sliderHandle;
            numberBarSlider.value = targetScore;
            targetNumberMarker.GetComponent<RectTransform>().localPosition= Vector3.zero;
            targetNumberMarker.GetComponent<RectTransform>().parent = previousParent;
            textTargetScore.text = targetScore.ToString();
            ResetCurrentScore();
        }
        StartCoroutine(CountDownCurrentScoreRoutine());
        textActionsRemaining.text = actionsRemaining.ToString();
    }   

    public void ResetCurrentScore()
    {
        numberBarSlider.value = 0;
        textCurrentScore.text = "0";
    }

    IEnumerator CountDownCurrentScoreRoutine()
    {

        var differenceBetweenScores = Mathf.Abs(numberBarSlider.value - FindObjectOfType<Wheel>().currentScore);
        
        bool scoreOutOfSliderRange = false;
        while (numberBarSlider.value != FindObjectOfType<Wheel>().currentScore && !scoreOutOfSliderRange)
        {
            if (numberBarSlider.value > FindObjectOfType<Wheel>().currentScore) { numberBarSlider.value -= .5f; }
            else if (numberBarSlider.value < FindObjectOfType<Wheel>().currentScore) { numberBarSlider.value += .5f; }
            numberBarSlider.value = System.MathF.Round(numberBarSlider.value, 1);
            textCurrentScore.text = Mathf.RoundToInt(numberBarSlider.value).ToString();   
            yield return new WaitForSeconds(.9f / differenceBetweenScores*.1f);
            if (numberBarSlider.value == numberBarSlider.maxValue || numberBarSlider.value == numberBarSlider.minValue)
            {
                scoreOutOfSliderRange = FindObjectOfType<Wheel>().currentScore > numberBarSlider.maxValue || FindObjectOfType<Wheel>().currentScore < numberBarSlider.minValue;
            }
        }
        textCurrentScore.text = FindObjectOfType<Wheel>().currentScore.ToString();

    }
}
