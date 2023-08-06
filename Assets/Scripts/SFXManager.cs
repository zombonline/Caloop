using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public const string BUTTON_GENERIC = "event:/SFX/Button/Button_Generic";
    public const string WHEEL_SPIN = "event:/SFX/Wheel/Wheel_Spin";
    FMOD.Studio.EventInstance wheelSpin;
    bool wheelSpinPlaying = false;

    public const string BAR_MOVEMENT = "event:/SFX/Bar_Movement";
    FMOD.Studio.EventInstance barMovement;
    bool barMoving = false;
    private void Start()
    {
    }

    public void PlayButtonGeneric()
    {
        PlayAudioEvent(BUTTON_GENERIC);
    }
    public void PlayAudioEvent(string path)
    {
        var audioEvent = RuntimeManager.CreateInstance(path);
        audioEvent.start();
        audioEvent.release();
    }

    public void PlayWheelSpin()
    {
        if(wheelSpinPlaying)
        { return; }
        wheelSpinPlaying = true;
        wheelSpin = RuntimeManager.CreateInstance(WHEEL_SPIN);
        wheelSpin.start();
    }

    public void StopWheelSpin()
    {
        wheelSpinPlaying = false;
        wheelSpin.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        wheelSpin.release();
    }

    public void PlayBarMovement(float speed, float direction)
    {
        
        barMoving = true;
        barMovement = RuntimeManager.CreateInstance(BAR_MOVEMENT);
        barMovement.start();
        barMovement.setParameterByName("Number Direction", direction, false);
        barMovement.setParameterByName("Number Speed", speed, false);
        wheelSpin.release();


    }


}
