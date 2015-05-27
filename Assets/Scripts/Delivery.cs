using UnityEngine;
using System.Collections;

public class Delivery : ScriptableObject 
{
    public int Quantity;
    public int DeliveryID;
    public bool Accepted;

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
