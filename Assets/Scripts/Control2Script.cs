using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Control2Script : MonoBehaviour
{
    public DataScript data;
    EventSystem es;
    Text headerText;
    Image progressBar;
    string title;
    float progress;
    string[] labels = new string[6];
    Button[] buttons = new Button[6];
    Text[] texts = new Text[6];
    // Start is called before the first frame update
    void Start()
    {
        es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        headerText = GameObject.Find("HText").GetComponent<Text>();
        progressBar = GameObject.Find("ProgressBar").GetComponent<Image>();
        for (int i = 0; i < 6; i++)
        {
            buttons[i] = transform.GetChild(i).GetComponent<Button>();
            buttons[i].onClick.AddListener(OnClickHandler);
            texts[i] = transform.GetChild(i).GetComponentInChildren<Text>();
        }
        data.InitTest();
        data.NextQuestion(labels, out title, out progress);
        UpdateTestInfo();
        UpdateQuestionInfo();
    }

    void UpdateTestInfo()
    {
        headerText.text = title;
        progressBar.fillAmount = progress;
    }

    void UpdateQuestionInfo()
    {
        for (int i = 0; i < 6; i++)
        {
            texts[i].text = labels[i];
            buttons[i].interactable = true;
        }
        es.SetSelectedGameObject(buttons[0].gameObject);
    }

    void OnClickHandler()
    {
        int idx = es.currentSelectedGameObject.transform.GetSiblingIndex();
        if (idx == 0) // question button
        {
            data.AdditionalTestAction();
            return;
        }
        // other buttons - user tried to answer the question
        string res = data.CheckAnswer(idx, ref title, ref progress);
        if (res == "") // correct answer
        {
            bool b = data.NextQuestion(labels, out title, out progress);
            UpdateTestInfo();
            if (!b)
            {
                es.SetSelectedGameObject(GameObject.Find("HRButton"));
                for (int i = 0; i < 6; i++)
                    buttons[i].interactable = false;
            }
            else
                UpdateQuestionInfo();
        }
        else
        {
            texts[idx].text = res;
            buttons[idx].interactable = false;
            UpdateTestInfo();
            es.SetSelectedGameObject(buttons[0].gameObject);
        }
    }

    void OnDestroy() => data.SaveResult();

    void Update()
    {
        if (Input.inputString.Length > 0)
        {
            int i = Input.inputString[0] - '0';
            if (i >= 0 && i <= 5 && buttons[i].interactable)
            {
                es.SetSelectedGameObject(buttons[i].gameObject);
                OnClickHandler();
            }
        }
    }
}
