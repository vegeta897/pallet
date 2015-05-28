using UnityEngine;
using System.Collections;

public class Delivery : ScriptableObject 
{
    private bool accepted;
    private float acceptTime;
    private bool delivered;
    private bool unloading;
    private int unloaded;
    private bool complete;

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
            DeliveryTime = 60; // 1 day
            acceptTime = Time.time;
        }
    }
    public bool Delivered
    {
        get
        {
            return delivered;
        }
        set
        {
            delivered = value;
        }
    }
    public bool Unloading
    {
        get
        {
            return unloading;
        }
        set
        {
            unloading = value;
        }
    }
    public int Unloaded
    {
        get
        {
            return unloaded;
        }
        set
        {
            unloaded = value;
            if(unloaded >= Quantity)
            {
                ScriptableObject.Destroy(this);
            }
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
