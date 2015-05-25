using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionItem : MonoBehaviour 
{
    public Text TxtQuantity;

    public void SetQuantity (int qty)
    {
        TxtQuantity.text = qty.ToString();
    }

	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}
}
