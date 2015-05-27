using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

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

    private Warehouse warehouse;
    private int selectedPendingID;
    private int selectedActiveID;
    private Dictionary<int, PendingActiveItem> pendingItems = new Dictionary<int, PendingActiveItem>();
    private Dictionary<int, PendingActiveItem> activeItems = new Dictionary<int, PendingActiveItem>();

    public void SetWage()
    {
        warehouse.Wage = (decimal)Mathf.Round(float.Parse(InpWage.text) * 100) / 100;
        InpWage.text = warehouse.Wage.ToString("F2");
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
        warehouse.AcceptDelivery(selectedPendingID);
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
        warehouse.RejectDelivery(selectedPendingID);
        GameObject.Destroy(pendingItems[selectedPendingID].gameObject);
        pendingItems.Remove(selectedPendingID);
        selectedPendingID = -1;
    }

    void Start()
    {
        warehouse = FindObjectOfType<Warehouse>();
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
        TxtMoney.text = "$" + warehouse.Money.ToString("F2");
        TxtWorkers.text = "Workers: " + warehouse.Workers;
        TxtUntilPayday.text = "Next Payday: " + Mathf.CeilToInt(warehouse.NextPayday - Time.time);
        TxtPaydayAmount.text = "Payday Cost: " + (warehouse.Workers * warehouse.Wage).ToString("F2");
        TxtStockAmount.text = "Stock Amount: " + warehouse.Stock;
        BtnFire.interactable = warehouse.Workers > 0;
    }
}