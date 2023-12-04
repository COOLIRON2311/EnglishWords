using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public DataScript data;
    // Start is called before the first frame update
    void Start()
    {
        var toggle = GameObject.Find("1Toggle").GetComponent<Toggle>();
        toggle.isOn = data.OptAudioEnRu;
        toggle.onValueChanged.AddListener(b => data.OptAudioEnRu = b);

        var dropdown = GameObject.Find("2Dropdown").GetComponent<Dropdown>();
        dropdown.value = data.OptTopicName;
        dropdown.onValueChanged.AddListener(v => data.OptTopicName = v);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
