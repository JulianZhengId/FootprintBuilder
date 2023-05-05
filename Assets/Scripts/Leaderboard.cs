using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private List<Transform> entries;
    private List<Entry> myEntries;
    [SerializeField] private TextMeshProUGUI leaderboardTitle;
    [SerializeField] private Image screenshot;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private float oscillationSpeed = 10f;

    void Awake()
    {
        warningText.color = new Color(0, 0, 0, 0);
        if (GameData.gameData.DifficultyGame == GameData.Difficulty.easy)
        {
            leaderboardTitle.text = "Leaderboard (Low)";
        }
        else if (GameData.gameData.DifficultyGame == GameData.Difficulty.medium)
        {
            leaderboardTitle.text = "Leaderboard (Middle)";
        }
        else
        {
            leaderboardTitle.text = "Leaderboard (High)";
        }
    }

    void Start()
    {
        ShowScreenshot();
        StartCoroutine(ShowWarningText(1f));
        AddEntry(GameData.gameData.Result.indexScore, GameData.gameData.Result.moneyUsage, GameData.gameData.Result.co2Consumption);
        string key;
        if (GameData.gameData.DifficultyGame == GameData.Difficulty.easy)
        {
            key = "LeaderboardLow";
        }
        else if (GameData.gameData.DifficultyGame == GameData.Difficulty.medium)
        {
            key = "LeaderboardMiddle";
        }
        else
        {
            key = "LeaderboardHigh";
        }
        string jsonString = PlayerPrefs.GetString(key);
        Entries e = JsonUtility.FromJson<Entries>(jsonString);
        myEntries = e.listOfEntries;

        //Sorting
        for (int k = 0; k < myEntries.Count; k++)
        {
            for (int l = k + 1; l < myEntries.Count; l++)
            {
                if (myEntries[l].indexScore > myEntries[k].indexScore)
                {
                    Entry tmp = myEntries[k];
                    myEntries[k] = myEntries[l];
                    myEntries[l] = tmp;
                }
            }
        }

        for (int j = myEntries.Count; j < 8; j++)
        {
            entries[j].gameObject.SetActive(false);
        }

        int i = 0;
        foreach (Entry entry in myEntries)
        {
            if (i >= 8) return;
            CreateEntryTransform(entry, i);
            i++;
        }
    }

    private void AddEntry(float score, float money, float co2)
    {
        int money_int = Mathf.RoundToInt(money / 1000);
        int co2_int = Mathf.RoundToInt(co2);
        Entry newEntry = new Entry { indexScore = score, money = money_int, co2 = co2_int };

        //Add To List
        string key;
        if (GameData.gameData.DifficultyGame == GameData.Difficulty.easy)
        {
            key = "LeaderboardLow";
        }
        else if (GameData.gameData.DifficultyGame == GameData.Difficulty.medium)
        {
            key = "LeaderboardMiddle";
        }
        else
        {
            key = "LeaderboardHigh";
        }
        string jsonString = PlayerPrefs.GetString(key);
        Entries e = JsonUtility.FromJson<Entries>(jsonString);
        if (e == null)
        {
            e = new Entries { listOfEntries = new List<Entry>() };
        }
        if (e.listOfEntries == null)
        {
            e.listOfEntries = new List<Entry>();
        }
        e.listOfEntries.Add(newEntry);

        //Delete
        int listCount = e.listOfEntries.Count;
        if (listCount >= 20)
        {
            e.listOfEntries.RemoveRange(10, listCount - 10);
        }

        //Save
        string json = JsonUtility.ToJson(e);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();

    }

    private void CreateEntryTransform(Entry newEntry, int i)
    {
        Transform entry = entries[i];
        entry.gameObject.SetActive(true);
        //Rank
        int rank = i + 1;
        string rankString;
        switch (rank)
        {
            case 1: rankString = rank + "st"; break;
            case 2: rankString = rank + "nd"; break;
            case 3: rankString = rank + "rd"; break;
            default: rankString = rank + "th"; break;
        }
        entry.GetChild(0).GetComponent<TextMeshProUGUI>().text = rankString;

        //Score
        float indexScore = newEntry.indexScore;
        entry.GetChild(1).GetComponent<TextMeshProUGUI>().text = indexScore.ToString();

        //Money
        int money = newEntry.money;
        entry.GetChild(2).GetComponent<TextMeshProUGUI>().text = money + "K";

        //CO2
        int co2 = newEntry.co2;
        entry.GetChild(3).GetComponent<TextMeshProUGUI>().text = string.Format("{0:#,###0}", co2);
    }

    private void ShowScreenshot()
    {
        //Screenshot
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(GameData.gameData.ScreenshotBytes);
        var image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        screenshot.sprite = image;
    }

    private IEnumerator ShowWarningText(float t)
    {
        yield return new WaitForSeconds(t);
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            float oscillate = 0.5f * (Oscillate(time, oscillationSpeed) + 1);
            warningText.color = new Color(0, 0, 0, oscillate);
            yield return null;
        }
    }

    float Oscillate(float time, float speed)
    {
        return -Mathf.Cos(time * speed / Mathf.PI);
    }

    private class Entries
    {
        public List<Entry> listOfEntries;
    }

    [System.Serializable]
    private class Entry
    {
        public float indexScore;
        public int money;
        public int co2;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
