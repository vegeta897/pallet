using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public delegate void WorkerSelected(Worker selectedWorker);

public class BtnWorker : MonoBehaviour, IPointerClickHandler
{
    public event WorkerSelected OnWorkerSelected;

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

    public void OnPointerClick(PointerEventData data)
    {
        OnWorkerSelected(worker);
    }

	void Start ()
    {

	}
	
	void Update () 
    {
	    
	}
}