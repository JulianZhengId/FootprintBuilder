using System.Collections.Generic;
using UnityEngine;

public class GameReset : MonoBehaviour
{
    private void Awake()
    {
        if (GameData.gameData != null)
        {
            GameData.gameData.SelectedObject = null;
            GameData.gameData.DataA = new GameData.BuildingData(null);
            GameData.gameData.DataB = new GameData.BuildingData(null);
            GameData.gameData.EditMode = GameData.EditModes.IDLE;
            GameData.gameData.Result = new GameData.GameResult(1000000, 225000);
            GameData.gameData.ScreenshotBytes = null;
            GameData.gameData.SpawnedObjects = new List<GameObject>();
        }
    }
}
