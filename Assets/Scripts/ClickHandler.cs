using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class ClickHandler : MonoBehaviour
{
    public IObservable<EventArgs> clickTrigger;
    private Renderer myRenderer;
    private TextHandler textHandler;

    private void Awake()
    {
        textHandler = FindObjectOfType<TextHandler>();
        myRenderer = GetComponent<Renderer>();
        if (gameObject.GetComponent<ObservablePointerClickTrigger>() == null)
        {
            clickTrigger = gameObject.AddComponent<ObservablePointerClickTrigger>()
                .OnPointerClickAsObservable()
                .Select(e => EventArgs.Empty);
        }
        else
        {
            clickTrigger = gameObject.GetComponent<ObservablePointerClickTrigger>()
                .OnPointerClickAsObservable()
                .Select(e => EventArgs.Empty);
        }
    }

    private void Start()
    {
        myRenderer.material.SetFloat("_OutlineWidth", 1);
        clickTrigger.Subscribe(clickOutput).AddTo(this);
    }

    public void clickOutput(EventArgs e)
    {
        //Check GameData
        if (GameData.gameData == null) return;

        //Check Selected Object
        if (GameData.gameData.SelectedObject == this.gameObject) return;

        //Undo Outline
        if (GameData.gameData.SelectedObject != null)
        {
            GameData.gameData.SelectedObject.GetComponent<ClickHandler>().UndoOutline();
        }

        //Edit Mode
        GameData.gameData.EditMode = GameData.EditModes.MODIFY_HEIGHT;

        //Set Text
        SetBuildingAttributesText();

        //Apply Outline
        GameData.gameData.SelectedObject = this.gameObject;
        ApplyOutline();
    }

    private void SetBuildingAttributesText()
    {
        var parent = this.gameObject.transform.parent;
        textHandler.setBuildingHeightText(parent.localScale.y);
    }

    public void UndoOutline()
    {
        myRenderer.material.SetFloat("_OutlineWidth", 1);
    }

    public void ApplyOutline()
    {
        myRenderer.material.SetFloat("_OutlineWidth", 1.0175f);
        myRenderer.material.SetColor("_OutlineColor", new Color(1f, 0.64f, 0f));
    }
}
