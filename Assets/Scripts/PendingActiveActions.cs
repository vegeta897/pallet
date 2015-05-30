using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PendingActiveActions : MonoBehaviour 
{
    private ActionItem selectedItem;

    public string ListType;
    public UIManager UIManager;
    public Button BtnAccept;
    public Text TxtAccept;
    public Button BtnCancel;
    public Text TxtCancel;
    public Button BtnOption1;
    public Text TxtOption1;

    // TODO: Implement a linear flow for orders and deliveries, which they can move through forwards or back
    public void Accept()
    {
        string origStatus = selectedItem.Status;
        if (origStatus == "new")
        {
            selectedItem.Status = "accepted";
            UIManager.AcceptItem(selectedItem);
            gameObject.SetActive(false);
        }
        if (selectedItem.Type == "delivery")
        {
            if (origStatus == "delivered")
            {
                selectedItem.Status = "unloading";
            }
        }
        else if (selectedItem.Type == "order")
        {
            if (origStatus == "accepted")
            {
                selectedItem.Status = "picking";
            }
            if (origStatus == "picked")
            {
                selectedItem.Status = "loading";
            }
            if (origStatus == "loaded")
            {
                selectedItem.Status = "shipping";
            }
        }
    }

    public void Cancel()
    {
        UIManager.RemoveActionItem(selectedItem);
        gameObject.SetActive(false);
    }

    public void ActionItemSelected(ActionItem item)
    {
        selectedItem = item;
        if (selectedItem == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive((selectedItem.Status == "new" && ListType == "pending") || (selectedItem.Status != "new" && ListType == "active"));
        }
    }

	void Start () 
    {
        UIManager.OnActionItemSelected += ActionItemSelected;
        gameObject.SetActive(false);
	}
	
	void Update () 
    {
	    
	}

    void LateUpdate()
    {
        if(selectedItem != null)
        {
            BtnAccept.gameObject.SetActive(true);

            if (selectedItem.Type == "delivery")
            {
                switch (selectedItem.Status)
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
            else if (selectedItem.Type == "order")
            {
                switch (selectedItem.Status)
                {
                    case "new":
                        TxtAccept.text = "Accept";
                        break;
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