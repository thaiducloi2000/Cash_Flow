using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class History_Prefabs : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Money;
    [SerializeField] private TextMeshProUGUI Match_Time;
    [SerializeField] private TextMeshProUGUI Match_Date;

    public void SetText(HistoryDTO history)
    {
        Money.text = history.TotalMoney.ToString();
        //TimeSpan parsedTime = TimeSpan.Parse(history.MatchTime);
        TimeSpan parsedTime;
        string[] substrings = history.StartTime.ToString().Split(' ');
        string date = substrings[0];
        Match_Date.text = date;

        if (TimeSpan.TryParse(history.MatchTime, out parsedTime))
        {
            TimeSpan minutesAndSeconds = new TimeSpan(0, parsedTime.Minutes, parsedTime.Seconds);

            Match_Time.text = minutesAndSeconds.ToString(@"mm\:ss");
        }
        else
        {
            Debug.LogError("Failed to parse time.");
        }
    }
}
