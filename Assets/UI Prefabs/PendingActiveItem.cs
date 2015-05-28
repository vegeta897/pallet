using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PendingActiveItem : MonoBehaviour, IPointerClickHandler
{
    public string ItemType;
    public Text TxtPendingTitle;
    public Text TxtActiveTitle;
    public Text TxtDescription;
    public Text TxtQuantity;
    public RawImage ImgProgress;

    private Button thisButton;
    private ColorBlock defaultColors;
    private ColorBlock selectedColors;
    private Delivery delivery;
    private UIManager uiManager;
    private bool selected;

    public UIManager UIManager
    {
        get 
        {
            return uiManager;
        }
        set
        {
            uiManager = value;
        }
    }
    public Delivery Delivery
    {
        get
        {
            return delivery;
        }
        set
        {
            delivery = value;
            TxtQuantity.text = delivery.Quantity.ToString(); // Update delivery quantity text
            if(delivery.Accepted)
            {
                TxtActiveTitle.text = "Delivery #<b>" + delivery.DeliveryID + "</b>";
            }
        }
    }
    public bool Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;
            thisButton.colors = value ? selectedColors : defaultColors;
        }
    }

    //public void OnDeselect(BaseEventData data)
    //{
    //    Debug.Log(ItemType + " item " + delivery.Index + "deselected");
    //    thisButton.colors = defaultColors;
    //    uiManager.SelectPendingActiveItem(-1,ItemType); // Hide item actions panel
    //}

    public void OnPointerClick(PointerEventData data)
    {
        thisButton.colors = selectedColors;
        uiManager.SelectPendingActiveItem(delivery.DeliveryID, ItemType); // Show item actions panel
    }

	void Start ()
    {
        thisButton = this.gameObject.GetComponent<Button>();
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
        if(delivery.Accepted)
        {
            ImgProgress.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, 560, (Delivery.DeliveryTime - Delivery.TimeRemaining()) / Delivery.DeliveryTime));
            TxtDescription.text = "Arrives in <b>" + Mathf.CeilToInt(Delivery.TimeRemaining() / 2.5f) + "</b> hours";
        }
    }
}