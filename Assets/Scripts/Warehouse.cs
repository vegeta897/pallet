using UnityEngine;
using System.Collections;

public class Warehouse : MonoBehaviour 
{

    private decimal money = 1000;
    private int workers = 2;
    private decimal wage = 50;

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

    public float NextPayday = 10f; 
    private float paydayPeriod = 10f;

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
	}

    void LateUpdate ()
    {

    }
}