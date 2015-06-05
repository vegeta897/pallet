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
        ImgProgress.gameObject.SetActive(false);
        if(item.Type == "delivery" && item.Status == "accepted")
        {
            TxtDescription.text = "Delivery will depart in the morning";
        }
        if (item.Status == "delivering")
        {
            Delivery d = item as Delivery;
            ImgProgress.gameObject.SetActive(true);
            ImgProgress.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, 560, (d.DeliveryTime - d.TimeRemaining()) / d.DeliveryTime));
            TxtDescription.text = "Arrives in <b>" + Mathf.CeilToInt(d.TimeRemaining() / 2.5f) + "</b> hours";
        }
        if (item.Status == "delivered")
        {
            TxtDescription.text = "Arrived, waiting to unload";
        }
        if(item.Status == "unloading")
        {
            Delivery d = item as Delivery;
            ImgProgress.gameObject.SetActive(true);
            ImgProgress.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, 560, (item.Quantity - d.QtyUnloaded) / (float)item.Quantity));
            TxtDescription.text = "Unloading stock <b>" + d.QtyUnloaded + "</b> of <b>" + item.Quantity + "</b>";
            TxtQuantity.text = (item.Quantity - d.QtyUnloaded).ToString();
        }
        if (item.Status == "picking")
        {
            Order o = item as Order;
            ImgProgress.gameObject.SetActive(true);
            ImgProgress.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, 560, o.QtyPicked / (float)item.Quantity));
            TxtDescription.text = "Picking stock <b>" + o.QtyPicked + "</b> of <b>" + item.Quantity + "</b>";
        }
        if (item.Status == "picked")
        {
            TxtDescription.text = "Stock picked and ready to load";
        }
        if (item.Status == "loading")
        {
            Order o = item as Order;
            ImgProgress.gameObject.SetActive(true);
            ImgProgress.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, 560, (item.Quantity - o.QtyLoaded) / (float)item.Quantity));
            TxtDescription.text = "Loading stock <b>" + o.QtyLoaded + "</b> of <b>" + item.Quantity + "</b>";
        }
        if (item.Status == "loaded")
        {
            TxtDescription.text = "Stock loaded and ready to ship";
        }
        if (item.Status == "shipping")
        {
            Order o = item as Order;
            ImgProgress.gameObject.SetActive(true);
            ImgProgress.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, 560, (o.ShippingTime - o.TimeRemaining()) / o.ShippingTime));
            TxtDescription.text = "Arrives at destination in <b>" + Mathf.CeilToInt(o.TimeRemaining() / 2.5f) + "</b> hours";
        }
    }
}