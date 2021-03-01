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
    public Color woodPecker = new Color(111f, 69f, 14f, 1f);
    public Color sandCastle = new Color(219f, 168f, 66f, 1f);

    [Header("Date and Time Texts")]
    public Text dayText;
    public Text monthText;
    public Text yearText;
    public Text hourText;
    public Text minuteText;

    [Header("Up Buttons")]
    public Button dayButtonUp;
    public Button monthButtonUp;
    public Button yearButtonUp;
    public Button hourButtonUp;
    public Button minuteButtonUp;

    [Header("Down Buttons")]
    public Button dayButtonDown;
    public Button monthButtonDown;
    public Button yearButtonDown;
    public Button hourButtonDown;
    public Button minuteButtonDown;


    public bool disabledButton;
    public bool leapYear;
    public bool nowDay, nowMonth, nowHour;

    public Button CreateTaskButton;

    int intDay = 1, intMonth = 1, intYear = 1980, intHour = 00, intMinute = 00;

    public int minDay = 1;
    public int minMonth = 1;
    public int minYear = 1980;
    public int minHour = 00;
    public int minMinute = 00;

    public int maxDay = 31;
    public int maxMonth = 12;
    public int maxHour = 23;
    public int maxMinute = 59;

    private int presentDay = 1, presentMonth = 1, presentYear = 1, presentHour = 1, presentMinute = 1;

    public class NewTaskList
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

        public NewTaskList(string title, string text, Color color, string day, string month, string year, string hour, string minute, int index)
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

    string jsonFileName = "TaskManager.json";

    void Start()
    {
        // sciezka do pliku z danymi do taskow
        // filePath = Application.persistentDataPath + "/checklist.txt";
        filePath = Path.Combine(Application.persistentDataPath, jsonFileName);
        readTaskFromJson();

        dayText.text = System.DateTime.Now.ToString("dd");
        monthText.text = System.DateTime.Now.ToString("MM");
        yearText.text = System.DateTime.Now.ToString("yyyy");
        hourText.text = System.DateTime.Now.ToString("HH");
        minuteText.text = System.DateTime.Now.ToString("mm");

        intDay = int.Parse(dayText.text);
        intMonth = int.Parse(monthText.text);
        intYear = int.Parse(yearText.text);
        intHour = int.Parse(hourText.text);
        intMinute = int.Parse(minuteText.text);

        minDay = intDay;
        minMonth = intMonth;
        minYear = intYear;
        minHour = intHour;
        minMinute = intMinute;
    }


    public void createNewTask()
    {
        string temp = TitleField.text;
        string temp2 = TextField.text;
        Color temp3 = flatPlum;
        createButtonPressed(temp, temp2, temp3, minDay.ToString(), minMonth.ToString(), minYear.ToString(), minHour.ToString(), minMinute.ToString());
    }

    public void createButtonPressed(string objTitle, string objText, Color objColor, string objDay, string objMonth, string objYear, string objHour, string objMinute, int objIndex = 0)
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

        // ustawiamy jaki tytul, opis, kolor, date i index taskowi 
        taskObject.SetObjectInfo(objTitle, objText, objColor, objDay, objMonth, objYear, objHour, objMinute, index);
        /*
        Debug.Log("taskObject name: " + taskObject.objTitle);
        Debug.Log("taskObject text: " + taskObject.objText);
        Debug.Log("taskObject index: " + taskObject.objIndex);
        Debug.Log("taskObject color: " + taskObject.objColor);
        */

        // znajdujemy componenty tekstu i wsadzamy je w liste
        Text[] textsInTask = taskObject.GetComponentsInChildren<Text>();

        // nadajemy kazdemu odpowiednia wartosc podana przez uzytkownia
        textsInTask[0].text = objTitle;
        textsInTask[1].text = objText;
        textsInTask[2].text = index.ToString();
        textsInTask[3].text = objDay + "-" + objMonth + "-" + objYear + "   " + objHour + ":" + objMinute;

        // znajdujemy obrazek w prefabie i zmieniamy jego kolor
        Image[] imageInTask = taskObject.GetComponentsInChildren<Image>();
        imageInTask[1].color = selectedColor;
        imageInTask[2].color = selectedColor;



        tasksList.Add(taskObject);

        saveTaskToJson();

        //Debug.Log("Da sie to wyswietlic?" + tasksList[0].objTitle);

    }

    void saveTaskToJson()
    {
        string fileContent = "";

        for (int i = 0; i < tasksList.Count; i++)
        {
            NewTaskList temp = new NewTaskList(tasksList[i].objTitle, tasksList[i].objText, tasksList[i].objColor, tasksList[i].objDay,
                tasksList[i].objMonth, tasksList[i].objYear, tasksList[i].objHour, tasksList[i].objMinute, tasksList[i].objIndex);
            fileContent += JsonUtility.ToJson(temp) + "\n";
        }

        System.IO.File.WriteAllText(filePath, fileContent);
    }

    void readTaskFromJson()
    {
        string fileJson = "";

        fileJson = System.IO.File.ReadAllText(filePath);

        string[] splitTextContent = fileJson.Split('\n');

        foreach (string textContent in splitTextContent)
        {
            if (textContent.Trim() != "")
            {
                NewTaskList temp = JsonUtility.FromJson<NewTaskList>(textContent.Trim());
                createButtonPressed(temp.objTitle, temp.objText, temp.objColor, temp.objDay, temp.objMonth, temp.objYear, temp.objHour, temp.objMinute, temp.objIndex);
            }
        }
    }

    private void OnEnable()
    {
       // Debug.Log("Do you see me?");

        // info o aktualnym czasie z DateTest.cs
        presentTime = DateTestScript.GetComponent<DateTest>();

        presentDay = int.Parse(presentTime.DTday);
        presentMonth = int.Parse(presentTime.DTmonth);
        presentYear = int.Parse(presentTime.DTfullyear);
        presentHour = int.Parse(presentTime.DThour);
        presentMinute = int.Parse(presentTime.DTminute);

        if (addTaskPanel.activeSelf)
        {
            minDay = presentDay;
            minMonth = presentMonth;
            minYear = presentYear;
            minHour = presentHour;
            minMinute = presentMinute;

            //Debug.Log("Tylko raz");
        }
    }

    private void OnDisable()
    {
        TitleField.text = "";
        TextField.text = "";
    }

    public bool CorrectDateCheck()
    {
        if (intDay < presentDay | intMonth < presentMonth | intYear < presentYear | intHour < presentHour | intMinute < presentMinute)
            if (nowDay == true && nowMonth == true && nowHour == true)
                disabledButton = true;

        return disabledButton;
    }

    public bool CorrectFieldCheck()
    {
        if (TitleField.text.Length == 0)
            disabledButton = true;

        return disabledButton;
    }

    // w DateTest01.cs dale metode Update w public jbc
    void Update()
    {
        Debug.Log("Da sie to wyswietlic?" + tasksList.Count);

        // sprawdzanie czy rok jest przestepny
        if (intYear % 4 == 0)
        {
            if (intYear % 100 == 0)
            {
                if (intYear % 400 == 0)
                    leapYear = true;
                else
                    leapYear = false;
            }
            else
                leapYear = true;
        }
        else
            leapYear = false;

        // ograniczanie minimum dnia
        if (minDay >= intDay & minMonth == intMonth & minYear == intYear)
        {
            nowDay = true;
            dayButtonDown.interactable = false;
        }
        else if (intDay == 1)
        {
            nowDay = false;
            dayButtonDown.interactable = false;
        }
        else
        {
            nowDay = false;
            dayButtonDown.interactable = true;
        }

        // ograniczanie maksimum dnia
        switch (intMonth)
        {
            case 1:
                maxDay = 31;
                break;
            case 2:
                maxDay = 28;
                break;
            case 3:
                maxDay = 31;
                break;
            case 4:
                maxDay = 30;
                break;
            case 5:
                maxDay = 31;
                break;
            case 6:
                maxDay = 30;
                break;
            case 7:
                maxDay = 31;
                break;
            case 8:
                maxDay = 31;
                break;
            case 9:
                maxDay = 30;
                break;
            case 10:
                maxDay = 31;
                break;
            case 11:
                maxDay = 30;
                break;
            case 12:
                maxDay = 31;
                break;
        }

        if (intMonth == 2 & leapYear == true)
            maxDay = 29;


        if (maxDay <= intDay)
        {
            dayText.text = maxDay.ToString();
            dayButtonUp.interactable = false;
        }
        else
            dayButtonUp.interactable = true;


        // ograniczanie minimum miesiaca
        if (minMonth >= intMonth & minYear == intYear)
        {
            if (nowDay == true)
            {
                intDay = minDay;
                dayText.text = minDay.ToString();
            }
            nowMonth = true;
            monthButtonDown.interactable = false;
        }
        else if (intMonth == 1)
        {
            nowMonth = false;
            monthButtonDown.interactable = false;
        }
        else
        {
            nowMonth = false;
            monthButtonDown.interactable = true;
        }


        // ograniczanie maksimum miesiaca
        if (maxMonth <= intMonth & minYear == intYear)
            monthButtonUp.interactable = false;
        else if (intMonth == 12)
            monthButtonUp.interactable = false;
        else
            monthButtonUp.interactable = true;


        // ograniczanie minimum roku
        if (minYear >= intYear)
        {
            if (nowMonth == true)
            {
                intMonth = minMonth;
                monthText.text = minMonth.ToString();
            }
            yearButtonDown.interactable = false;
        }
        else
            yearButtonDown.interactable = true;


        // ograniczanie minimum godzin
        if (minHour >= intHour & minDay == intDay & minMonth == intMonth & minYear == intYear)
        {
            nowHour = true;
            hourText.text = minHour.ToString();
            hourButtonDown.interactable = false;
        }
        else if (intHour == 00)
        {
            nowHour = false;
            hourButtonDown.interactable = false;
        }
        else
        {
            nowHour = false;
            hourButtonDown.interactable = true;
        }

        // ograniczanie maksimum godzin
        if (maxHour <= intHour)
            hourButtonUp.interactable = false;
        else
            hourButtonUp.interactable = true;


        // ograniczanie minimum minut
        if (minMinute >= intMinute & minDay == intDay & minMonth == intMonth & minYear == intYear & minHour == intHour)
        {
            intMinute = minMinute;
            minuteText.text = intMinute.ToString();
            minuteButtonDown.interactable = false;
        }
        else if (intMinute == 00)
            minuteButtonDown.interactable = false;
        else
            minuteButtonDown.interactable = true;

        // ograniczanie maksimum minut
        if (maxMinute <= intMinute)
            minuteButtonUp.interactable = false;
        else
            minuteButtonUp.interactable = true;


        bool disabledByTitle = CorrectFieldCheck();
        bool disabledByDate = CorrectDateCheck();

        if (disabledByTitle == true)
        {
            disabledByDate = true;
            disabledButton = true;
        }

        if (disabledByDate == true)
        {
            minDay = presentDay;
            minMonth = presentMonth;
            minYear = presentYear;
            minHour = presentHour;
            minMinute = presentMinute;
        }


        if (disabledButton == true)
        {
            CreateTaskButton.interactable = false;
            //Debug.Log("CreateButton disabled" + intMinute + " " + presentMinute);

            wrongDataText.SetActive(true);
            StartCoroutine(DisplayWrongInfo());

            minDay = presentDay;
            minMonth = presentMonth;
            minYear = presentYear;
            minHour = presentHour;
            minMinute = presentMinute;
        }
        else
        {
            CreateTaskButton.interactable = true;
            //Debug.Log("CreateButton able" + intMinute + " " + presentMinute);
        }

        //Debug.Log("disabledButton state: " + disabledButton);
    }

    public IEnumerator DisplayWrongInfo()
    {
        yield return new WaitForSeconds(3.0f);
        disabledButton = false;
        CreateTaskButton.interactable = true;
        wrongDataText.SetActive(false);
    }

    // przypisywanie przyciskow / mocking the buttons
    public void DayButtonUp(Text dayText)
    {
        intDay++;
        dayText.text = intDay.ToString();
    }

    public void DayButtonDown(Text dayText)
    {
        intDay--;
        dayText.text = intDay.ToString();
    }

    public void MonthButtonUp(Text monthText)
    {
        intMonth++;
        monthText.text = intMonth.ToString();
    }

    public void MonthButtonDown(Text monthText)
    {
        intMonth--;
        monthText.text = intMonth.ToString();
    }

    public void YearButtonUp(Text yearText)
    {
        intYear++;
        yearText.text = intYear.ToString();
    }

    public void YearButtonDown(Text yearText)
    {
        intYear--;
        yearText.text = intYear.ToString();
    }

    public void HourButtonUp(Text hourText)
    {
        intHour++;
        hourText.text = intHour.ToString();
    }

    public void HourButtonDown(Text hourText)
    {
        intHour--;
        hourText.text = intHour.ToString();
    }

    public void MinuteButtonUp(Text minuteText)
    {
        intMinute++;
        minuteText.text = intMinute.ToString();
    }

    public void MinuteButtonDown(Text minuteText)
    {
        intMinute--;
        minuteText.text = intMinute.ToString();
    }
}