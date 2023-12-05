using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Canvas mainCanvas;
    public Button menuItem;
    protected EventSystem es;
    CanvasGroup mainCanvasGroup;
    GameObject mainSelectedObject;
    Transform menuPanel;
    // Start is called before the first frame update
    protected void Start()
    {
        gameObject.SetActive(false);
    }

    protected void InitMenu(string[] titles, System.Action<int> menuHandler)
    {
        es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        menuPanel = GameObject.Find("MenuPanel").transform;
        for (int i = 0; i < titles.Length; i++)
            AddCommand(titles[i], menuHandler, i);
        AddCommand("Отмена", null, 0);
    }

    void AddCommand(string title, System.Action<int> menuHandler, int index)
    {
        var b = Instantiate(menuItem);
        b.GetComponentInChildren<Text>().text = title;
        b.transform.SetParent(menuPanel);
        b.transform.localScale = Vector2.one;
        b.onClick.AddListener(CloseMenu);
        if (menuHandler != null)
            b.onClick.AddListener(() => menuHandler(index));
    }

    public void ShowMenu()
    {
        mainCanvasGroup = mainCanvas.GetComponent<CanvasGroup>();
        if (mainCanvasGroup == null)
        {
            Debug.LogError("The main canvas doesn't contain the CanvasGroup component.");
            return;
        }
        mainCanvasGroup.interactable = false;
        mainSelectedObject = es.currentSelectedGameObject;
        gameObject.SetActive(true);
        es.SetSelectedGameObject(menuPanel.GetChild(0).gameObject);
    }

    void CloseMenu()
    {
        gameObject.SetActive(false);
        mainCanvasGroup.interactable = true;
        es.SetSelectedGameObject(mainSelectedObject);
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CloseMenu();
    }

}
