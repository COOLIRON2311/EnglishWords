using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsMenuScript : MenuScript
{
    public Canvas dialogCanvas;
    DialogScript dialog;
    // Start is called before the first frame update
    protected new void Start()
    {
        InitMenu(new string[] { "Удалить первый результат", "Удалить все результаты" }, MenuHandler);
        dialog = dialogCanvas.GetComponent<DialogScript>();
        base.Start();
    }

    void MenuHandler(int n)
    {
        var content = GameObject.Find("Content").transform;
        var emptyResults = false;
        if (n == 0)
        { // Удаление первого результата
            if (content.childCount > 0)
                Destroy(content.GetChild(0).gameObject);
            if (content.childCount > 1)
                es.SetSelectedGameObject(content.GetChild(1).gameObject);
            else
                emptyResults = true;
        }
        else if (n == 1)
        { // Удаление всех результатов
            for (int i = 0; i < content.childCount; i++)
                Destroy(content.GetChild(i).gameObject);
            emptyResults = true;
        }
        if (emptyResults)
        {
            es.SetSelectedGameObject(GameObject.Find("HLButton"));
            DisableMenuItem(0);
            DisableMenuItem(1);
        }
        dialog.ShowDialog("Информация", "Команда успешно выполнена");
    }
}
