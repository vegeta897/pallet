using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Order : ActionItem
{
    public override event StockProcessed OnStockProcessed;

    public Order()
    {
        progression = new List<string>() 
        {
            "new",
            "accepted",
            "picking",
            "picked",
            "loading",
            "loaded",
            "shipping",
            "complete"
        };
        status = progression[0];
        Type = "order";
    }

    protected override List<string> progression { get; set; }
    private float acceptTime;
    private int qtyPicked;
    private int qtyLoaded;
    private float shipTime;
    private string status;

    public override string Type { get; set; }
    public int ShippingTime;
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
    public int QtyPicked
    {
        get
        {
            return qtyPicked;
        }
        set
        {
            qtyPicked = Mathf.Min(value, Quantity);
        }
    }
    public int QtyLoaded
    {
        get
        {
            return qtyLoaded;
        }
        set
        {
            qtyLoaded = Mathf.Min(value,Quantity);
        }
    }

    protected override void BeginStep()
    {
        switch (status)
        {
            case "accepted":
                acceptTime = Time.time;
                break;
            case "picking":

                break;
            case "picked":

                break;
            case "loading":

                break;
            case "loaded":

                break;
            case "shipping":
                shipTime = Time.time;
                ShippingTime = 60; // 1 day
                break;
        }
    }

    public override float TimeRemaining()
    {
        return shipTime + ShippingTime - Time.time;
    }
    public override bool NeedWorkers()
    {
        return status == "picking" || status == "loading";
    }
    public override bool StepComplete()
    {
        bool complete = false;
        switch (status)
        {
            case "picking":
                complete = qtyPicked == Quantity;
                break;
            case "loading":
                complete = qtyLoaded == Quantity;
                break;
            case "shipping":
                complete = TimeRemaining() <= 0;
                break;
        }
        return complete;
    }
    public override int QtyTime()
    {
        return status == "picking" ? 2 : 1;
    }
    public override void ProcessStock(int qty)
    {
        int processAmt;
        switch (status)
        {
            case "picking":
                processAmt = Mathf.Min(Quantity - qtyPicked, qty);
                qtyPicked += processAmt;
                OnStockProcessed("picked", processAmt);
                break;
            case "loading":
                processAmt = Mathf.Min(qtyPicked - qtyLoaded, qty);
                qtyLoaded += processAmt;
                OnStockProcessed("loaded", processAmt);
                break;
        }
    }
    public override bool CanDoStep(int stockRacked)
    {
        return status != "picking" || stockRacked - WorkerCount > 0;
    }
}