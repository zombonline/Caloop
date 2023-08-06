using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    //This script will contain some custom logic to be used by the UI. Some ui components need to be enabled by other scripts and not buttons
    //so methods for doing so will be here aswell as some custom button logic such as resetting game progress.
    [SerializeField] GameObject wndwResetProgressWarning,wndwResults, wndwNoMoreLevels, wdnwHint, btnSkipLevel;
    [SerializeField] Volume volume;
    [SerializeField] TextAsset hintTextFile;
    [SerializeField] string[] hints;

    [SerializeField] TextMeshProUGUI textHint;



    private void Awake()
    {
        hints = hintTextFile.text.Split(Environment.NewLine,
                            StringSplitOptions.RemoveEmptyEntries);
    }

    private void Update()
    {
        EnableSkipButtonAfterTime();
    }

    public void LoadMenuMusic()
    {
        FindObjectOfType<FMODController>().ChangeMusicState(0);
    }

    private void EnableSkipButtonAfterTime()
    {
        if (FindObjectOfType<Timer>().timer > 90f)
        {
            btnSkipLevel.SetActive(true);
        }
        else
        {
            btnSkipLevel.SetActive(false);
        }
    }

    public void ResetProgress()
    {
        LevelLoader.SetProgress(0);
        SceneManager.LoadScene("Splash");
    }

    public void OpenNoMoreLevelsScreen()
    {
        wndwNoMoreLevels.SetActive(true);
    }
    public void OpenHintScreen()
    {
        wdnwHint.SetActive(true);
    }
    public void FadeInBlur()
    {
        StartCoroutine(FadeCoroutine(volume.weight, .5f, .125f)); 
    }
    public void FadeOutBlur()
    {
        StartCoroutine(FadeCoroutine(volume.weight, 0f, .125f));
    }
    IEnumerator FadeCoroutine(float start, float end, float duration)
    {
        while(volume.weight != end)
        {
            if (start < end)
            {
                volume.weight += .01f;
                if(volume.weight > end)
                {
                    volume.weight = end;
                }
            }
            else if (start > end)
            {
                volume.weight -= .01f;
                if (volume.weight < end)
                {
                    volume.weight = end;
                }
            }
            yield return new WaitForSeconds(duration *.01f);
        }
    }


   
    public void OpenResultsScreen()
    { 
        //public method to enable a UI component, made public to be used by another script (The wheel when a spin is over and the player has won).
        wndwResults.SetActive(true);
    }
    public void LoadHint(string hint)
    {
        //if the hint passed in is empty, a random hint from a text file will be used in it's place.
        if (hint == "")
        {
            hint = hints[UnityEngine.Random.Range(0, hints.Length)];
        }
        textHint.text = hint;

    }
}
