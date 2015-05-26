using UnityEngine;
using System.Collections;

public class Delivery : ScriptableObject 
{
    public int Quantity;
    public int Index;

    public void Init(int index, int qty)
    {
        Quantity = qty;
        Index = index;
    }

	void Start () 
    {
	    
	}
	
	void Update () 
    {
	    
	}
}
