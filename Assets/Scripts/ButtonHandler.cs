using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using UnityEngine.XR.ARFoundation;

public class ButtonHandler : MonoBehaviour
{
    //Textures
    [SerializeField] private Textures textures;

    //Panel
    [SerializeField] private GameObject comparisonPanel;
    [SerializeField] private TextMeshProUGUI textDataA;
    [SerializeField] private TextMeshProUGUI textDataB;

    //Buttons
    [SerializeField] private Button materialButton;
    [SerializeField] private Button componentButton;
    [SerializeField] private Button statisticsButton;
    [SerializeField] private GameObject materialChildButtons;
    [SerializeField] private GameObject componentChildButtons;
    [SerializeField] private GameObject statisticsChildButtons;

    private bool childButtonsOpened;
    private Vector3 topPosition;
    private Vector3 componentButtonInitialPos;
    private Vector3 statisticsButtonInitialPos;

    //BarProgress
    [SerializeField] private BarProgress moneyBarProgress;
    [SerializeField] private BarProgress co2BarProgress;

    private void Awake()
    {
        topPosition = materialButton.transform.position;
        componentButtonInitialPos = componentButton.transform.position;
        statisticsButtonInitialPos = statisticsButton.transform.position;
    }

    public void AssignWoodTexture()
    {
        if (GameData.gameData.SelectedObject == null) return;
        var selectedObject = GameData.gameData.SelectedObject;
        var baseAttributes = selectedObject.GetComponent<BaseAttributes>();
        var prevObjectMaterial = baseAttributes.GetObjectMaterial();
        if (prevObjectMaterial != BaseAttributes.ObjectMaterial.Wood)
        {
            RecalculateMoneyAndCO2(selectedObject, 2.7f, 0.9f, BaseAttributes.ObjectMaterial.Wood, textures.woodTexture, textures.basicWoodTexture);
        }
    }

    public void AssignSteelTexture()
    {
        if (GameData.gameData.SelectedObject == null) return;
        var selectedObject = GameData.gameData.SelectedObject;
        var baseAttributes = selectedObject.GetComponent<BaseAttributes>();
        var prevObjectMaterial = baseAttributes.GetObjectMaterial();
        if (prevObjectMaterial != BaseAttributes.ObjectMaterial.Steel)
        {
            RecalculateMoneyAndCO2(selectedObject, 10.3f, 1.2f, BaseAttributes.ObjectMaterial.Steel, textures.steelTexture, textures.basicSteelTexture);
        }
    }

    public void AssignBrickTexture()
    {
        if (GameData.gameData.SelectedObject == null) return;
        var selectedObject = GameData.gameData.SelectedObject;
        var baseAttributes = selectedObject.GetComponent<BaseAttributes>();
        var prevObjectMaterial = baseAttributes.GetObjectMaterial();
        if (prevObjectMaterial != BaseAttributes.ObjectMaterial.Brick)
        {
            RecalculateMoneyAndCO2(selectedObject, 5.7f, 1.1f, BaseAttributes.ObjectMaterial.Brick, textures.brickTexture, textures.basicBrickTexture);
        }
    }

    public void AssignConcreteTexture()
    {
        if (GameData.gameData.SelectedObject == null) return;
        var selectedObject = GameData.gameData.SelectedObject;
        var baseAttributes = selectedObject.GetComponent<BaseAttributes>();
        var prevObjectMaterial = baseAttributes.GetObjectMaterial();
        if (prevObjectMaterial != BaseAttributes.ObjectMaterial.Concrete)
        {
            RecalculateMoneyAndCO2(selectedObject, 8.1f, 1f, BaseAttributes.ObjectMaterial.Concrete, textures.concreteTexture, textures.basicConcreteTexture);
        }
    }

    public void MaterialButton()
    {
        if (!childButtonsOpened)
        {
            childButtonsOpened = true;
            materialChildButtons.gameObject.SetActive(true);
            componentButton.gameObject.SetActive(false);
            statisticsButton.gameObject.SetActive(false);
        }
        else
        {
            childButtonsOpened = false;
            materialChildButtons.gameObject.SetActive(false);
            componentButton.gameObject.SetActive(true);
            componentButton.transform.position = componentButtonInitialPos;
            statisticsButton.gameObject.SetActive(true);
            statisticsButton.transform.position = statisticsButtonInitialPos;
        }
    }

