using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeaderScript : MonoBehaviour
{
    public DataScript data;
    public Canvas menuCanvas;
    MenuScript menu;
    CanvasGroup canvasGroup;
    Button lBut, rBut;
    /// <summary>
    /// current scene index
    /// </summary>
    int cIdx;
    // Start is called before the first frame update
    void Start()
    {
        lBut = transform.GetChild(0).GetComponent<Button>();
        rBut = transform.GetChild(1).GetComponent<Button>();
        cIdx = SceneManager.GetActiveScene().buildIndex;
        if (menuCanvas != null)
            menu = menuCanvas.GetComponent<MenuScript>();
        canvasGroup = GameObject.Find("Canvas").GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.anyKeyDown || canvasGroup != null && !canvasGroup.interactable)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            OnClickHandler((int)Scenes.Exit);

        else if (Input.GetKeyDown(KeyCode.F5))
            OnClickHandler((int)Scenes.S1Learn);

        else if (Input.GetKeyDown(KeyCode.F6))
            OnClickHandler((int)Scenes.S2Control);

        else if (Input.GetKeyDown(KeyCode.F7))
            OnClickHandler((int)Scenes.S3Results);

        else if (Input.GetKeyDown(KeyCode.F2))
            OnClickHandler((int)Scenes.S4Settings);

        else if (Input.GetKeyDown(KeyCode.F1))
            OnClickHandler((int)Scenes.S5Help);

        else if (Input.GetKeyDown(KeyCode.F3) && lBut.IsActive() && lBut.interactable)
            lBut.onClick.Invoke();

        else if (Input.GetKeyDown(KeyCode.F4) && rBut.IsActive() && rBut.interactable)
            rBut.onClick.Invoke();
    }

    public void OnClickHandler(int index)
    {
        if (index >= 0)
        {
            if (index != cIdx)
                SceneManager.LoadScene(index);
        }
        else if (index == -1)
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        else if (index == -2)
        {
            if (menu != null)
                menu.ShowMenu();
        }
    }

    void OnDestroy() => data.SavePrefs();
}
