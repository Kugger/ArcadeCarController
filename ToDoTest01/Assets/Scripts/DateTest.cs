using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateTest : MonoBehaviour
{
    public Text dataText;

    public string DTday, DTmonth, DTyear, DTfullyear, DThour, DTminute, DTseconds;

    void Update()
    {
        var presentTime = System.DateTime.UtcNow.ToLocalTime();

        DTday = presentTime.ToString("dd");
        DTmonth = presentTime.ToString("MM");
        DTyear = presentTime.ToString("yy");
        DTfullyear = presentTime.ToString("yyyy");
        DThour = presentTime.ToString("HH");
        DTminute = presentTime.ToString("mm");
        DTseconds = presentTime.ToString("ss");

        dataText.text = "Today is " + DTday + "-" + DTmonth + "-" + DTyear + " | " + DThour  + ":" + DTminute + ":" + DTseconds;
    }
}