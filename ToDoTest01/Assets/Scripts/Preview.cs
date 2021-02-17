/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChecklistManager : MonoBehaviour
{
    public Transform content;

    public GameObject addPannel;

    public Button createButton;

    public GameObject checklistItemPrefab;


    // string filePath = Application.persistentDataPath + "/checklist.txt";
    string filePath;


    private List<ChecklistObjectTut> checklistObjects = new List<ChecklistObjectTut>();

    private InputField[] addInputFields;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/checklist.txt";
        addInputFields = addPannel.GetComponentsInChildren<InputField>();

        createButton.onClick.AddListener(delegate { CreateChecklistItem(addInputFields[0].text, addInputFields[1].text); });
    }

    public void SwitchMode(int mode)
    {
        switch (mode)
        {
            // Regular checklist mode
            case 0:
                addPannel.SetActive(false);
                break;

            // AddPanel visible
            case 1:
                addPannel.SetActive(true);
                break;
        }
    }

    void CreateChecklistItem(string name, string type)
    {
        GameObject item = Instantiate(checklistItemPrefab);

        item.transform.SetParent(content);

        ChecklistObjectTut itemObject = item.GetComponent<ChecklistObjectTut>();

        int index = 0;
        if (checklistObjects.Count > 0)
            index = checklistObjects.Count - 1;

        itemObject.SetObjectInfo(name, type, index);

        checklistObjects.Add(itemObject);
        ChecklistObjectTut temp = itemObject;
        itemObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckItem(temp); });

        SwitchMode(0);
    }

    void CheckItem(ChecklistObjectTut item)
    {
        checklistObjects.Remove(item);
        Destroy(item.gameObject);
    }

}


*/