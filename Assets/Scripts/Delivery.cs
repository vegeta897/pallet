using UnityEngine;
using System.Collections;

public class Delivery : ScriptableObject 
{
    private bool accepted;
    private float acceptTime;

    public int Quantity;
    public int DeliveryID;
    public int DeliveryTime;
    public bool Accepted
    {
        get
        {
            return accepted;
        }
        set
        {
            accepted = value;
            DeliveryTime = 180; // 3 days
            acceptTime = Time.time;
        }
    }

    public float TimeRemaining()
    {
        return acceptTime + DeliveryTime - Time.time;
    }

    public void Init(int id, int qty)
    {
        Quantity = qty;
        DeliveryID = id;
    }

	void Start () 
    {
	    
	}
	
	void Update () 
    {
	    
	}
}
