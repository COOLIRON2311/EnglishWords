using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultsScript : MonoBehaviour
{
    public DataScript data;
    // Start is called before the first frame update
    void Start()
    {
        if (data.ResultCount == 0)
            return;

        for (int i = 0; i < data.ResultCount; i++)
        {
            var b = Instantiate(data.mainButton);
            b.GetComponentInChildren<Text>().text = data.Result(i);
            b.transform.SetParent(transform);
            b.transform.localScale = Vector2.one;
        }

        var es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        es.SetSelectedGameObject(transform.GetChild(0).gameObject);

        var child0 = transform.GetChild(0).GetComponent<Button>();
        data.SetNavigationDown(GameObject.Find("HRButton").GetComponent<Button>(), child0);
        data.SetNavigationDown(GameObject.Find("HLButton").GetComponent<Button>(), child0);
    }
}
