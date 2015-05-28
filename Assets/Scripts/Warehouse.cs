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
    private int DeliveryInterval = 5 * 60; // 5 days

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

    public void RejectDelivery(int id)
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
                    Debug.Log("payday!  " + "$" + (workers * 50).ToString("F2"));
                    money -= workers * 50;
                    NextPayday = seconds + PaydayInterval;
                }
                if (seconds % (DeliveryInterval) == 0 || seconds == 1) // Create an order on start
                {
                    Debug.Log("new delivery request!");
                    Delivery newDelivery = ScriptableObject.CreateInstance("Delivery") as Delivery;
                    LastDeliveryID += 1;
                    newDelivery.Init(LastDeliveryID, Random.Range(1, 8) * 5);
                    deliveries[LastDeliveryID] = newDelivery;
                    UIManager.AddPendingItem(newDelivery);
                }
            }
        }
    }

	void Start ()
    {
        NextPayday = PaydayInterval;
        StartCoroutine(DoTick()); // Begin ticking
	}
	
	void Update ()
    {

	}

    void LateUpdate ()
    {

    }
}