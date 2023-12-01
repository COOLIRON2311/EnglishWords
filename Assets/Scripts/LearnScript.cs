using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LearnScript : MonoBehaviour
{
    public DataScript data;
    public Button mainButton;
    // Start is called before the first frame update
    void Start()
    {
        // Setup DropDown
        var dd = GameObject.Find("MainDropdown").GetComponent<Dropdown>();
        dd.value = data.Level;
        dd.onValueChanged.AddListener(OnValueChangeHandler);

        // Create buttons with current level topics
        for (int i = 0; i < data.TopicCount; i++)
        {
            var b = Instantiate(mainButton);
            b.GetComponentInChildren<Text>().text = data.Topic(i);
            b.transform.SetParent(transform);
            b.transform.localScale = Vector2.one;
        }

        // Set selection to first button
        var es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        es.SetSelectedGameObject(transform.GetChild(data.S1ItemIndex).gameObject);
        var scrollbar = GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        scrollbar.value = data.S1ScrollbarValue;
        scrollbar.onValueChanged.AddListener(v => data.S1ScrollbarValue = v);
    }

    void Update()
    {
        if (Input.inputString.Length > 0)
        {
            int i = Input.inputString[0] - '1';
            if (i >= 0 && i <= 3)
                OnValueChangeHandler(i);
        }
    }

    void OnValueChangeHandler(int i)
    {
        if (i != data.Level)
        {
            data.SetLevel(i);
            // Reload scene to refresh its contents
            SceneManager.LoadScene((int)Scenes.S1Learn);
        }
    }
}
