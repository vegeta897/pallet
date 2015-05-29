using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PendingActiveActions : MonoBehaviour 
{
    private BtnActionItem btnItem;
    private ActionItem item;

    public UIManager UIManager;
    public Button BtnAccept;
    public Text TxtAccept;
    public Button BtnCancel;
    public Text TxtCancel;
    public Button BtnOption1;
    public Text TxtOption1;
    public BtnActionItem BtnItem
    {
        get
        {
            return btnItem;
        }
        set
        {
            btnItem = value;
            item = value != null ? btnItem.Item : null;
            gameObject.SetActive(value != null);
        }
    }

    // TODO: Implement a linear flow for orders and deliveries, which they can move through forwards or back
    public void Accept()
    {
        string origStatus = item.Status;
        if (origStatus == "new")
        {
            item.Status = "accepted";
            UIManager.AcceptItem(item);
            gameObject.SetActive(false);
        } 
        if(item.Type == "delivery")
        {
            if (origStatus == "delivered")
            {
                item.Status = "unloading";
            }
        }
        else if(item.Type == "order")
        {
            if (origStatus == "accepted")
            {
                item.Status = "picking";
            }
            if (origStatus == "picked")
            {
                item.Status = "loading";
            }
            if (origStatus == "loaded")
            {
                item.Status = "shipping";
            }
        }
    }

    public void Cancel()
    {
        UIManager.RemoveItem(btnItem.Item);
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
        if(btnItem != null)
        {
            BtnAccept.gameObject.SetActive(true);

            if (item.Type == "delivery")
            {
                switch (item.Status)
                {
                    case "new":
                        TxtAccept.text = "Accept";
                        break;
                    case "accepted":
                        BtnAccept.gameObject.SetActive(false);
                        break;
                    case "delivering":
                        BtnAccept.gameObject.SetActive(false);
                        break;
                    case "delivered":
                        TxtAccept.text = "Unload";
                        break;
                }
            }
            else if(item.Type == "order")
            {
                switch (item.Status)
                {
                    case "accepted":
                        TxtAccept.text = "Pick";
                        break;
                    case "picking":
                        BtnAccept.gameObject.SetActive(false);
                        break;
                    case "picked":
                        TxtAccept.text = "Load";
                        break;
                    case "loading":
                        BtnAccept.gameObject.SetActive(false);
                        break;
                    case "loaded":
                        TxtAccept.text = "Ship";
                        break;
                    case "shipping":
                        BtnAccept.gameObject.SetActive(false);
                        break;
                }
            }
        }
    }
}