using UnityEngine;
using System.Collections;

public class Delivery : ActionItem 
{
    private float acceptTime;
    private int qtyUnloaded;
    private string status = "new";

    public new string Type = "order";
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
            switch (status)
            {
                case "accepted":
                    DeliveryTime = 60; // 1 day
                    acceptTime = Time.time;
                    break;
                case "delivering":
                    // TODO: Deliver at next available time, store delivery departure time
                    break;
                case "delivered":

                    break;
                case "unloading":

                    break;
            }
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

    public override float TimeRemaining()
    {
        return acceptTime + DeliveryTime - Time.time;
    }

    public void Init(int id, int qty)
    {
        Quantity = qty;
        ID = id;
    }
}