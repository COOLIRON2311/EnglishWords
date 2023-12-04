using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Learn2Script : MonoBehaviour
{
    public DataScript data;
    // Start is called before the first frame update
    void Start()
    {
        data.GetWords(data.S1ItemIndex);
        var hText = GameObject.Find("HText").GetComponent<Text>();
        hText.text = data.Topic(data.S1ItemIndex);

        for (int i = 0; i < data.WordCount; i++)
        {
            var b = Instantiate(data.mainButton);
            b.GetComponentInChildren<Text>().text = data.Word(i);
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
