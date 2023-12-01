using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtonScript : MonoBehaviour
{
    public DataScript data;
    public void OnClickHandler()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case (int)Scenes.S1Learn:
                data.CurrentTopicIndex = transform.GetSiblingIndex();
                SceneManager.LoadScene((int)Scenes.S6Learn2);
                return;

            case (int)Scenes.S6Learn2:
                data.PlayAudio(transform.GetSiblingIndex());
                return;
        }
    }
}
