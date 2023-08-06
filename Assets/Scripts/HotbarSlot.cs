using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HotbarSlot : MonoBehaviour
{

    //This script will be attached to each slot on the hotbar at the bottom of the game screen.
    //It will manage the info displayed on the slot and when it should be shown.
    [SerializeField] public SpriteRenderer numberSprite, tileSprite;
    [SerializeField] TextMeshPro numberText;
    [SerializeField] Sprite[] numberSprites;
    [SerializeField] Sprite reverseDirectionSprite, skipNextSegmentSprite;

    public Segment storedSegment;

    public void CheckSegment()
    {

        Debug.Log(storedSegment.transform.parent == this.transform);
        //if a segment is stored, use the info from that segment and assign the slots information.
        if (storedSegment.transform.parent == this.transform)
        {
            AssignFunctionAndNumberToText();
            AssignNumberSprite();
            CopySegmentAbilityColor();
        }
        //if a segment is NOT stored, remove all information and display a blank slot.
        else
        {
            EmptySlot();
        }
    }

    public void EmptySlot()
    {
        numberSprite.sprite = null;
        tileSprite.color = Color.clear;
        numberText.text = string.Empty;
    }

    private void CopySegmentAbilityColor()
    {
        //change slot background color to match the segment color.
        tileSprite.color = storedSegment.sliceSpriteRenderer.color;
    }

    private void AssignNumberSprite()
    {
        //Display a sprite that shows the value of the segment (1-9)
        if (storedSegment.powerAmount > 0)
        {
            numberSprite.sprite = numberSprites[storedSegment.powerAmount - 1];
        }
        //If the segment is a special power and has no value, then selected the specific sprite to display
        else
        {
            switch (storedSegment.powerType)
            {
                case PortionPowerType.ReverseDirection:
                    numberSprite.sprite = reverseDirectionSprite;
                    break;
                case PortionPowerType.SkipNextSegment:
                    numberSprite.sprite = skipNextSegmentSprite;
                    break;
            }
        }
    }

    private void AssignFunctionAndNumberToText()
    {
        //change text in corner of the slot to show what the segment stored inside does e.g. "+", "-".
        switch (storedSegment.powerType)
        {
            case PortionPowerType.Addition:
                numberText.text = "+";
                break;
            case PortionPowerType.Subtraction:
                numberText.text = "-";
                break;
            case PortionPowerType.Multiplication:
                numberText.text = "x";
                break;
            case PortionPowerType.Division:
                numberText.text = "÷";
                break;
            case PortionPowerType.ReverseDirection:
                break;
        }
        //if the stored segment's number is greater than 0, add its value to the string.
        if (storedSegment.powerAmount > 0)
        {
            numberText.text = numberText.text + storedSegment.powerAmount.ToString();
        }
        else
        {
            numberText.text = "";
        }
    }
}
