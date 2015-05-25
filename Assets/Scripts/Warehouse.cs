using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

    private float nextPayday = 10f; 
    private float paydayPeriod = 10f;

    public Text TxtMoney;
    public Text TxtWorkers;
    public Text TxtUntilPayday;
    public Text TxtPaydayAmount;

    public void HireWorker ()
    {
        workers += 1;
    }

    public void FireWorker()
    {
        workers -= workers == 0 ? 0 : 1;
    }

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Time.time > nextPayday) 
         {
             nextPayday = Time.time + paydayPeriod;
             money -= workers * 50;
         }
	}

    void LateUpdate ()
    {
        TxtMoney.text = "$" + money;
        TxtWorkers.text = "Workers: " + workers;
        TxtUntilPayday.text = "Next Payday: " + Mathf.CeilToInt(nextPayday-Time.time);
        TxtPaydayAmount.text = "Payday Cost: " + workers * wage;
    }
}
