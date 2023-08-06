using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODMethods : MonoBehaviour
{
    public void ChangeMusicState(int value)
    {
        FindObjectOfType<FMODController>().ChangeMusicState(value);
    }



}
