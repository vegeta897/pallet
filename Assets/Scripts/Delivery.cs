using UnityEngine;
using System.Collections;

public class Delivery : ScriptableObject 
{
    private bool accepted;

    public int Quantity;
    public int DeliveryID;
    public float DeliveryTime;
    public bool Accepted
    {
        get
        {
            return accepted;
        }
        set
        {
            accepted = value;
            DeliveryTime = Time.time + 30f;
        }
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
