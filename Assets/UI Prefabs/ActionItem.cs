using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ActionItem : MonoBehaviour, IDeselectHandler, IPointerClickHandler
{
    public Text TxtQuantity;
    private Button thisButton;
    private ColorBlock defaultColors;
    private ColorBlock selectedColors;
    private Delivery delivery;
    public Delivery Delivery
    {
        get
        {
            return Delivery;
        }
        set
        {
            delivery = value;
            TxtQuantity.text = delivery.Quantity.ToString();
        }
    }

    public void OnDeselect(BaseEventData data)
    {
        Debug.Log("action item " + delivery.Index + "deselected");
        thisButton.colors = defaultColors;
    }

    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log("action item " + delivery.Index + " clicked!");
        thisButton.Select();
        thisButton.colors = selectedColors;
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