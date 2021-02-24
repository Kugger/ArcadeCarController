using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Text;
using System.IO;

public class TaskManager : MonoBehaviour
{
    [Header("Transforms")]
    public Transform content;

    // public NewTask newTask;

    [Header("GameObjects")]
    public GameObject addTaskPanel;
    public GameObject checklistItemPrefab;
    public GameObject DateTestScript;
    public GameObject wrongDataText;

    DateTest presentTime;

    // sciezka do pliku gdzie bedziemy przechowywac nasze taski
    string filePath;

    private List<NewTask> tasksList = new List<NewTask>();

    private Toggle selectedToggle;

    [Header("Inputs")]
    public InputField TitleField;
    public InputField TextField;
    public ToggleGroup ColorToggleGroup;

    [Header("Colors")]
    public Color hardMint = new Color(0f, 111f, 73f, 1f);
    public Color modernSuit = new Color(67f, 117f, 219f, 1f);
    public Color bookCover = new Color(219f, 67f, 82f, 1f);
    public Color flatPlum = new Color(120f, 36f, 184f, 1f);
    public Color woodPecker =  new Color(111f, 69f, 14f, 1f);
    public Color sandCastle = new Color(219f, 168f, 66f, 1f);

    [Header("DateAndTimeTexts")]
    public Text dayText;
    public Text monthText;
    public Text yearText;
    public Text hourText;
    public Text minuteText;

    [Header("ButtonsUp")]
    public Button dayButtonUp;
    public Button monthButtonUp;
    public Button yearButtonUp;
    public Button hourButtonUp;
    public Button minuteButtonUp;

    [Header("ButtonsDown")]
    public Button dayButtonDown;
    public Button monthButtonDown;
    public Button yearButtonDown;
    public Button hourButtonDown;
    public Button minuteButtonDown;

    public Button CreateTaskButton;



    void Start()
    {
        filePath = Application.persistentDataPath + "/checklist.txt";
    }

    public void createNewTask() 
    {
        string temp = TitleField.text;
        string temp2 = TextField.text;
        createButtonPressed(temp, temp2, hardMint);

    }

    public void createButtonPressed(string objTitle, string objText, Color objColor, int objIndex = 0)
    {
        // inicjalizowanie tasku uzywajac przygotowanego prefabu
        GameObject task = Instantiate(checklistItemPrefab);
        // dodatnie taskowi "porzadku"
        task.transform.SetParent(content);
        // tworzenie tasku
        NewTask taskObject = task.GetComponent<NewTask>();

        // indeksowanie tasków
        int index = 0;
        if (tasksList.Count == 0)
            index = 0;
        else
            index = tasksList.Count;

        // zbieranie ktory color toggle zostal wybrany
        foreach (var toggle in ColorToggleGroup.ActiveToggles())
        {
            if (toggle.isOn)
                selectedToggle = toggle;
        }

        // wybieranie koloru z wybranego toggle w AddTaskPanel
        Color selectedColor = selectedToggle.GetComponentInChildren<Image>().color;

        // ustawiamy jaki tytul, opis, kolor i index taskowi 
        taskObject.SetObjectInfo(objTitle, objText, objColor, index);
        Debug.Log("taskObject name: " + taskObject.objTitle);
        Debug.Log("taskObject text: " + taskObject.objText);
        Debug.Log("taskObject index: " + taskObject.objIndex);
        Debug.Log("taskObject color: " + taskObject.objColor);

        // znajdujemy componenty tekstu i wsadzamy je w liste
        Text[] textsInTask = taskObject.GetComponentsInChildren<Text>();

        // nadajemy kazdemu odpowiednia wartosc podana przez uzytkownia
        textsInTask[0].text = objTitle;
        textsInTask[1].text = objText;
        textsInTask[2].text = index.ToString();

        // znajdujemy obrazek w prefabie i zmieniamy jego kolor
        Image[] imageInTask = taskObject.GetComponentsInChildren<Image>();
        imageInTask[1].color = selectedColor;

        tasksList.Add(taskObject);
    }
}
