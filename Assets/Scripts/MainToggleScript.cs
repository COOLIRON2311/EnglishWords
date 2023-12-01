using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainToggleScript : MonoBehaviour, ISelectHandler
{
    public DataScript data;
    public void OnSelect(BaseEventData _)
    {
        data.S2ItemIndex = transform.GetSiblingIndex();
    }
}
