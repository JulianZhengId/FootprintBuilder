using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System;

public class DragHandler : MonoBehaviour
{
    public IObservable<Vector2> dragTrigger;
    public IObservable<EventArgs> endDragTrigger;

    [SerializeField] private float scalingFactor = 10f;
    [SerializeField] private float minScale = 0.05f;
    [SerializeField] private float maxScale = 15f;

    [SerializeField] private TextHandler textHandler;
    [SerializeField] private BarProgress moneyBarProgress;
    [SerializeField] private BarProgress co2BarProgress;

    private void Awake()
    {
        dragTrigger = gameObject.AddComponent<ObservableDragTrigger>()
            .OnDragAsObservable()
            .Select(e => e.delta);

        endDragTrigger = gameObject.AddComponent<ObservableEndDragTrigger>()
            .OnEndDragAsObservable()
            .Select(e => EventArgs.Empty);
    }

    private void Start()
    {
        dragTrigger.Subscribe(Drag).AddTo(this);
        endDragTrigger.Subscribe(EndDrag).AddTo(this);
    }

    private void Drag(Vector2 v)
    {
        //Check NULL
        if (GameData.gameData == null) return;
        if (GameData.gameData.SelectedObject == null || GameData.gameData.EditMode == GameData.EditModes.IDLE) return;
        if (v.y > 0 && (moneyBarProgress.GetAmount() <= 0 || co2BarProgress.GetAmount() >= GameData.gameData.CO2Limit)) return;


        var selectedObject = GameData.gameData.SelectedObject.transform;
        var parentSelectedObject = selectedObject.parent;

        if (parentSelectedObject.transform.localScale.y >= maxScale && v.y > 0) return;

        //Modify Height
        if (GameData.gameData.EditMode == GameData.EditModes.MODIFY_HEIGHT)
        {
            if (GameData.gameData.SelectedObject.activeSelf)
            {
                var scale = v.y * scalingFactor * Time.deltaTime;
                parentSelectedObject.localScale += new Vector3(0f, scale, 0f);
                if (parentSelectedObject.localScale.y > 0.05 || scale > 0)
                {
                    CalculateMoneyAndCO2(selectedObject, scale);
                }
            }
            else return;
        }

        //Clamping
        parentSelectedObject.localScale = new Vector3(
            Mathf.Clamp(parentSelectedObject.localScale.x, minScale, maxScale),
            Mathf.Clamp(parentSelectedObject.localScale.y, minScale, maxScale),
            Mathf.Clamp(parentSelectedObject.localScale.z, minScale, maxScale));

        //Set Text
       textHandler.setBuildingHeightText(parentSelectedObject.localScale.y);

        //Set Tiling
        var tilingY = Mathf.Floor(parentSelectedObject.localScale.y / 2.7f);
        selectedObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, tilingY);

        var baseAttributes = selectedObject.GetComponent<BaseAttributes>();
        //Set Door
        if (parentSelectedObject.transform.localScale.y <= 3f)
        {
            baseAttributes.GetDoor().SetActive(false);
            baseAttributes.SetDoorActive(false);
        }
        else if (parentSelectedObject.transform.localScale.y < 8.1f)
        {
            GameObject door = baseAttributes.GetDoor();
            door.transform.localScale = new Vector3(0.2f, 0.005f, 0.015f);
            door.transform.localPosition = new Vector3(0, 0.0025f, -0.5125f);
        }
        else if (parentSelectedObject.transform.localScale.y < 10.8f)
        {
            GameObject door = baseAttributes.GetDoor();
            door.transform.localScale = new Vector3(0.2f, 0.0035f, 0.015f);
            door.transform.localPosition = new Vector3(0, 0.00175f, -0.5125f);
        }
        else
        {
            GameObject door = baseAttributes.GetDoor();
            door.transform.localScale = new Vector3(0.2f, 0.0022f, 0.015f);
            door.transform.localPosition = new Vector3(0, 0.0011f, -0.5125f);
        }
    }

    private void CalculateMoneyAndCO2(Transform obj, float scale)
    {
        var baseAttributes = obj.GetComponent<BaseAttributes>();
        bool hasSolar = baseAttributes.GetSolar();
        var objectMaterial = baseAttributes.GetObjectMaterial();

        //Cost Scale and Emission
        var emissionCO2 = 1f;
        var costScale = 1f;
        if (objectMaterial == BaseAttributes.ObjectMaterial.Brick)
        {
            emissionCO2 = 5.7f;
            costScale = 1.1f;
        } 
        else if (objectMaterial == BaseAttributes.ObjectMaterial.Wood)
        {
            emissionCO2 = 2.7f;
            costScale = 0.9f;
        }
        else if (objectMaterial == BaseAttributes.ObjectMaterial.Steel)
        {
            emissionCO2 = 10.3f;
            costScale = 1.2f;
        }
        else if (objectMaterial == BaseAttributes.ObjectMaterial.Concrete)
        {
            emissionCO2 = 8.1f;
            costScale = 1f;
        }

        //Squaremeter
        float squaremeter = scale * baseAttributes.GetLength() * baseAttributes.GetWidth() / 2.7f;
        //CO2
        float amountCO2 = 0;
        if (hasSolar)
        {
            amountCO2 += 8.5f * squaremeter;
        }
        else
        {
            amountCO2 += 7.5f * squaremeter;
        }
        amountCO2 += emissionCO2 * squaremeter;
        Debug.Log(amountCO2);
        baseAttributes.AddCO2(amountCO2);
        co2BarProgress.GainAmount(amountCO2);

        //Money
        float amountCost = 0;
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
        baseAttributes.AddCost(amountCost);
        moneyBarProgress.LoseAmount(amountCost);
    }

    private void EndDrag(EventArgs e)
    {
        if (GameData.gameData == null) return;
        if (GameData.gameData.SelectedObject == null || GameData.gameData.EditMode == GameData.EditModes.IDLE) return;

        //Height
        Transform parent = GameData.gameData.SelectedObject.transform.parent;
        float height = parent.localScale.y;
        height = Mathf.Round(height * 100) / 100;
        textHandler.setBuildingHeightText(height);

        //Round Money
        moneyBarProgress.RoundPercentageText();
        //Round CO2
        co2BarProgress.RoundPercentageText();
    }
}
