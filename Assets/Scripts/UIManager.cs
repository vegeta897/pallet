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
    public PendingItemActions PanPendingItemActions;
    public PendingItemActions PanActiveItemActions;

    private int selectedPendingID;
    private int selectedActiveID;
    private Dictionary<int, PendingActiveItem> pendingItems = new Dictionary<int, PendingActiveItem>();
    private Dictionary<int, PendingActiveItem> activeItems = new Dictionary<int, PendingActiveItem>();

    public void SetTimescale(int multi)
    {
        Warehouse.Timescale = multi;
        Btn1x.Highlight(false);
        Btn2x.Highlight(false);
        Btn4x.Highlight(false);
        Btn8x.Highlight(false);
        switch (multi)
        {
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

    public void Pause()
    {
        Warehouse.Paused = !Warehouse.Paused;
        BtnPause.Highlight(Warehouse.Paused);
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

    public void SelectPendingActiveItem(int id, string itemType)
    {
        if(itemType == "pending")
        {
            selectedPendingID = id;
            PanPendingItemActions.gameObject.SetActive(id >= 0);
        }
        else
        {
            selectedActiveID = id;
            PanActiveItemActions.gameObject.SetActive(id >= 0);
        }
        foreach (KeyValuePair<int, PendingActiveItem> item in (itemType == "pending" ? pendingItems : activeItems))
        {
            item.Value.Selected = item.Key == id;
        }
    }

    public void AcceptDelivery()
    {
        PanPendingItemActions.gameObject.SetActive(false);
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
        PanPendingItemActions.gameObject.SetActive(false);
        Warehouse.RejectDelivery(selectedPendingID);
        GameObject.Destroy(pendingItems[selectedPendingID].gameObject);
        pendingItems.Remove(selectedPendingID);
        selectedPendingID = -1;
    }

    void Start()
    {
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
        BtnPause.onClick.AddListener(() => Pause());
        Btn1x.onClick.AddListener(() => SetTimescale(1));
        Btn2x.onClick.AddListener(() => SetTimescale(2));
        Btn4x.onClick.AddListener(() => SetTimescale(4));
        Btn8x.onClick.AddListener(() => SetTimescale(8));
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        TxtMoney.text = "$" + Warehouse.Money.ToString("F2");
        TxtWorkers.text = "Workers: " + Warehouse.Workers;
        TxtUntilPayday.text = "Next Payday: " + Mathf.CeilToInt((float)(Warehouse.NextPayday - Warehouse.Tick)/48f)+ " days";
        TxtPaydayAmount.text = "Payday Cost: $" + (Warehouse.Workers * Warehouse.Wage).ToString("F2");
        TxtStockAmount.text = "Stock Count: " + Warehouse.Stock;
        BtnFire.interactable = Warehouse.Workers > 0;
        int hour = Mathf.FloorToInt(Warehouse.Tick / 2) + 7; // Convert ticks to hours with +7 hour offset
        TxtTime.text = (hour % 12 == 0 ? 12 : hour % 12) + ":00";
        TxtTimeAMPM.text = hour % 24 > 11 ? "PM" : "AM";
    }
}