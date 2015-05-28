using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

    public Warehouse Warehouse;
    public Text TxtTime;
    public Text TxtTimeAMPM;
    public Button BtnPause;
    public Button Btn1x;
    public Button Btn2x;
    public Button Btn4x;
    public Button Btn8x;
    public Text TxtMoney;
    public Text TxtWorkers;
    public Text TxtUntilPayday;
    public Text TxtPaydayAmount;
    public Text TxtStockAmount;
    public Button BtnHire;
    public Button BtnFire;
    public InputField InpWage;
    public GameObject PanPendingList;
    public GameObject PanPendingItems;
    public PendingActiveItem BtnPendingItem;
    public GameObject PanActiveList;
    public GameObject PanActiveItems;
    public PendingActiveItem BtnActiveItem;
    public PendingActiveActions PanPendingItemActions;
    public PendingActiveActions PanActiveItemActions;

    private int selectedPendingID;
    private int selectedActiveID;
    private Dictionary<int, PendingActiveItem> pendingItems = new Dictionary<int, PendingActiveItem>();
    private Dictionary<int, PendingActiveItem> activeItems = new Dictionary<int, PendingActiveItem>();

    public void SetTimescale(int multi)
    {
        Time.timeScale = multi;
        BtnPause.Highlight(false);
        Btn1x.Highlight(false);
        Btn2x.Highlight(false);
        Btn4x.Highlight(false);
        Btn8x.Highlight(false);
        switch (multi)
        {
            case 0:
                BtnPause.Highlight();
                break;
            case 1:
                Btn1x.Highlight();
                break;
            case 2:
                Btn2x.Highlight();
                break;
            case 4:
                Btn4x.Highlight();
                break;
            case 8:
                Btn8x.Highlight();
                break;
        }
    }

    public void SetWage()
    {
        Warehouse.Wage = (decimal)Mathf.Round(float.Parse(InpWage.text) * 100) / 100;
        InpWage.text = Warehouse.Wage.ToString("F2");
    }

    public void AddPendingItem(Delivery newDelivery)
    {
        PendingActiveItem pendingItem = Instantiate(BtnPendingItem) as PendingActiveItem;
        pendingItem.transform.SetParent(PanPendingItems.transform, false);
        pendingItem.Delivery = newDelivery;
        pendingItem.ItemType = "pending";
        pendingItem.UIManager = this;
        pendingItems[newDelivery.DeliveryID] = pendingItem;
    }

    public void SelectItem(PendingActiveItem item)
    {
        if(item.ItemType == "pending")
        {
            selectedPendingID = item.Delivery.DeliveryID;
            PanPendingItemActions.Item = item;
        }
        else
        {
            selectedActiveID = item.Delivery.DeliveryID;
            PanActiveItemActions.Item = item;
        }
        foreach (KeyValuePair<int, PendingActiveItem> listItem in (item.ItemType == "pending" ? pendingItems : activeItems))
        {
            listItem.Value.Selected = listItem.Key == item.Delivery.DeliveryID;
        }
    }

    public void AcceptDelivery()
    {
        Warehouse.AcceptDelivery(selectedPendingID);
        PendingActiveItem activeItem = Instantiate(BtnActiveItem) as PendingActiveItem;
        activeItem.transform.SetParent(PanActiveItems.transform, false);
        activeItem.Delivery = pendingItems[selectedPendingID].Delivery;
        activeItem.ItemType = "active";
        activeItem.UIManager = this;
        activeItems[selectedPendingID] = activeItem;
        GameObject.Destroy(pendingItems[selectedPendingID].gameObject);
        pendingItems.Remove(selectedPendingID);
        selectedPendingID = -1;
    }

    public void RejectDelivery()
    {
        RemoveItem(selectedPendingID);
    }

    public void RemoveItem(int itemID)
    {
        Warehouse.RemoveItem(itemID);
        if(pendingItems.ContainsKey(itemID))
        {
            GameObject.Destroy(pendingItems[itemID].gameObject);
            PanPendingItemActions.Item = null;
            pendingItems.Remove(itemID);
        }
        if(activeItems.ContainsKey(itemID))
        {
            GameObject.Destroy(activeItems[itemID].gameObject);
            PanActiveItemActions.Item = null;
            activeItems.Remove(itemID);
        }
        selectedPendingID = selectedPendingID == itemID ? -1 : selectedPendingID;
        selectedActiveID = selectedActiveID == itemID ? -1 : selectedActiveID;

    }

    public void Click(BaseEventData data)
    {
        PanPendingItemActions.Item = null;
        PanActiveItemActions.Item = null;
        selectedPendingID = -1;
        selectedActiveID = -1;
        foreach (KeyValuePair<int, PendingActiveItem> item in pendingItems)
        {
            item.Value.Selected = false;
        }
        foreach (KeyValuePair<int, PendingActiveItem> item in activeItems)
        {
            item.Value.Selected = false;
        }
    }

    void Start()
    {
        

        BtnPause.onClick.AddListener(() => SetTimescale(0));
        Btn1x.onClick.AddListener(() => SetTimescale(1));
        Btn2x.onClick.AddListener(() => SetTimescale(2));
        Btn4x.onClick.AddListener(() => SetTimescale(4));
        Btn8x.onClick.AddListener(() => SetTimescale(8));
        Btn1x.Highlight();
        foreach (Transform child in PanPendingItems.transform) // Remove editor placeholder
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in PanActiveItems.transform) // Remove editor placeholder
        {
            GameObject.Destroy(child.gameObject);
        }
        InpWage.textComponent.alignment = TextAnchor.MiddleRight;
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        TxtMoney.text = "<b>$" + Warehouse.Money.ToString("F2") + "</b>";
        TxtWorkers.text = "Workers: <b>" + Warehouse.Workers + "</b>";
        TxtUntilPayday.text = "Next Payday: <b>" + Mathf.CeilToInt((float)(Warehouse.NextPayday - Time.time) / 48f) + " days</b>";
        TxtPaydayAmount.text = "Payday Cost: <b>$" + (Warehouse.Workers * Warehouse.Wage).ToString("F2") + "</b>";
        TxtStockAmount.text = "Stock Count: <b>" + Warehouse.Stock + "</b>";
        BtnFire.interactable = Warehouse.Workers > 0;
        int hour = Mathf.FloorToInt(Time.time / 2.5f) + 7; // Convert ticks to hours with +7 hour offset
        TxtTime.text = (hour % 12 == 0 ? 12 : hour % 12) + ":00";
        TxtTimeAMPM.text = hour % 24 > 11 ? "PM" : "AM";
    }
}