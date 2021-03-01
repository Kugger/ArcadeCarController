using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddTaskPanelState : MonoBehaviour
{
    public GameObject addTaskPanel;
    public GameObject DateTestScript;
    DateTest presentTime;

    public Text dayText;
    public Text monthText;
    public Text yearText;
    public Text hourText;
    public Text minuteText;

    public InputField TitleField;
    public InputField TextField;

    private void OnEnable()
    {
        // info o aktualnym czasie z DateTest.cs
        presentTime = DateTestScript.GetComponent<DateTest>();

        dayText.text = presentTime.DTday;
        monthText.text = presentTime.DTmonth;
        yearText.text = presentTime.DTfullyear;
        hourText.text = presentTime.DThour;
        minuteText.text = presentTime.DTminute;
    }

    private void OnDisable()
    {
        TitleField.text = "";
        TextField.text = "";
    }
}
