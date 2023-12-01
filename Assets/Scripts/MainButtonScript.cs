using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainButtonScript : MonoBehaviour, ISelectHandler
{
    public DataScript data;
    public void OnClickHandler()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case (int)Scenes.S1Learn:
                SceneManager.LoadScene((int)Scenes.S6Learn2);
                return;

            case (int)Scenes.S6Learn2:
                data.PlayAudio(transform.GetSiblingIndex());
                return;
        }
    }
    public void OnSelect(BaseEventData eventData)
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)Scenes.S1Learn)
            data.S1ItemIndex = transform.GetSiblingIndex();
    }
}
