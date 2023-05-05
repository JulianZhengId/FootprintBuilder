using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class BarProgress : MonoBehaviour
{
    enum AmountType
    {
        Money,
        CO2
    }

    private Slider amountBarDisplay;
    [SerializeField] private TextMeshProUGUI personalPercentageText;
    [SerializeField] private TextMeshProUGUI personalAmountText;
    [Header("Main Variables:")]

    private float amount;
    private float amountMaxValue;
    [SerializeField] private AmountType amountType;

    private void Awake()
    {
        if (amountBarDisplay == null)
        {
            amountBarDisplay = GetComponent<Slider>();
        }

        amountBarDisplay.minValue = 0;
        if (amountType == AmountType.Money)
        {
            amountMaxValue = GameData.gameData.MoneyLimit;
            amount = amountMaxValue;
        }
        else if (amountType == AmountType.CO2)
        {
            amountMaxValue = GameData.gameData.CO2Limit;
            amount = 0;
        }
        amountBarDisplay.maxValue = amountMaxValue;
    }

    private void Start()
    {
        UpdateBar();
    }


    public void UpdateBar()
    {
        if (amount < 0)
        {
            amount = 0;
        }

        if (amount > amountMaxValue)
        {
            amount = amountMaxValue;
        }
        amountBarDisplay.value = amount;
        SetPercentageText(amount);
        personalAmountText.text = Mathf.RoundToInt(amount).ToString();
    }

    public void GainAmount(float value)
    {
        amount += value;
        UpdateBar();
    }

    public void LoseAmount(float value)
    {
        amount -= value;
        UpdateBar();
    }

    public void SetAmount(float value)
    {
        amount = value;
        UpdateBar();
    }

    private void SetPercentageText(float value)
    {
        if (amountType == AmountType.Money)
        {
            personalPercentageText.text = (amount / amountMaxValue) * 100 + " %";
        }
        else
        {
            personalPercentageText.text = (amount / amountMaxValue) * 500 + " %";
        }
    }

    public void RoundPercentageText()
    {
        if (amountType == AmountType.Money)
        {
            float value = amount / amountMaxValue * 100;
            value = Mathf.Round(value * 100) / 100;
            personalPercentageText.text = value + " %";
        }
        else
        {
            float value = amount / amountMaxValue * 500;
            value = Mathf.Round(value * 100) / 100;
            personalPercentageText.text = value + " %";
        }

    }

    public float GetAmount()
    {
        return amount;
    }
}