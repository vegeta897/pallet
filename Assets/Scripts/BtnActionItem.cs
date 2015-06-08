using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public delegate void ActionItemSelected(ActionItem selectedActionItem);

public class BtnActionItem : MonoBehaviour, IPointerClickHandler
{
    public event ActionItemSelected OnActionItemSelected;

    public UIManager UIManager
    {
        get
        {
            return uiManager;
        }
        set
        {
            uiManager = value;
            uiManager.OnActionItemSelected += ActionItemSelected;
        }
    }
    public Text TxtTitle;
    public Text TxtDescription;
    public Text TxtQuantity;
    public RawImage ImgProgress;

    private UIManager uiManager;
    private Button thisButton;
    private ColorBlock defaultColors;
    private ColorBlock selectedColors;
    private ActionItem item;

    public ActionItem Item
    {
        get
        {
            return item;
        }
        set
        {
            item = value;
            TxtQuantity.text = item.Quantity.ToString(); // Update quantity text
            TxtTitle.text = item.Type.Substring(0, 1).ToUpper() + item.Type.Substring(1, item.Type.Length - 1) + 
                (item.Status == "new" ? " Request" : " #<b>" + item.ID + "</b>");
            TxtDescription.text = item.Type == "delivery" ? "The distributor would like to send stock to your warehouse" :
                "A client wants to purchase some of your stock";
        }
    }
    public void ActionItemSelected(ActionItem selectedActionItem)
    {
        if (thisButton == null) // TODO: Need to stop this monobehavior when object destroyed
        {
            uiManager.OnActionItemSelected -= ActionItemSelected;
        }
        else
        {
            thisButton.colors = item == selectedActionItem ? selectedColors : defaultColors;
        }
    }

    public void OnPointerClick(PointerEventData data)
    {
        if(data.pointerId == -2 && item.Status == "new") // Right click
        {
            UIManager.RemoveActionItem(item); // Delete if pending item
        }
        else
        {
            OnActionItemSelected(item);
        }
    }

	void Start ()
    {
        thisButton = gameObject.GetComponent<Button>();
        defaultColors = thisButton.colors;
        selectedColors = defaultColors;
        selectedColors.normalColor = new Color(0.78F, 1F, 0.78F, 1F);
        selectedColors.highlightedColor = new Color(0.9F, 1F, 0.9F, 1F);
	}
	
	void Update () 
    {

	}
    void LateUpdate()
    {
        ImgProgress.gameObject.SetActive(item.StepProgress() >= 0);
        if(item.StepProgress() >= 0)
        {
            ImgProgress.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, 560, item.StepProgress()));
        }
        TxtDescription.text = item.StepDescription();
    }
}