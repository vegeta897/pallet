using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ActionItem : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    public Text TxtQuantity;
    private Button thisButton;
    private ColorBlock defaultColors;
    private ColorBlock selectedColors;
    private Delivery delivery;
    private UIManager uiManager;
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
        }
    }

    public void OnDeselect(BaseEventData data)
    {
        Debug.Log("action item " + delivery.Index + "deselected");
        thisButton.colors = defaultColors;
        uiManager.SelectActionItem(-1); // Hide action item actions panel
    }

    public void OnSelect(BaseEventData data)
    {
        Debug.Log("action item " + delivery.Index + " selected!");
        thisButton.Select();
        thisButton.colors = selectedColors;
        uiManager.SelectActionItem(delivery.Index); // Show action item actions panel
    }

	void Start ()
    {
        thisButton = this.gameObject.GetComponent<Button>();
        defaultColors = thisButton.colors;
        selectedColors = defaultColors;
        selectedColors.highlightedColor = new Color(0.78F, 1F, 0.78F, 1F);
	}
	
	void Update () 
    {
	    
	}
}