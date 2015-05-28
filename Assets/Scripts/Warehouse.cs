using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Warehouse : MonoBehaviour 
{

    private decimal money = 10000;
    private int workers = 2;
    private decimal wage = 50;
    private int stock = 0;
    private Dictionary<int, Delivery> deliveries = new Dictionary<int, Delivery>();

    public UIManager UIManager;
    public int LastDeliveryID = 0;
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
    public int Stock
    {
        get
        {
            return stock;
        }
        set
        {
            stock = value;
        }
    }

    // 1 hour = 2.5 seconds
    // 1 day = 60 seconds
    public static int PaydayInterval = 14 * 60; // 14 days
    public int NextPayday;
    private int DeliveryInterval;

    public void HireWorker()
    {
        workers += 1;
    }

    public void FireWorker()
    {
        workers -= workers == 0 ? 0 : 1;
    }

    public void AcceptDelivery(int id)
    {
        deliveries[id].Accepted = true;
    }

    public void RemoveItem(int id)
    {
        ScriptableObject.Destroy(deliveries[id]);
        deliveries.Remove(id);
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
                    newDelivery.Init(LastDeliveryID, Random.Range(2, 8) * 5);
                    deliveries[LastDeliveryID] = newDelivery;
                    UIManager.AddPendingItem(newDelivery);
                    DeliveryInterval = Random.Range(1,3) * 60; // Next delivery request in 1-3 days
                }
                int busyWorkers = 0;
                List<int> deliveryKeys = new List<int>(deliveries.Keys);
                foreach (int key in deliveryKeys)
                {
                    Delivery item = deliveries[key];
                    if(item.Accepted && !item.Delivered && item.TimeRemaining() <= 0)
                    {
                        item.Delivered = true;
                    }
                    if(item.Unloading)
                    {
                        int stockUnloaded = Mathf.Min((item.Quantity - item.Unloaded), workers - busyWorkers);
                        stock += stockUnloaded;
                        item.Unloaded += stockUnloaded;
                        busyWorkers = stockUnloaded;

                        if(item.Unloaded >= item.Quantity)
                        {
                            UIManager.RemoveItem(key);
                        }
                    }
                }
                
            }
        }
    }

	void Start ()
    {
        NextPayday = PaydayInterval;
        DeliveryInterval = Random.Range(1, 3) * 60; // 1-3 days
        StartCoroutine(DoTick()); // Begin ticking
	}
	
	void Update ()
    {

	}

    void LateUpdate ()
    {

    }
}