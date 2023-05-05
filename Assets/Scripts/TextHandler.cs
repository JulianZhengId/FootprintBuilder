using UnityEngine;
using TMPro;

public class TextHandler : MonoBehaviour
{
    //Building
    [SerializeField] private TextMeshProUGUI buildingHeight;
    //Personal
    [SerializeField] private TextMeshProUGUI difficultyText;

    public void setBuildingHeightText(float f)
    {
        buildingHeight.text = "Height : " + f;
    }

    private void Awake()
    {
        var difficulty = GameData.gameData.DifficultyGame;
        if (difficulty == GameData.Difficulty.easy)
        {
            SetDifficultyText("Low");
        }
        else if (difficulty == GameData.Difficulty.medium)
        {
            SetDifficultyText("Middle");
        }
        else
        {
            SetDifficultyText("High");
        }
    }

    private void SetDifficultyText(string s)
    {
        difficultyText.text = "Living Standard : " + s;
    }
}
