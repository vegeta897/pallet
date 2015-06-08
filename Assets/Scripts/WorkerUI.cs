using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class WorkerUI : MonoBehaviour
{
    public WorkerManager WorkerManager;
    public WorkerForm WorkerForm;
    public Warehouse Warehouse;
    public GameObject PanWorkerList;
    public BtnWorker PrefabBtnWorker;
    public Text TxtWorkers;
    public Text TxtUntilPayday;
    public Text TxtPaydayAmount;
    public Button BtnFire;
    public InputField InpWage;
    public Button BtnSetWage;

    private Worker selectedWorker;

    private Dictionary<Worker, BtnWorker> workers = new Dictionary<Worker, BtnWorker>();

    public void AddWorker(Worker newWorker)
    {
        BtnWorker newBtnWorker = Instantiate(PrefabBtnWorker) as BtnWorker;
        newBtnWorker.transform.SetParent(PanWorkerList.transform, false);
        newBtnWorker.Worker = newWorker;
        newBtnWorker.OnWorkerSelected += WorkerSelected;
        workers.Add(newWorker, newBtnWorker);
        WorkerForm.HideHireForm();
    }

    public void RemoveWorker(Worker removedWorker)
    {
        GameObject.Destroy(workers[removedWorker].gameObject);
        workers.Remove(removedWorker);
        WorkerSelected(null);
    }
    public void HireWorker()
    {
        Worker newWorker = WorkerManager.HireWorker((decimal)Mathf.Max(7.5f, Mathf.Round(float.Parse(WorkerForm.InpWage.text) * 100) / 100));
        AddWorker(newWorker);
    }

    public void FireWorker()
    {
        WorkerManager.FireWorker(selectedWorker);
        RemoveWorker(selectedWorker);
    }

    private void WorkerSelected(Worker worker)
    {
        foreach (KeyValuePair<Worker, BtnWorker> btn in workers)
        {
            btn.Value.GetComponent<Button>().Highlight(false);
        }
        selectedWorker = worker;
        if(selectedWorker != null)
        {
            workers[selectedWorker].GetComponent<Button>().Highlight();
        }
        BtnFire.interactable = worker != null;
        BtnSetWage.interactable = worker != null;
        InpWage.interactable = worker != null;
        InpWage.text = worker != null ? worker.Wage.ToString("F2") : InpWage.text;
    }

    public void SetWage()
    {
        selectedWorker.Wage = (decimal)Mathf.Max(7.5f, Mathf.Round(float.Parse(InpWage.text) * 100) / 100);
        InpWage.text = selectedWorker.Wage.ToString("F2");
    }
    public void Click(BaseEventData data)
    {
        WorkerSelected(null);
    }

    void Start()
    {
        foreach (Transform child in PanWorkerList.transform) // Remove editor placeholder
        {
            GameObject.Destroy(child.gameObject);
        }
        InpWage.textComponent.alignment = TextAnchor.MiddleRight;
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        TxtWorkers.text = "<b>" + WorkerManager.WorkerCount + "</b> worker" + (WorkerManager.WorkerCount == 1 ? "" : "s");
        TxtUntilPayday.text = Mathf.CeilToInt((float)(Warehouse.NextPayday - Utility.GetTime()) / 64f) + " days";
        TxtPaydayAmount.text = "$" + WorkerManager.PaydayAmount().ToString("N0");
    }
}