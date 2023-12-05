using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsMenuScript : MenuScript
{
    // Start is called before the first frame update
    protected new void Start()
    {
        InitMenu(new string[] { "Удалить первый результат", "Удалить все результаты" }, MenuHandler);
        base.Start();
    }

    void MenuHandler(int n)
    {
        if (n == 0)
            Debug.Log("Удален первый результат");
        else if (n == 1)
            Debug.Log("Удалены все результаты");
    }
}
