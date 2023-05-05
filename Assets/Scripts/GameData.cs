using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public enum EditModes
    {
        IDLE,
        MODIFY_HEIGHT
    }

    public enum Difficulty
    {
        easy,
        medium,
        hard
    }

    public struct BuildingData
    {
        public float cost;
        public float co2;

        public float height;
        public float length;
        public float width;
        public float volume;
        public float squaremeter;
        public int floors;

        public bool solar;
        public bool roof;
        public BaseAttributes.ObjectMaterial objectMaterial;

        public BuildingData(GameObject obj)
        {
            height = 0;
            length = 0;
            width = 0;
            volume = 0;
            squaremeter = 0;
            cost = 0;
            co2 = 0;
            floors = 0;
            solar = false;
            roof = false;
            objectMaterial = BaseAttributes.ObjectMaterial.Blank;
        }

        public BuildingData(Transform parent, Transform child)
        {
            var baseAttributes = child.GetComponent<BaseAttributes>();
            height = Mathf.Round(parent.transform.localScale.y * 100) / 100;
            length = baseAttributes.GetLength();
            width = Mathf.Round(baseAttributes.GetWidth() * 100) / 100;
            float tmp = length * width * height;
            volume = Mathf.Round(tmp * 100) / 100;
            squaremeter = Mathf.Round(tmp / 2.7f * 100) / 100;
            cost = Mathf.Round(baseAttributes.GetCost() * 100) / 100;
            co2 = Mathf.Round(baseAttributes.GetCO2() * 100) / 100;
            floors = Mathf.FloorToInt(parent.localScale.y / 2.7f);
            solar = baseAttributes.GetSolar();
            roof = baseAttributes.GetRoofActive();
            objectMaterial = baseAttributes.GetObjectMaterial();
        }

        public override string ToString()
        {
            string str = "Cost : " + cost + "  $" + '\n';
            str += "CO2 : " + co2 + ((co2 > 0) ? "  kgs/y" : "  kg/y") + "\n\n";

            str += "Length : " + length + "  m" + '\n';
            str += "Width : " + width + "  m" + '\n';
            str += "Height : " + height + "  m" + '\n';
            str += "Volume : " + volume + "  m3" + "\n";
            str += "Squaremeter : " + squaremeter + "  m2" + '\n';
            str += "Floors : " + floors + "\n\n";

            str += "Solar : " + (solar ? "Yes" : "No") + '\n';
            str += "Roof : " + (roof ? "Yes" : "No") + '\n';
            str += "Material : " + objectMaterial;
            return str;
        }
    }

    public struct GameResult
    {
        public float moneyUsage;
        public float co2Consumption;
        public float indexScore;

        public GameResult(float moneyLeft, float co2Left)
        {
            moneyUsage = Mathf.RoundToInt(1000000 - moneyLeft);
            co2Consumption = Mathf.RoundToInt(co2Left);
            if (co2Consumption == 0 || moneyUsage == 0) indexScore = 0;
            else indexScore = Mathf.Round(moneyUsage / co2Consumption * 100) / 100;
        }
    }

    public static GameData gameData;
    private float moneyLimit = 0;
    private float co2Limit = 0;
    private Difficulty difficultyGame;
    private GameObject selectedObject;
    private EditModes editMode = EditModes.IDLE;
    private BuildingData dataA;
    private BuildingData dataB;
    private GameResult result;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private byte[] screenshotBytes;

    public EditModes EditMode
    {
        get { return editMode; }
        set { editMode = value; }
    }

    public float MoneyLimit
    {
        get { return moneyLimit; }
        set { moneyLimit = value; }
    }

    public float CO2Limit
    {
        get { return co2Limit; }
        set { co2Limit = value; }
    }

    public Difficulty DifficultyGame
    {
        get { return difficultyGame; }
        set { difficultyGame = value; }
    }

    public GameObject SelectedObject
    {
        get { return selectedObject; }
        set { selectedObject = value; }
    }

    public BuildingData DataA
    {
        get { return dataA; }
        set { dataA = value; }
    }

    public BuildingData DataB
    {
        get { return dataB; }
        set { dataB = value; }
    }

    public GameResult Result
    {
        get { return result; }
        set { result = value; }
    }

    public List<GameObject> SpawnedObjects
    {
        get { return spawnedObjects; }
        set { spawnedObjects = value; }
    }

    public byte[] ScreenshotBytes
    {
        get { return screenshotBytes; }
        set { screenshotBytes = value; }
    }

    private void Awake()
    {
        Singleton();
    }

    void Singleton()
    {
        if (GameData.gameData == null)
        {
            GameData.gameData = this;
        }
        else
        {
            if (GameData.gameData != this)
            {
                Destroy(GameData.gameData.gameObject);
                GameData.gameData = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
