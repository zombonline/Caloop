using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonState : MonoBehaviour
{
    public bool mouseOver, mouseDown;
    public void SetMouseover(bool val)
    {
        mouseOver = val;
    }
    public void SetMouseDown(bool val)
    {
        mouseDown = val;
    }
}
