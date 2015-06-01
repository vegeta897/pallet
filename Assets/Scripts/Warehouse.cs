﻿using UnityEngine;
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

    private void AddActionItem(ActionItem newActionItem)
    {
        actionItems.Add(newActionItem);
        if (OnActionItemAdded != null)
        {
            OnActionItemAdded(newActionItem);
        }
    }

    public void RemoveActionItem(ActionItem removedActionItem)
    {
        ScriptableObject.Destroy(removedActionItem);
        actionItems.Remove(removedActionItem);
        if (OnActionItemRemoved != null)
        {
            OnActionItemRemoved(removedActionItem);
        }
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
                    money -= WorkerManager.WorkerCount * 50;
                    NextPayday = seconds + PaydayInterval;
                }
                if (DeliveryInterval == 0 || seconds % DeliveryInterval == 0) // Create an order on start
                {
                    Delivery newDelivery = ScriptableObject.CreateInstance("Delivery") as Delivery;
                    lastDeliveryID += 1;
                    newDelivery.Init("delivery", lastDeliveryID, Random.Range(2, 8) * 5);
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
                        newOrder.Init("order", lastOrderID, Random.Range(3, 6) * Random.Range(1, 5));
                        AddActionItem(newOrder);
                    }
                }
                int busyWorkers = 0;
                for (int i = actionItems.Count-1; i >= 0; i--) // Process current deliveries and orders
                {

                    // TODO: Send actionable deliveries and orders to WorkerManager with AddTask

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
                            int thisStockUnloaded = Mathf.Min((item.Quantity - d.QtyUnloaded), WorkerManager.WorkerCount - busyWorkers);
                            stockUnloaded += thisStockUnloaded;
                            d.QtyUnloaded += thisStockUnloaded;
                            busyWorkers += thisStockUnloaded;

                            if (d.QtyUnloaded >= item.Quantity)
                            {
                                RemoveActionItem(item);
                            }
                        }
                    }
                    else if(item.Type == "order")
                    {
                        Order o = item as Order;
                        if(item.Status == "picking")
                        {
                            int thisStockPicked = Mathf.Min((item.Quantity - o.QtyPicked), WorkerManager.WorkerCount - busyWorkers);
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
                            int thisStockLoaded = Mathf.Min((item.Quantity - o.QtyLoaded), WorkerManager.WorkerCount - busyWorkers);
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
                            RemoveActionItem(item);
                        }
                    }
                }
                // If workers are free after handling orders/deliveries, transfer unloaded stock to racks
                int thisStockRacked = Mathf.Min(WorkerManager.WorkerCount - busyWorkers, stockUnloaded);
                stockUnloaded -= thisStockRacked;
                stockRacked += thisStockRacked;
            }
        }
    }

    void Awake ()
    {

    }

	void Start ()
    {

        NextPayday = PaydayInterval;
        OrderInterval = 60; // Every day
        StartCoroutine(DoTick()); // Begin ticking
	}
	
	void Update ()
    {

	}
}