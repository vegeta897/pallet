using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public event ActionItemSelected OnActionItemSelected;

    public Warehouse Warehouse;
    public WorkerManager WorkerManager;
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
    public Text TxtTotalStock;
    public Text TxtStockRacked;
    public Text TxtStockUnloaded;
    public Text TxtStockPicked;
    public WorkerList WorkerList;
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

    private ActionItem selectedActionItem;
    private Worker selectedWorker;
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

    public void HireWorker()
    {
        Worker newWorker = WorkerManager.HireWorker();
        BtnWorker newBtnWorker = WorkerList.AddWorker(newWorker);
        newBtnWorker.OnWorkerSelected += WorkerSelected;
    }

    public void FireWorker()
    {
        WorkerManager.FireWorker(selectedWorker);
        WorkerList.RemoveWorker(selectedWorker);
    }

    private void WorkerSelected(Worker worker)
    {
        selectedWorker = worker;
        BtnFire.interactable = worker != null;
    }

    public void SetWage()
    {
        Warehouse.Wage = (decimal)Mathf.Round(float.Parse(InpWage.text) * 100) / 100;
        InpWage.text = Warehouse.Wage.ToString("F2");
    }

    private void ActionItemAdded(ActionItem newActionItem)
    {
        BtnActionItem newBtnActionItem = Instantiate(BtnActionItem) as BtnActionItem;
        newBtnActionItem.transform.SetParent(PanPendingItems.transform, false);
        newBtnActionItem.Item = newActionItem;
        newBtnActionItem.UIManager = this;
        newBtnActionItem.OnActionItemSelected += ActionItemSelected;
        actionItems[newActionItem] = newBtnActionItem;
    }

    private void ActionItemRemoved(ActionItem removedActionItem)
    {
        if(selectedActionItem == removedActionItem)
        {
            ActionItemSelected(null);
        }
        actionItems[removedActionItem].OnActionItemSelected -= ActionItemSelected;
        GameObject.Destroy(actionItems[removedActionItem].gameObject);
        actionItems.Remove(removedActionItem);
    }

    private void ActionItemSelected(ActionItem actionItem)
    {
        selectedActionItem = actionItem;
        OnActionItemSelected(actionItem);
    }

    public void AcceptItem(ActionItem item)
    {
        actionItems[item].transform.SetParent(PanActiveItems.transform, false);
        OnActionItemSelected(item);
        Warehouse.AcceptItem(item);
    }

    public void RemoveActionItem(ActionItem actionItem)
    {
        Warehouse.RemoveActionItem(actionItem);
    }

    public void Click(BaseEventData data)
    {
        ActionItemSelected(null);
    }

    void Start()
    {
        Warehouse.OnActionItemAdded += ActionItemAdded;
        Warehouse.OnActionItemRemoved += ActionItemRemoved;

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
        foreach (Transform child in WorkerList.transform) // Remove editor placeholder
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
        TxtWorkers.text = "Workers: <b>" + WorkerManager.WorkerCount + "</b>";
        TxtUntilPayday.text = "Next Payday: <b>" + Mathf.CeilToInt((float)(Warehouse.NextPayday - Time.time) / 64f) + " days</b>";
        TxtPaydayAmount.text = "Payday Cost: <b>$" + (WorkerManager.WorkerCount * Warehouse.Wage).ToString("F2") + "</b>";
        TxtTotalStock.text = "Total Stock: <b>" + (Warehouse.StockRacked + Warehouse.StockPicked + Warehouse.StockUnloaded) + "</b>";
        TxtStockRacked.text = Warehouse.StockRacked.ToString();
        TxtStockUnloaded.text = Warehouse.StockUnloaded.ToString();
        TxtStockPicked.text = Warehouse.StockPicked.ToString();
        TxtTime.text = (Warehouse.Hour() % 12 == 0 ? 12 : Warehouse.Hour() % 12) + ":00";
        TxtTimeAMPM.text = Warehouse.Hour() % 24 > 11 ? "PM" : "AM";
    }
}