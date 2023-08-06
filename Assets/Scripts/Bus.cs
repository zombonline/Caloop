using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus : MonoBehaviour
{
    FMOD.Studio.Bus bus;
    [SerializeField] [Range(10f,-80f)] public float busVolume;
    [SerializeField] string busPath;
    private void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus(busPath);
        bus.getVolume(out float volume);
        busVolume = volume;
    }

    private void Awake()
    {

        if (tag == "Music")
        {
            busVolume = -40 + (50 / 10) * PlayerPrefs.GetInt(VolumeControl.MUSIC_VOLUME,8);
        }
        if(tag == "SFX")
        {
            busVolume = -40 + (50 / 10) * PlayerPrefs.GetInt(VolumeControl.SFX_VOLUME,10);

        }

    }
    private void Update()
    {
        bus.setVolume(DecilbelToLinear(busVolume));
    }
    private float DecilbelToLinear(float dB)
    {
        float linear = Mathf.Pow(10f,dB/20f);
        return linear;
    }
}
