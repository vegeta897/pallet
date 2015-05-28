using UnityEngine;
using System.Collections;

public class Order : ActionItem
{
    private float acceptTime;
    private int qtyPicked;
    private int qtyLoaded;
    private float shipTime;
    private string status = "new";

    public new string Type = "order";
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
            switch(status)
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

    public override float TimeRemaining()
    {
        return shipTime + ShippingTime - Time.time;
    }
}
