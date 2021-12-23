using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI decimalText;
    private float Timer;


    private void Update()
    {
        Timer += Time.deltaTime;

        TimeSpan t = TimeSpan.FromSeconds(Timer);
        timeText.text = string.Format("{0:D1}",  t.Seconds);
        decimalText.text = string.Format("{0:D2}", Mathf.RoundToInt(t.Milliseconds / 10f));

        //timeText.text = t.ToString(@"ss");
        //decimalText.text = t.ToString(@"ff");
        //timeText.text = string.Format("{0:D2}:{1:D2}",t.Seconds, t.Milliseconds);

    }
}
