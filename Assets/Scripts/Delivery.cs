using UnityEngine;
using System.Collections;

public class Delivery : ActionItem 
{
    private float acceptTime;
    private float deliveryDepartTime;
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
                    acceptTime = Time.time;
                    break;
                case "delivering":
                    DeliveryTime = 60; // 1 day
                    deliveryDepartTime = Time.time;
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
        return deliveryDepartTime + DeliveryTime - Time.time;
    }

    public void Init(int id, int qty)
    {
        Quantity = qty;
        ID = id;
    }
}