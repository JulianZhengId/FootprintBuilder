using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneySpent;
    [SerializeField] private TextMeshProUGUI co2Consumed;
    [SerializeField] private TextMeshProUGUI indexScore;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private Image earthImage;
    [SerializeField] private List<Sprite> earthImages;

    [SerializeField] private float oscillationSpeed = 10f;
    private bool showWarningText = false;
    private float time = 0;

    private void Awake()
    {
        warningText.faceColor = new Color(0, 0, 0, 0);
    }

    private void Start()
    {
        ShowResult();
    }

    private void ShowResult()
    {
        if (GameData.gameData == null) return;
        //Attributes
        var result = GameData.gameData.Result;
        moneySpent.text = result.moneyUsage + " $";
        co2Consumed.text =result.co2Consumption + " Kilograms / year";
        indexScore.text = result.indexScore.ToString();
        float value = result.co2Consumption / GameData.gameData.CO2Limit * 500;
        value = Mathf.Round(value * 100) / 100;
        percentageText.text = value + " %";
        SetEarthImage(value);
        StartCoroutine(ShowWarningText(1.5f));
    }

    private IEnumerator ShowWarningText(float t)
    {
        yield return new WaitForSeconds(t);
        showWarningText = true;
    }

    private void Update()
    {
        if (!showWarningText) return;
        time += Time.deltaTime;
        float oscillate = 0.5f * (Oscillate(time, oscillationSpeed) + 1);
        warningText.faceColor = new Color(0, 0, 0, oscillate);
    }

    float Oscillate(float time, float speed)
    {
        return -Mathf.Cos(time * speed / Mathf.PI);
    }

    public void LoadNextScene()
    {
        if (!showWarningText) return;
        SceneManager.LoadSceneAsync(3);
    }

    private void SetEarthImage(float value)
    {
        if (value >= 500)
        {
            earthImage.sprite = earthImages[4];
        }
        earthImage.sprite = earthImages[Mathf.FloorToInt(value / 100)];
    }
}
