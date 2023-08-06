using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    public const string MUSIC_VOLUME = "music volume";
    public const string SFX_VOLUME = "sfx volume";

    [SerializeField] TextMeshProUGUI textSFXVol, textMusicVol;


    private void Update()
    {
        textSFXVol.text = PlayerPrefs.GetInt(SFX_VOLUME,10).ToString();

        textMusicVol.text = PlayerPrefs.GetInt(MUSIC_VOLUME, 8).ToString();

    }

    public void IncrementVolume(string tag)
    {
        if (tag == "Music")
        {
            PlayerPrefs.SetInt(MUSIC_VOLUME, PlayerPrefs.GetInt(MUSIC_VOLUME, 8) + 1);
            if(PlayerPrefs.GetInt(MUSIC_VOLUME, 8) > 10)
            {
                PlayerPrefs.SetInt(MUSIC_VOLUME, 10);
            }
            else if (PlayerPrefs.GetInt(MUSIC_VOLUME, 8) < 0f)
            {
                PlayerPrefs.SetInt(MUSIC_VOLUME, 0);
            }
            Bus bus = GameObject.FindWithTag(tag).GetComponent<Bus>();
            bus.busVolume = -40 + (50 / 10) * PlayerPrefs.GetInt(MUSIC_VOLUME, 8);
        }
        if (tag == "SFX")
        {
            PlayerPrefs.SetInt(SFX_VOLUME, PlayerPrefs.GetInt(SFX_VOLUME, 10) + 1);
            if (PlayerPrefs.GetInt(SFX_VOLUME, 10) > 10)
            {
                PlayerPrefs.SetInt(SFX_VOLUME, 10);
            }
            else if (PlayerPrefs.GetInt(SFX_VOLUME, 10) < 0)
            {
                PlayerPrefs.SetInt(SFX_VOLUME, 0);
            }
            Bus bus = GameObject.FindWithTag(tag).GetComponent<Bus>();
            bus.busVolume = -40 + (50 / 10) * PlayerPrefs.GetInt(SFX_VOLUME, 10);
        }
    }
    public void DecrementVolume(string tag)
    {
        if (tag == "Music")
        {
            PlayerPrefs.SetInt(MUSIC_VOLUME, PlayerPrefs.GetInt(MUSIC_VOLUME, 8) - 1);
            if (PlayerPrefs.GetInt(MUSIC_VOLUME) > 10)
            {
                PlayerPrefs.SetInt(MUSIC_VOLUME, 10);
            }
            else if (PlayerPrefs.GetInt(MUSIC_VOLUME, 8) < 0)
            {
                PlayerPrefs.SetInt(MUSIC_VOLUME, 0);
            }
            Bus bus = GameObject.FindWithTag(tag).GetComponent<Bus>();
            bus.busVolume = -40 + (50 / 10) * PlayerPrefs.GetInt(MUSIC_VOLUME, 8);
        }
        if (tag == "SFX")
        {
            PlayerPrefs.SetInt(SFX_VOLUME, PlayerPrefs.GetInt(SFX_VOLUME, 10) - 1);
            if (PlayerPrefs.GetInt(SFX_VOLUME) > 10)
            {
                PlayerPrefs.SetInt(SFX_VOLUME, 10);
            }
            else if (PlayerPrefs.GetInt(SFX_VOLUME, 10) < 0f)
            {
                PlayerPrefs.SetInt(SFX_VOLUME, 0);
            }
            Bus bus = GameObject.FindWithTag(tag).GetComponent<Bus>();
            bus.busVolume = -40 + (50 / 10) * PlayerPrefs.GetInt(SFX_VOLUME,10);
        }
    }
}
