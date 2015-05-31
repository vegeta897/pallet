using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BtnWorker : MonoBehaviour 
{
    private Worker worker;

    public Text TxtName;
    public Text TxtStatus;
    public Worker Worker
    {
        get
        {
            return worker;
        }
        set
        {
            worker = value;
            TxtName.text = "Worker " + worker.ID;
        }
    }

	void Start () 
    {

	}
	
	void Update () 
    {
	    
	}
}