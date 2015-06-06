using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void ActionItemAdded(ActionItem newActionItem);
public delegate void ActionItemRemoved(ActionItem removedActionItem);

public class Warehouse : MonoBehaviour
{
    public event ActionItemAdded OnActionItemAdded;
    public event ActionItemRemoved OnActionItemRemoved;

    private decimal money = 2000;
    private decimal wage = 50;
    private int stockRacked = 0;
    private int stockPicked = 0;
    private int stockUnloaded = 0;
    private List<ActionItem> actionItems = new List<ActionItem>();
    private int lastDeliveryID = 0;
    private int lastOrderID = 0;

    public float Timescale = 1;
    public WorkerManager WorkerManager;

    public decimal Money
    {
        get 
        {
            return money;
        }
        set 
        {
            money = value;
        }
    }
    public decimal Wage
    {
        get
        {
            return wage;
        }
        set
        {
            wage = value;
        }
    }
    public int StockRacked
    {
        get
        {
            return stockRacked;
        }
        set
        {
            stockRacked = value;
        }
    }
    public int StockPicked
    {
        get
        {
            return stockPicked;
        }
        set
        {
            stockPicked = value;
        }
    }
    public int StockUnloaded
    {
        get
        {
            return stockUnloaded;
        }
        set
        {
            stockUnloaded = value;
        }
    }

    // 1 hour = 2.5 seconds
    // 1 day = 60 seconds
    public static int PaydayInterval = 14 * 60; // 14 days
    public int NextPayday;
    private int DeliveryInterval;
    private int OrderInterval;

    public void AcceptItem(ActionItem item)
    {
        money -= item.Type == "delivery" ? item.Quantity * 15 : 0;
    }

    public void OnStockProcessed(string destination, int qty)
    {
        switch(destination)
        {
            case "unloaded":
                StockUnloaded += qty;
                break;
            case "picked":
                StockPicked += qty;
                StockRacked -= qty;
                break;
            case "loaded":
                StockPicked -= qty;
                break;
            case "racked":
                int actualQty = Mathf.Min(StockUnloaded, qty);
                StockUnloaded -= actualQty;
                StockRacked += actualQty;
                WorkerManager.StockRacking = Mathf.Max(0,WorkerManager.StockRacking-qty);
                break;
        }
    }

    private void AddActionItem(ActionItem newActionItem)
    {
        actionItems.Add(newActionItem);
        newActionItem.OnStockProcessed += OnStockProcessed;
        if (OnActionItemAdded != null)
        {
            OnActionItemAdded(newActionItem);
        }
    }

    public void RemoveActionItem(ActionItem removedActionItem)
    {
        Money += removedActionItem.Status == "complete" ? removedActionItem.Quantity * 25 : 0;
        actionItems.Remove(removedActionItem);
        if (OnActionItemRemoved != null)
        {
            OnActionItemRemoved(removedActionItem);
        }
        ScriptableObject.Destroy(removedActionItem);
    }

    IEnumerator DoTick()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            int seconds = Mathf.FloorToInt(Utility.GetTime());
            if (seconds % (PaydayInterval) == 0) // If it is payday
            {
                money -= WorkerManager.WorkerCount * 50;
                NextPayday = seconds + PaydayInterval;
            }
            if (DeliveryInterval == 0 || seconds % DeliveryInterval == 0) // Create delivery on start
            {
                Delivery newDelivery = ScriptableObject.CreateInstance("Delivery") as Delivery;
                lastDeliveryID += 1;
                newDelivery.Init(lastDeliveryID, Random.Range(2, 8) * 5);
                AddActionItem(newDelivery);
                DeliveryInterval = Random.Range(3, 5) * 60; // Next request in 3-5 days
            }
            if (seconds % OrderInterval == 0)
            {
                int newOrderCount = Random.Range(0, 2);
                for(int i = 0; i < newOrderCount; i++)
                {
                    Order newOrder = ScriptableObject.CreateInstance("Order") as Order;
                    lastOrderID += 1;
                    newOrder.Init(lastOrderID, Random.Range(3, 6) * Random.Range(1, 5));
                    AddActionItem(newOrder);
                }
            }
        }
    }

    void Awake ()
    {

    }

	void Start ()
    {
        WorkerManager.Init(actionItems);
        NextPayday = PaydayInterval;
        OrderInterval = 60; // Every day
        StartCoroutine(DoTick()); // Begin ticking
	}
	
	void Update ()
    {

	}
}