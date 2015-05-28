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
    public BtnActionItem BtnActionItem;
    public GameObject PanActiveList;
    public GameObject PanActiveItems;
    public BtnActionItem BtnActiveItem;
    public PendingActiveActions PanPendingItemActions;
    public PendingActiveActions PanActiveItemActions;

    private BtnActionItem selectedPendingItem;
    private BtnActionItem selectedActiveItem;
    private Dictionary<ActionItem, BtnActionItem> actionItems = new Dictionary<ActionItem, BtnActionItem>();

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

    public void AddItem(ActionItem newActionItem)
    {
        BtnActionItem newBtnActionItem = Instantiate(BtnActionItem) as BtnActionItem;
        newBtnActionItem.transform.SetParent(PanPendingItems.transform, false);
        newBtnActionItem.Item = newActionItem;
        newBtnActionItem.UIManager = this;
        actionItems[newActionItem] = newBtnActionItem;
    }

    public void SelectItem(BtnActionItem item)
    {
        if(item.Item.Status == "new")
        {
            selectedPendingItem = item;
            PanPendingItemActions.BtnItem = item;
        }
        else
        {
            selectedActiveItem = item;
            PanActiveItemActions.BtnItem = item;
        }
        foreach(KeyValuePair<ActionItem, BtnActionItem> entry in actionItems)
        {
            entry.Value.Selected = entry.Value == selectedPendingItem || entry.Value == selectedActiveItem;
        }
    }

    public void AcceptItem(ActionItem item)
    {
        BtnActionItem activeItem = Instantiate(BtnActiveItem) as BtnActionItem;
        activeItem.transform.SetParent(PanActiveItems.transform, false);
        activeItem.Item = item;
        activeItem.UIManager = this;
        GameObject.Destroy(actionItems[item].gameObject);
        actionItems[item] = activeItem;
        selectedPendingItem = null;
        Warehouse.AcceptItem(item);
    }

    public void RejectItem()
    {
        RemoveItem(selectedPendingItem.Item);
    }

    public void RemoveItem(ActionItem item)
    {
        Warehouse.RemoveItem(item);
        GameObject.Destroy(actionItems[item].gameObject);
        if(item.Status == "new")
        {
            PanPendingItemActions.BtnItem = null;
        }
        else if(selectedActiveItem == actionItems[item])
        {
            PanActiveItemActions.BtnItem = null;
        }
        actionItems.Remove(item);
    }

    public void Click(BaseEventData data)
    {
        PanPendingItemActions.BtnItem = null;
        PanActiveItemActions.BtnItem = null;
        selectedPendingItem = null;
        selectedActiveItem = null;
        foreach (KeyValuePair<ActionItem, BtnActionItem> item in actionItems)
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
        TxtUntilPayday.text = "Next Payday: <b>" + Mathf.CeilToInt((float)(Warehouse.NextPayday - Time.time) / 64f) + " days</b>";
        TxtPaydayAmount.text = "Payday Cost: <b>$" + (Warehouse.Workers * Warehouse.Wage).ToString("F2") + "</b>";
        TxtStockAmount.text = "Stock Count: <b>" + Warehouse.Stock + "</b>";
        BtnFire.interactable = Warehouse.Workers > 0;
        TxtTime.text = (Warehouse.Hour() % 12 == 0 ? 12 : Warehouse.Hour() % 12) + ":00";
        TxtTimeAMPM.text = Warehouse.Hour() % 24 > 11 ? "PM" : "AM";
    }
}