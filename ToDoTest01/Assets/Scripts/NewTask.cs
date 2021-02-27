using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NewTask : MonoBehaviour
{
    public string objTitle;

    public string objText;

    public string objDay;
    public string objMonth;
    public string objYear;
    public string objHour;
    public string objMinute;

    public int objIndex;

    public Color objColor = new Color(0f, 111f, 73f);

    private Text itemText;

    void Start()
    {
        itemText = GetComponentInChildren<Text>();

        itemText.text = objTitle;
    }

    public void SetObjectInfo(string title, string text, Color color, string day, string month, string year, string hour, string minute, int index)
    {
        this.objTitle = title;
        this.objText = text;
        this.objColor = color;
        this.objIndex = index;

        this.objDay = day;
        this.objMonth = month;
        this.objYear = year;
        this.objHour = hour;
        this.objMinute = minute; 
    }

}
