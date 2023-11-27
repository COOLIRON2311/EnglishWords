using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class DataScript : ScriptableObject
{
    [SerializeField] int level;
    [SerializeField] List<string> data;
    [SerializeField] List<string> topics;
    public int Level { get => level; }
    public int TopicCount { get => topics.Count; }
    public string Topic(int i) => topics[i].Remove(0, 2);

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
}
