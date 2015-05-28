using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PendingActiveActions : MonoBehaviour 
{
    private PendingActiveItem item;

    public UIManager UIManager;
    public string type;
    public Button BtnAccept;
    public Button BtnCancel;
    public Button BtnOption1;
    public PendingActiveItem Item
    {
        get
        {
            return item;
        }
        set
        {
            item = value;
            gameObject.SetActive(value != null);
        }
    }

    public void Accept()
    {
        if(type == "pending")
        {
            Item.Delivery.Accepted = true;
            UIManager.AcceptDelivery();
            gameObject.SetActive(false);
        }
        else if(type == "active")
        {
            Item.Delivery.Unloading = true;
        }
    }

    public void Cancel()
    {
        if (type == "pending")
        {
            UIManager.RejectDelivery();
        }
        else if (type == "active")
        {
            UIManager.RemoveItem(item.Delivery.DeliveryID);
        }
        gameObject.SetActive(false);
    }

	void Start () 
    {
        gameObject.SetActive(false);
	}
	
	void Update () 
    {
	    
	}

    void LateUpdate()
    {
        if(item != null)
        {
            BtnAccept.gameObject.SetActive(!item.Delivery.Accepted || item.Delivery.Delivered);
        }
    }
}