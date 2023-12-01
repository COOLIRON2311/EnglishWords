using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlScript : MonoBehaviour
{
    public DataScript data;
    public Toggle mainToggle;
    SortedSet<int> checkedItems;
    Button hRButton;
    Transform togglePanel;
    Text hText;
    // Start is called before the first frame update
    void Start()
    {
        togglePanel = GameObject.Find("TogglePanel").transform;
        togglePanel.GetChild(data.TestType).GetComponent<Toggle>().isOn = true;
        for (int i = 0; i < 3; i++)
            SetToggleHandler(togglePanel.GetChild(i).GetComponent<Toggle>(), i);

        checkedItems = data.TestTopics;
        hRButton = GameObject.Find("HRButton").GetComponent<Button>();

        for (int i = 0; i < data.TopicCount; i++)
        {
            var t = Instantiate(mainToggle);
            t.GetComponentInChildren<Text>().text = data.Topic(i);
            t.transform.SetParent(gameObject.transform);
            t.transform.localScale = Vector2.one;
            if (checkedItems.Contains(i))
                t.GetComponent<Toggle>().isOn = true;
            SetItemHandler(t, i);
        }

        var es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        es.SetSelectedGameObject(transform.GetChild(data.S2ItemIndex).gameObject);

        var scrollbar = GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        scrollbar.value = data.S2ScrollbarValue;
        scrollbar.onValueChanged.AddListener(v => data.S2ScrollbarValue = v);

        var child0 = transform.GetChild(0).GetComponent<Toggle>();
        for (int i = 0; i < 3; i++)
        {
            data.SetNavigationDown(togglePanel.GetChild(i).GetComponent<Toggle>(), child0);
        }

        hText = GameObject.Find("HText").GetComponent<Text>();
        SetHeader();
    }

    void SetToggleHandler(Toggle t, int i)
    {
        t.onValueChanged.AddListener(b =>
        {
            if (b)
                data.TestType = i;
        });
    }

    void SetItemHandler(Toggle t, int i)
    {
        t.onValueChanged.AddListener(b =>
        {
            if (b)
                checkedItems.Add(i);
            else
                checkedItems.Remove(i);
            data.TestTopics = checkedItems;
            hRButton.interactable = checkedItems.Count > 0;
            SetHeader();
        });
    }

    void Update()
    {
        if (Input.inputString.Length > 0)
        {
            int i = Input.inputString[0] - '1';
            if (i >= 0 && i <= 2)
            {
                togglePanel.GetChild(i).GetComponent<Toggle>().isOn = true;
            }
        }
    }

    void SetHeader()
    {
        var s = $"Контроль \n{data.Level + 1}:{data.TestTopicsToString()}";
        if (s.Length > 25)
            s = s[..25] + "...";
        hText.text = s;
    }
}