    public void ComponentButton()
    {
        if (!childButtonsOpened)
        {
            childButtonsOpened = true;
            materialButton.gameObject.SetActive(false);
            componentButton.gameObject.SetActive(true);
            componentButton.transform.position = topPosition;
            statisticsButton.gameObject.SetActive(false);
            componentChildButtons.gameObject.SetActive(true);
        }
        else
        {
            childButtonsOpened = false;
            componentButton.gameObject.SetActive(true);
            componentButton.transform.position = componentButtonInitialPos;
            statisticsButton.gameObject.SetActive(true);
            statisticsButton.transform.position = statisticsButtonInitialPos;
            componentChildButtons.gameObject.SetActive(false);
            materialButton.gameObject.SetActive(true);
        }
    }

    public void StatisticsButton()
    {
        if (!childButtonsOpened)
        {
            childButtonsOpened = true;
            materialButton.gameObject.SetActive(false);
            componentButton.gameObject.SetActive(false);
            statisticsButton.transform.position = topPosition;
            statisticsButton.gameObject.SetActive(true);
            statisticsChildButtons.gameObject.SetActive(true);
        }
        else
        {
            childButtonsOpened = false;
            componentButton.gameObject.SetActive(true);
            componentButton.transform.position = componentButtonInitialPos;
            statisticsButton.gameObject.SetActive(true);
            statisticsButton.transform.position = statisticsButtonInitialPos;
            statisticsChildButtons.gameObject.SetActive(false);
            materialButton.gameObject.SetActive(true);
        }
    }

    public void SaveAsDataA()
    {
        if (GameData.gameData.SelectedObject == null) return;
        var selectedTransform = GameData.gameData.SelectedObject.transform;
        GameData.gameData.DataA = new GameData.BuildingData(selectedTransform.parent, selectedTransform);
    }

    public void SaveAsDataB()
    {
        if (GameData.gameData.SelectedObject == null) return;
        var selectedTransform = GameData.gameData.SelectedObject.transform;
        GameData.gameData.DataB = new GameData.BuildingData(selectedTransform.parent, selectedTransform);
    }

    public void DataComparison()
    {
        comparisonPanel.gameObject.SetActive(true);
        textDataA.text = GameData.gameData.DataA.ToString();
        textDataB.text = GameData.gameData.DataB.ToString();
    }

    public void Back()
    {
        comparisonPanel.gameObject.SetActive(false);
    }

    public void Solar()
    {
        if (GameData.gameData.SelectedObject == null) return;
        var baseAttributes = GameData.gameData.SelectedObject.GetComponent<BaseAttributes>();
        bool hasSolar = baseAttributes.GetSolar();
        GameObject solarPanels = baseAttributes.GetSolarPanels();

        if (!hasSolar)
        {
            GameObject roof = baseAttributes.GetRoof();
            roof.SetActive(true);
            baseAttributes.SetRoofActive(true);
        }
        baseAttributes.SetSolar(!hasSolar);
        solarPanels.SetActive(!hasSolar);
        RecalculateSolar(GameData.gameData.SelectedObject, !hasSolar);
    }

    public void Roof()
    {
        if (GameData.gameData.SelectedObject == null) return;
        var baseAttributes = GameData.gameData.SelectedObject.GetComponent<BaseAttributes>();

        if (baseAttributes.GetRoofActive())
        {
            baseAttributes.GetRoof().SetActive(false);
            bool hasSolar = baseAttributes.GetSolar();
            if (hasSolar)
            {
                GameObject solarPanels = baseAttributes.GetSolarPanels();
                baseAttributes.SetSolar(false);
                RecalculateSolar(GameData.gameData.SelectedObject, false);
                solarPanels.SetActive(false);
            }
        }
        else
        {
            baseAttributes.GetRoof().SetActive(true);
        }

        //Set Bool
        baseAttributes.SetRoofActive(!baseAttributes.GetRoofActive());
    }

    public void Door()
    {
        if (GameData.gameData.SelectedObject == null) return;
        if (GameData.gameData.SelectedObject.transform.parent.localScale.y < 3f) return;
        var baseAttributes = GameData.gameData.SelectedObject.GetComponent<BaseAttributes>();

        //Spawn Greens
        if (baseAttributes.GetDoorActive())
        {
            baseAttributes.GetDoor().SetActive(false);
        }
        else
        {
            baseAttributes.GetDoor().SetActive(true);
        }

        //Set Bool
        baseAttributes.SetDoorActive(!baseAttributes.GetDoorActive());
    }

