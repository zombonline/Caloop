using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODController : MonoBehaviour
{
    StudioEventEmitter emitter;

    private void Awake()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }

    public void ChangeMusicState(int value)
    {
        emitter.SetParameter("Music State", value, false);

    }

    public void ChangeAnswerState(int value)
    {
        
        emitter.SetParameter("Answer", value, false);
    }

}
