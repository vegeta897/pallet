using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Warehouse : MonoBehaviour 
{

    private decimal money = 2000;
    private int workers = 2;
    private decimal wage = 50;
    private int stockRacked = 0;
    private int stockPicked = 0;
    private int stockUnloaded = 0;
    private List<ActionItem> actionItems = new List<ActionItem>();

    public UIManager UIManager;
    public int LastDeliveryID = 0;
    public int LastOrderID = 0;
    public float Timescale = 1;

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
    public int Workers
    {
        get
        {
            return workers;
        }
        set
        {
            workers = value;
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

    public void HireWorker()
    {
        workers += 1;
    }

    public void FireWorker()
    {
        workers -= workers == 0 ? 0 : 1;
    }

    public void AcceptItem(ActionItem item)
    {
        money -= item.Type == "delivery" ? item.Quantity * 15 : 0;
    }

    public void RemoveItem(ActionItem item)
    {
        ScriptableObject.Destroy(item);
        actionItems.Remove(item);
    }

    public int Hour() // Returns 0-23 on 24-hour clock, +7 hour offset
    {
        return (Mathf.FloorToInt(Time.time / 2.5f) + 7) % 24;
    }

    IEnumerator DoTick()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            {
                int seconds = Mathf.FloorToInt(Time.time);
                if (seconds % (PaydayInterval) == 0) // If it is payday
                {
                    money -= workers * 50;
                    NextPayday = seconds + PaydayInterval;
                }
                if (seconds % (DeliveryInterval) == 0 || seconds == 1) // Create an order on start
                {
                    Delivery newDelivery = ScriptableObject.CreateInstance("Delivery") as Delivery;
                    LastDeliveryID += 1;
                    newDelivery.Init("delivery", LastDeliveryID, Random.Range(2, 8) * 5);
                    actionItems.Add(newDelivery);
                    UIManager.AddItem(newDelivery);
                    DeliveryInterval = Random.Range(1,3) * 60; // Next delivery request in 1-3 days
                }
                if (seconds % (OrderInterval) == 0)
                {
                    int newOrderCount = Random.Range(0, 2);
                    for(int i = 0; i < newOrderCount; i++)
                    {
                        Order newOrder = ScriptableObject.CreateInstance("Order") as Order;
                        LastOrderID += 1;
                        newOrder.Init("order", LastOrderID, Random.Range(3, 6) * Random.Range(1, 5));
                        actionItems.Add(newOrder);
                        UIManager.AddItem(newOrder);
                    }
                }
                int busyWorkers = 0;
                for (int i = actionItems.Count-1; i >= 0; i--) // Process current deliveries and orders
                {
                    ActionItem item = actionItems[i];
                    if(item.Type == "delivery")
                    {
                        Delivery d = item as Delivery;
                        if (item.Status == "accepted" && Hour() < 17 && Hour() > 4) // Delivers between 5AM - 4PM
                        {
                            item.Status = "delivering";
                        }
                        else if (item.Status == "delivering" && item.TimeRemaining() <= 0)
                        {
                            item.Status = "delivered";
                        }
                        else if(item.Status == "unloading")
                        {
                            int thisStockUnloaded = Mathf.Min((item.Quantity - d.QtyUnloaded), workers - busyWorkers);
                            stockUnloaded += thisStockUnloaded;
                            d.QtyUnloaded += thisStockUnloaded;
                            busyWorkers += thisStockUnloaded;

                            if (d.QtyUnloaded >= item.Quantity)
                            {
                                UIManager.RemoveItem(item);
                            }
                        }
                    }
                    else if(item.Type == "order")
                    {
                        Order o = item as Order;
                        if(item.Status == "picking")
                        {
                            int thisStockPicked = Mathf.Min((item.Quantity - o.QtyPicked), workers - busyWorkers);
                            thisStockPicked = Mathf.Min(thisStockPicked, stockRacked);
                            stockRacked -= thisStockPicked;
                            stockPicked += thisStockPicked;
                            o.QtyPicked += thisStockPicked;
                            busyWorkers += thisStockPicked;
                            if (o.QtyPicked >= item.Quantity)
                            {
                                item.Status = "picked";
                            }
                        }
                        else if (item.Status == "loading")
                        {
                            int thisStockLoaded = Mathf.Min((item.Quantity - o.QtyLoaded), workers - busyWorkers);
                            thisStockLoaded = Mathf.Min(thisStockLoaded, stockPicked);
                            stockPicked -= thisStockLoaded;
                            o.QtyLoaded += thisStockLoaded;
                            busyWorkers += thisStockLoaded;
                            if (o.QtyLoaded >= item.Quantity)
                            {
                                item.Status = "loaded";
                            }
                        }
                        else if (item.Status == "shipping" && item.TimeRemaining() <= 0)
                        {
                            money += item.Quantity * 25;
                            UIManager.RemoveItem(item);
                        }
                    }
                }
                // If workers are free after handling orders/deliveries, transfer unloaded stock to racks
                int thisStockRacked = Mathf.Min(workers - busyWorkers, stockUnloaded);
                stockUnloaded -= thisStockRacked;
                stockRacked += thisStockRacked;
            }
        }
    }

	void Start ()
    {
        NextPayday = PaydayInterval;
        DeliveryInterval = Random.Range(3, 5) * 60; // 3-5 days
        OrderInterval = 60; // Every day
        StartCoroutine(DoTick()); // Begin ticking
	}
	
	void Update ()
    {

	}

    void LateUpdate ()
    {

    }
}