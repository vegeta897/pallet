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

    public void OnPointerClick(PointerEventData data)
    {
        thisButton.colors = selectedColors;
        uiManager.SelectItem(this); // Show item actions panel
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
        if(delivery.Accepted && !delivery.Unloading)
        {
            ImgProgress.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, 560, (delivery.DeliveryTime - delivery.TimeRemaining()) / delivery.DeliveryTime));
            TxtDescription.text = Delivery.TimeRemaining() <= 0 ? "Arrived, waiting to unload" :
                "Arrives in <b>" + Mathf.CeilToInt(Delivery.TimeRemaining() / 2.5f) + "</b> hours";
        }
        if(delivery.Unloading)
        {
            ImgProgress.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, 560, (delivery.Quantity - delivery.Unloaded) / (float)delivery.Quantity));
            TxtDescription.text = "Unloading";
            TxtQuantity.text = (delivery.Quantity - delivery.Unloaded).ToString();
        }
    }
}