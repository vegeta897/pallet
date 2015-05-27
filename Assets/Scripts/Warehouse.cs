using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Warehouse : MonoBehaviour 
{

    private decimal money = 10000;
    private int workers = 2;
    private decimal wage = 50;
    private int stock = 0;

    private List<Delivery> deliveries = new List<Delivery>();

    public UIManager UIManager;

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

    public float PaydayInterval = 10f;
    public float NextPayday;
    private float DeliveryInterval = 5f;

    public void HireWorker()
    {
        workers += 1;
    }

    public void FireWorker()
    {
        workers -= workers == 0 ? 0 : 1;
    }

    IEnumerator PayDay(float interval)
    {
        while(true)
        {
            yield return new WaitForSeconds(interval);
            Debug.Log("payday!");
            money -= workers * 50;
            NextPayday = Time.time + PaydayInterval;
        }
    }

    IEnumerator NewDelivery(float interval)
    {
        while(true)
        {
            yield return new WaitForSeconds(interval);
            Debug.Log("new delivery request!");
            Delivery newDelivery = ScriptableObject.CreateInstance("Delivery") as Delivery;
            newDelivery.Init(deliveries.Count, 25);
            deliveries.Add(newDelivery);
            UIManager.AddActionItem(newDelivery);
        }
    }

	void Start ()
    {
        NextPayday = Time.time + PaydayInterval;
        StartCoroutine(PayDay(PaydayInterval)); // Begin coroutines
        StartCoroutine(NewDelivery(DeliveryInterval));
	}
	
	void Update ()
    {

	}

    void LateUpdate ()
    {

    }
}