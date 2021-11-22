using UnityEngine;
using TMPro;
using System;

public class CountDown : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    [Header("Time Values")]
    [SerializeField] [Range(0, 60)] private int m_seconds;
    [SerializeField] [Range(0, 60)] private int m_minutes;

    public Color fontColor;

    public bool showMilliseconds;

    private float currentSeconds;
    private int timerDefault;

    void Start()
    {
        timerText.color = fontColor;
        timerDefault = 0;
        timerDefault += (m_seconds + (m_minutes * 60));
        currentSeconds = timerDefault;
    }

    void Update()
    {
        if (!GameManager.Instance.m_gameOver && (currentSeconds -= Time.deltaTime) <= 0)
        {
            TimeUp();
            GameManager.Instance.GameOver();
        }
        else
        {
            if (showMilliseconds)
                timerText.text = TimeSpan.FromSeconds(currentSeconds).ToString(@"mm\:ss\:ff");
            else
                timerText.text = TimeSpan.FromSeconds(currentSeconds).ToString(@"mm\:ss");
        }

        
    }

    private void TimeUp()
    {
        if (showMilliseconds)
            timerText.text = "00:00:00";
        else
            timerText.text = "00:00";
    }
}