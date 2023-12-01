using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class DataScript : ScriptableObject
{
    [SerializeField] int level;
    [SerializeField] List<string> data;
    [SerializeField] List<string> topics;
    [SerializeField] List<WordInfo> words;
    [SerializeField] int[] itemIndex = new int[8];
    [SerializeField] float[] scrollbarValue = new float[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
    public int TestType;
    [SerializeField] string[] testTopics = new string[4] { "1", "1", "1", "1" };


    public int Level { get => level; }
    public int TopicCount { get => topics.Count; }
    public int WordCount { get => words.Count; }

    public int S1ItemIndex
    {
        get => itemIndex[level];
        set => itemIndex[level] = value;
    }

    public int S2ItemIndex
    {
        get => itemIndex[level + 4];
        set => itemIndex[level + 4] = value;
    }

    public float S1ScrollbarValue
    {
        get => scrollbarValue[level];
        set => scrollbarValue[level] = value;
    }

    public float S2ScrollbarValue
    {
        get => scrollbarValue[level + 4];
        set => scrollbarValue[level + 4] = value;
    }

    public string Topic(int i) => topics[i].Remove(0, 2);

    public string Word(int i) => $"{words[i].En} \u2013 {words[i].Ru}";

    private void Awake()
    {
        data = new(Resources.LoadAll<TextAsset>("Data").Select(e => e.name));
        SetLevel(level);
    }

    private void Reset() => Awake();

    public void SetLevel(int newLevel)
    {
        level = newLevel;
        topics = new(data.Where(e => e.StartsWith(newLevel + 1 + ".")));
    }

    public void GetWords(int topicIndex, bool reset = true)
    {
        if (reset)
            words.Clear();
        string text = Resources.Load<TextAsset>("Data/" + topics[topicIndex]).text;
        foreach (var str in text.Split('\n'))
        {
            string[] w = str.Split('#');
            if (w.Length == 3)
                words.Add(new WordInfo(w));
        }
    }
    public void PlayAudio(int wordIndex)
    {
        var audio = Camera.main.GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Sounds/" + words[wordIndex].Au);
        audio.Play();
    }

    public void SetNavigationDown(Selectable from, Selectable to)
    {
        var nav = from.navigation;
        nav.selectOnDown = to;
        from.navigation = nav;
    }

    public SortedSet<int> TestTopics
    {
        get
        {
            if (testTopics[level] == null || testTopics[level] == "")
                return new();
            return new(testTopics[level].Split(',').Select(e => int.Parse(e) - 1));
        }
        set => testTopics[level] = string.Join(",", value.Select(e => e + 1));
    }

    public string TestTopicsToString() => testTopics[level];
}

[System.Serializable]
public struct WordInfo
{
    public string En;
    public string Au;
    public string Ru;
    public WordInfo(string[] w)
    {
        En = w[0];
        Au = w[1];
        Ru = w[2];
    }
}
