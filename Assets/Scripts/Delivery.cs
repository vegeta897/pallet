﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Delivery : ActionItem
{
    public override event StockProcessed OnStockProcessed;

    public Delivery()
    {
        progression = new List<string>() 
        {
            "new",
            "accepted",
            "delivering",
            "delivered",
            "unloading",
            "complete"
        };
        status = progression[0];
        Type = "delivery";
    }

    protected override List<string> progression { get; set; }
    private float acceptTime;
    private float deliveryDepartTime;
    private int qtyUnloaded;
    private string status;

    public override string Type { get; set; }
    public int DeliveryTime;
    public override string Status
    {
        get
        {
            return status;
        }
        set
        {
            status = value;
        }
    }
    public int QtyUnloaded
    {
        get
        {
            return qtyUnloaded;
        }
        set
        {
            qtyUnloaded = value;
            if(qtyUnloaded >= Quantity)
            {
                ScriptableObject.Destroy(this);
            }
        }
    }

    protected override void BeginStep()
    {
        switch (status)
        {
            case "accepted":
                acceptTime = Utility.GetTime();
                break;
            case "delivering":
                DeliveryTime = 60; // 1 day
                deliveryDepartTime = Utility.GetTime();
                break;
            case "delivered":

                break;
            case "unloading":

                break;
        }
    }

    public override float TimeRemaining()
    {
        return deliveryDepartTime + DeliveryTime - Utility.GetTime();
    }
    public override bool NeedWorkers()
    {
        return status == "unloading";
    }
    public override void AutoStep()
    {
        switch (status)
        {
            case "accepted":
                if (Utility.Hour() < 17 && Utility.Hour() > 4)
                {
                    StepForward();
                }
                break;
            case "delivering":
                if (TimeRemaining() <= 0)
                {
                    StepForward();
                }
                break;
            case "unloading":
                if (qtyUnloaded == Quantity)
                {
                    StepForward();
                }
                break;
        }
    }
    public override int QtyTime()
    {
        return 1;
    }
    public override void ProcessStock(int qty)
    {
        int processAmt;
        switch (status)
        {
            case "unloading":
                processAmt = Mathf.Min(Quantity - qtyUnloaded, qty);
                qtyUnloaded += processAmt;
                OnStockProcessed("unloaded", processAmt);
                break;
        }
    }
    public override bool CanDoStep(int stockRacked)
    {
        return status != "unloading" || (Quantity-qtyUnloaded) - WorkerCount > 0;
    }
    public override string ForwardText()
    {
        switch(status)
        {
            case "new":
                return "Accept";
            case "accepted":
                return "Accepted";
            case "delivering":
                return "Delivering";
            case "delivered":
                return "Unload";
            case "unloading":
                return "Unloading";
            case "complete":
                return "Complete";
            default:
                return "";
        }
    }
    public override bool WaitingForInput()
    {
        switch (status)
        {
            case "new":
                return true;
            case "delivered":
                return true;
            default:
                return false;
        }
    }
    public override string StepDescription()
    {
        switch (status)
        {
            case "new":
                return "The distributor wants to sell stock to your warehouse";
            case "accepted":
                return "Delivery will depart in the morning";
            case "delivering":
                return "Arrives in <b>" + Mathf.CeilToInt(TimeRemaining() / 2.5f) + "</b> hours";
            case "delivered":
                return "Arrived, waiting to unload";
            case "unloading":
                return "Unloading stock <b>" + QtyUnloaded + "</b> of <b>" + Quantity + "</b>";
            case "complete":
                return "Delivery completed";
            default:
                return "";
        }
    }
    public override float StepProgress()
    {
        switch (status)
        {
            case "delivering":
                return (DeliveryTime - TimeRemaining()) / DeliveryTime;
            case "unloading":
                return (Quantity - QtyUnloaded) / (float)Quantity;
            default:
                return -1;
        }
    }
}