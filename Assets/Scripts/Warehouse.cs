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

    public float NextPayday = 10f; 
    private float paydayPeriod = 10f;

    private float nextDelivery = 5f;
    private float deliveryPeriod = 30f;

    public void HireWorker ()
    {
        workers += 1;
    }

    public void FireWorker()
    {
        workers -= workers == 0 ? 0 : 1;
    }

	void Start () 
    {

	}
	
	void Update () 
    {
        if (Time.time > NextPayday) 
         {
             NextPayday = Time.time + paydayPeriod;
             money -= workers * 50;
         }
        if (Time.time > nextDelivery)
        {
            nextDelivery = Time.time + deliveryPeriod;
            Delivery newDelivery = ScriptableObject.CreateInstance("Delivery") as Delivery;
            deliveries.Add(newDelivery);
            UIManager.AddActionItem(newDelivery,deliveries.Count);
        }
	}

    void LateUpdate ()
    {

    }
}