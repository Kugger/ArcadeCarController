using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateTest : MonoBehaviour
{
    public Text dataText;

    void Update()
    {
        string days = System.DateTime.UtcNow.ToLocalTime().ToString("dd");
        string months = System.DateTime.UtcNow.ToLocalTime().ToString("MM");
        string years = System.DateTime.UtcNow.ToLocalTime().ToString("yy");

        string hours = System.DateTime.UtcNow.ToLocalTime().ToString("HH");
        string minutes = System.DateTime.UtcNow.ToLocalTime().ToString("mm");
        string seconds = System.DateTime.UtcNow.ToLocalTime().ToString("ss");

        dataText.text = "Today is " + days + "-" + months + "-" + years + " | " + hours  + ":" + minutes + ":" + seconds;
    }
}