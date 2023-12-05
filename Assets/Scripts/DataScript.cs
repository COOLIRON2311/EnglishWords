using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    [SerializeField] List<TestInfo> results = new();
    List<int> remainInd = new();
    TestInfo test;
    int predInd;
    int ppredInd;
    System.Random r = new();
    List<int> testInd = new();
    public bool OptAudioEnRu;
    public int OptTopicName;
    public int OptVolume = 10;

    public Button mainButton;

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

    public int OptMainButtonFontSize
    {
        get => mainButton.GetComponentInChildren<Text>().fontSize;
        set => mainButton.GetComponentInChildren<Text>().fontSize = value;
    }

    public int OptMainButtonHeight
    {
        get => GetHeight(mainButton);
        set => SetHeight(mainButton, value);
    }

    public int ResultCount { get => results.Count; }

    public string Topic(int i)
    {
        string s = topics[i].Remove(0, 2);
        if (OptTopicName == 0 || level == 3)
            return s;
        var m = Regex.Match(s, @"(\d\d\.)(.*) \((.*)\)");
        return m.Groups[1].Value + m.Groups[OptTopicName + 1].Value;
    }

    public string Word(int i) => $"{words[i].En} \u2013 {words[i].Ru}";

    private void Awake()
    {
        data = new(Resources.LoadAll<TextAsset>("Data").Select(e => e.name));
        LoadPrefabs();
        LoadPrefs();
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
        audio.volume = OptVolume / 10.0f;
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

    public void InitTest()
    {
        words.Clear();
        foreach (int idx in TestTopics)
            GetWords(idx, false);
        remainInd.Clear();
        remainInd.AddRange(Enumerable.Range(0, words.Count));
        remainInd.AddRange(Enumerable.Range(0, words.Count));
        test = new(this);
        predInd = -1;
        ppredInd = -1;
    }

    string GetTitle() => $"Вопрос {test.Answers + 1} из {test.Questions}\nРейтинг: {test.Rating * 100:f2}";

    float GetProgress() => (float)test.Answers / test.Questions;

    public bool NextQuestion(string[] labels, out string title, out float progress)
    {
        // 💀-----------------------------------💀
        //  | Оставь надежду, всяк сюда входящий |
        // 💀-----------------------------------💀
        if (test.Answers == test.Questions)
        {
            title = $"Итоговый рейтинг: {test.Rating * 100:f2}\nОценка: {test.Mark}";
            progress = 1;
            return false;
        }
        // (1) определяются значения выходных параметров title и progress
        title = GetTitle();
        progress = GetProgress();
        // (2)  из списка индексов remainInd выбирается очередное слово, которое будет проверяться в создаваемом вопросе;
        // слово выбирается таким образом, чтобы оно не совпадало со словами, которые проверялись в двух предыдущих вопросах
        // (если такой выбор возможен);
        int q = -1;
        for (int i = 0; i < 10; i++)
        {
            q = r.Next(remainInd.Count);
            if (remainInd[q] != predInd && remainInd[q] != ppredInd)
                break;
        }
        int qInd = remainInd[q];
        // (3)  выбранное для текущего вопроса слово удаляется из списка remainInd;
        // кроме того, корректируются переменные ppredInd и predInd;
        remainInd.RemoveAt(q);
        ppredInd = predInd;
        predInd = qInd;
        // (4) формируется список индексов testInd, содержащий индекс слова для проверки и
        // индексы пяти вариантов слов для ответов;
        testInd.Clear();
        testInd.Add(qInd);
        for (int i = 0; i < 4; i++)
        {
            int idx = r.Next(words.Count);
            while (testInd.Contains(idx))
                idx = r.Next(words.Count);
            testInd.Add(idx);
        }
        testInd.Insert(r.Next(1, 6), qInd);
        // (5) по информации о виде теста и по индексам списка testInd генерируются надписи
        // label, предназначенные для отображения на сцене; в случае теста третьего вида
        // дополнительно воспроизводится аудиофайл, соответствующий проверяемому слову
        if (TestType == 1)
        {
            labels[0] = words[testInd[0]].Ru;
            for (int i = 1; i < 6; i++)
                labels[i] = words[testInd[i]].En;
        }
        else
        {
            labels[0] = TestType == 0 ? words[testInd[0]].En : "[Audio]";
            if (TestType == 2 || OptAudioEnRu)
                PlayAudio(testInd[0]);
            for (int i = 1; i < 6; i++)
                labels[i] = words[testInd[i]].Ru;
        }
        return true;
    }

    public string CheckAnswer(int ansInd, ref string title, ref float progress)
    {
        if (testInd[ansInd] == testInd[0])
        {
            test.Answers++;
            return "";
        }
        test.Mistakes++;
        test.Questions += 2;
        remainInd.Add(testInd[ansInd]);
        remainInd.Add(testInd[0]);
        title = GetTitle();
        progress = GetProgress();
        if (TestType == 2)
            PlayAudio(testInd[ansInd]);
        if (TestType == 1)
            return $"{words[testInd[ansInd]].En} \u2013 {words[testInd[ansInd]].Ru}";
        return $"{words[testInd[ansInd]].Ru} \u2013 {words[testInd[ansInd]].En}";
    }

    public void AdditionalTestAction()
    {
        if (TestType != 1)
            PlayAudio(testInd[0]);
    }

    public void SaveResult()
    {
        if (test.Mark > 2)
            results.Add(test);
    }

    void LoadPrefabs()
    {
        mainButton = Resources.Load<GameObject>("Prefabs/MainButton").GetComponent<Button>();
    }

    public int GetHeight(Component comp) => (int)comp.GetComponent<RectTransform>().sizeDelta.y;

    public void SetHeight(Component comp, int value)
    {
        var rt = comp.GetComponent<RectTransform>();
        var sd = rt.sizeDelta;
        sd.y = value;
        rt.sizeDelta = sd;
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("TestType", TestType);
        for (int i = 0; i < 8; i++)
        {
            PlayerPrefs.SetInt($"ItemIndex{i}", itemIndex[i]);
            PlayerPrefs.SetFloat($"ScrollbarValue{i}", scrollbarValue[i]);
        }
        for (int i = 0; i < 4; i++)
            PlayerPrefs.SetString($"TestTopics{i}", testTopics[i]);
        PlayerPrefs.SetInt("OptAudioEnRu", OptAudioEnRu ? 1 : 0);
        PlayerPrefs.SetInt("OptTopicName", OptTopicName);
        PlayerPrefs.SetInt("OptVolume", OptVolume);
        PlayerPrefs.SetInt("OptMainButtonFontSize", OptMainButtonFontSize);
        PlayerPrefs.SetInt("OptMainButtonHeight", OptMainButtonHeight);

        PlayerPrefs.SetInt("ResultsCount", results.Count);
        for (int i = 0; i < results.Count; i++)
            PlayerPrefs.SetString($"Results{i}", results[i].ToString());
    }

    void LoadPrefs()
    {
        level = PlayerPrefs.GetInt("Level", 0);
        TestType = PlayerPrefs.GetInt("TestType", 0);
        for (int i = 0; i < 8; i++)
        {
            itemIndex[i] = PlayerPrefs.GetInt($"ItemIndex{i}", 0);
            scrollbarValue[i] = PlayerPrefs.GetFloat($"ScrollbarValue{i}", 0);
        }
        for (int i = 0; i < 4; i++)
            testTopics[i] = PlayerPrefs.GetString($"TestTopics{i}", "1");

        OptAudioEnRu = PlayerPrefs.GetInt("OptAudioEnRu", 0) == 1;
        OptTopicName = PlayerPrefs.GetInt("OptTopicName", 0);
        OptVolume = PlayerPrefs.GetInt("OptVolume", 10);
        OptMainButtonFontSize = PlayerPrefs.GetInt("OptMainButtonFontSize", 10);
        OptMainButtonHeight = PlayerPrefs.GetInt("OptMainButtonHeight", 25);

        results.Clear();
        var cnt = PlayerPrefs.GetInt("ResultsCount", 0);
        for (int i = 0; i < cnt; i++)
            results.Add(new TestInfo(PlayerPrefs.GetString($"Results{i}", "")));
    }

    public string TestTypeToString(int testType)
    {
        return testType switch
        {
            0 => "En-Ru",
            1 => "Ru-En",
            _ => "Au-Ru",
        };
    }

    public string Result(int i)
    {
        var r = results[i];
        return $"{r.StartTime} {r.Level + 1}:{r.Topics} "
            + $"{TestTypeToString(r.Type)} {r.Rating * 100:f0}\u00A0[{r.Mark}]";
    }
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

[System.Serializable]
public class TestInfo
{
    public int Answers;
    public int Mistakes;
    public int Questions;
    public int Type;
    public int Level;
    public string Topics;
    public int WordCount;
    public string StartTime;
    public TestInfo(DataScript data)
    {
        Answers = 0;
        Mistakes = 0;
        Questions = 2 * data.WordCount;
        Type = data.TestType;
        Level = data.Level;
        Topics = data.TestTopicsToString();
        WordCount = data.WordCount;
        StartTime = System.DateTime.Now.ToString("yy.MM.dd HH:mm:ss");
    }

    public TestInfo(string info)
    {
        var s = info.Split('|');
        if (s.Length != 8)
            return;
        Answers = int.Parse(s[0]);
        Mistakes = int.Parse(s[1]);
        Questions = int.Parse(s[2]);
        Type = int.Parse(s[3]);
        Level = int.Parse(s[4]);
        Topics = s[5];
        WordCount = int.Parse(s[6]);
        StartTime = s[7];
    }

    public override string ToString() => $"{Answers}|{Mistakes}|{Questions}|{Type}|{Level}|{Topics}|{WordCount}|{StartTime}";

    public float Rating
    {
        get => (Answers > Mistakes) ? Mathf.Pow((float)(Answers - Mistakes) / Questions, 2) : 0;
    }

    public int Mark
    {
        get
        {
            float r = Rating;
            if (r < 0.6f)
                return 2;
            if (r < 0.7f)
                return 3;
            if (r < 0.85f)
                return 4;
            return 5;
        }
    }
}

