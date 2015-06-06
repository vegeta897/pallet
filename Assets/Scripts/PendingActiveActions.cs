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

    public void Accept()
    {
        selectedItem.StepForward();
        if (selectedItem.Status == "accepted")
        {
            UIManager.AcceptItem(selectedItem);
            gameObject.SetActive(false);
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
            BtnAccept.interactable = selectedItem.WaitingForInput();
            TxtAccept.text = selectedItem.ForwardText();
        }
    }
}