    public void End()
    {
        foreach (GameObject obj in GameData.gameData.SpawnedObjects)
        {
            if (!obj.activeSelf) return;
        }

        if (GameData.gameData.SelectedObject != null)
        {
            GameData.gameData.SelectedObject.GetComponent<ClickHandler>().UndoOutline();
        }

        comparisonPanel.SetActive(false);
        StartCoroutine(TakeScreenshot());
        float moneyLeft = moneyBarProgress.GetAmount();
        float co2Left = co2BarProgress.GetAmount();
        GameData.gameData.Result = new GameData.GameResult(moneyLeft, co2Left);
        StartCoroutine(HoldSecondsAndLoad(0.5f));
    }

    private void RecalculateMoneyAndCO2(GameObject selectedObject, float emission, float costScale, BaseAttributes.ObjectMaterial objectMaterial, Texture texture, Texture basicTexture)
    {
        var baseAttributes = selectedObject.GetComponent<BaseAttributes>();
        var parent = selectedObject.transform.parent;
        var height = parent.transform.localScale.y;

        var prevCO2 = baseAttributes.GetCO2();
        var prevCost = baseAttributes.GetCost();

        var squaremeter = height * baseAttributes.GetLength() * baseAttributes.GetWidth() / 3;
        var hasSolar = baseAttributes.GetSolar();

        float amountCO2 = 0f;
        //CO2
        if (hasSolar)
        {
            amountCO2 += 8.5f * squaremeter;
        }
        else
        {
            amountCO2 += 7.5f * squaremeter;
        }
        amountCO2 += emission * squaremeter;
        var diffCO2 = amountCO2 - prevCO2;

        //Cost
        float amountCost = 0f;
        var difficulty = GameData.gameData.DifficultyGame;
        if (difficulty == GameData.Difficulty.easy)
        {
            amountCost += squaremeter * costScale * 3000;
        }
        else if (difficulty == GameData.Difficulty.medium)
        {
            amountCost += squaremeter * costScale * 3500;
        }
        else
        {
            amountCost += squaremeter * costScale * 4000;
        }
        var diffCost = amountCost - prevCost;

        if (co2BarProgress.GetAmount() >= diffCO2 && moneyBarProgress.GetAmount() >= diffCost)
        {
            //Calculation
            //CO2
            co2BarProgress.GainAmount(diffCO2);
            baseAttributes.SetCO2(amountCO2);
            //Money
            moneyBarProgress.LoseAmount(diffCost);
            baseAttributes.SetCost(amountCost);

            //Assign Texture
            AssignTexture(selectedObject, objectMaterial, texture, basicTexture);
        }
    }

    private void AssignTexture(GameObject selectedObject, BaseAttributes.ObjectMaterial objectMaterial, Texture texture, Texture basicTexture)
    {
        selectedObject.GetComponent<BaseAttributes>().SetObjectMaterial(objectMaterial);
        selectedObject.GetComponent<Renderer>().material.mainTexture = texture;
        selectedObject.GetComponent<BaseAttributes>().GetTop().GetComponent<Renderer>().material.mainTexture = basicTexture;
    }

    private void RecalculateSolar(GameObject obj, bool currentHasSolar)
    {
        BaseAttributes baseAttributes = obj.GetComponent<BaseAttributes>();
        Transform parent = obj.transform.parent;
        float height = parent.transform.localScale.y;
        float squaremeter = height * baseAttributes.GetLength() * baseAttributes.GetWidth() / 3;

        float diffCO2;
        if (currentHasSolar)
        {
            diffCO2 = -1f * squaremeter;
        }
        else
        {
            diffCO2 = 1f * squaremeter;
        }

        if (co2BarProgress.GetAmount() >= diffCO2)
        {
            co2BarProgress.GainAmount(diffCO2);
            baseAttributes.AddCO2(diffCO2);
        }
    }

    IEnumerator HoldSecondsAndLoad(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadSceneAsync(2);
        LoaderUtility.Deinitialize();
    }

    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        GameData.gameData.ScreenshotBytes = tex.EncodeToPNG();
        Destroy(tex);
    }
}
