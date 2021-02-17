using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NewTask : MonoBehaviour
{
    public string objTitle;

    public string objText;

    public int objIndex;

    public Color objColor = new Color(0f, 111f, 73f);

    private Text itemText;

    void Start()
    {
        itemText = GetComponentInChildren<Text>();

        itemText.text = objTitle;
    }

    public void SetObjectInfo(string title, string text, Color color, int index)
    {
        this.objTitle = title;
        this.objText = text;
        this.objColor = color;
        this.objIndex = index;
    }

}
