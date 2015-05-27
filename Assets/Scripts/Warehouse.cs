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
    public int Tick;

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

    // 1 tick = 1 second
    // 1 hour = 2 ticks
    // 1 day = 48 ticks
    public static int PaydayInterval = 14 * 48; // 14 days
    public int NextPayday;
    private int DeliveryInterval = 5 * 48; // 5 days

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
            Tick += 1;
            if (Tick % (PaydayInterval) == 0) // If it is payday
            {
                Debug.Log("payday!  " + "$" + (workers * 50).ToString("F2"));
                money -= workers * 50;
                NextPayday = Tick + PaydayInterval;
            }
            if (Tick % (DeliveryInterval) == 0 || Tick == 1) // Create an order on start
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