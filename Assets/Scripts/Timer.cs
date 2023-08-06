using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] float startSeconds;
    [SerializeField] TextMeshProUGUI timerText;
    public float timer, mins, secs;
    public bool timerRunning;
    private void Awake()
    {
        timer = startSeconds;
    }

    private void Update()
    {
        if (timerRunning) 
        {
            timer += Time.deltaTime;
        }
        mins = Mathf.FloorToInt(timer / 60);
        secs = Mathf.FloorToInt(timer % 60);
        timerText.text = mins.ToString("00") + ":" + secs.ToString("00");
    }

    public void ToggleTimerRunning(bool val)
    {
        timerRunning = val;
    }

    public void ResetTimer()
    {
        timer = 0f;
    }


}
