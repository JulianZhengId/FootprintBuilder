using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.XR.ARFoundation;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject livingStandard;
    [SerializeField] private GameObject iconAnimation;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private TextMeshProUGUI warningText;
    private ToggleGroup difficultyToggle;

    private void Awake()
    {
        warningText.text = "";
        StartCoroutine(activateInputField());
        difficultyToggle = livingStandard.GetComponent<ToggleGroup>();
    }

    public void StartGame()
    {
        warningText.text = "Loading...";
        Toggle toggle = difficultyToggle.ActiveToggles().FirstOrDefault();
        string difficulty = toggle.GetComponentInChildren<TextMeshProUGUI>().text;

        if (difficulty == "high")
        {
            GameData.gameData.DifficultyGame = GameData.Difficulty.hard;
        }
        else if (difficulty == "middle")
        {
            GameData.gameData.DifficultyGame = GameData.Difficulty.medium;
        }
        else
        {
            GameData.gameData.DifficultyGame = GameData.Difficulty.easy;
        }
        GameData.gameData.MoneyLimit = 1000000;
        GameData.gameData.CO2Limit = 13500;
        LoaderUtility.Initialize();
        SceneManager.LoadSceneAsync(1);
        return;
    }

    IEnumerator activateInputField()
    {
        yield return new WaitForSeconds(1.3f);
        livingStandard.SetActive(true);
        iconAnimation.SetActive(true);
        startGameButton.SetActive(true);
        float t = 0;
        float duration = 1.5f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, t / duration);
            startGameButton.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
}