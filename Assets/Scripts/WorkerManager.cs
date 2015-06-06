using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkerManager : MonoBehaviour
{
    public Warehouse Warehouse;
    public Worker PrefabWorker;
    private int workerCount = 0;
    private int lastWorkerID = 0;
    private List<Worker> workers = new List<Worker>();
    private List<ActionItem> actionItems;

    public int StockRacking = 0;
    public int WorkerCount
    {
        get
        {
            return workerCount;
        }
        set
        {
            workerCount = value;
        }
    }

    public Worker HireWorker()
    {
        workerCount += 1;
        lastWorkerID += 1;
        Worker newWorker = Instantiate(PrefabWorker) as Worker;
        newWorker.Init(lastWorkerID);
        newWorker.transform.SetParent(gameObject.transform, false);
        newWorker.OnStockProcessed += Warehouse.OnStockProcessed;
        workers.Add(newWorker);
        return newWorker;
    }

    public void FireWorker(Worker firedWorker)
    {
        workerCount -= 1;
        workers.Remove(firedWorker);
        GameObject.Destroy(firedWorker.gameObject);
    }

    IEnumerator HandleTasks()
    {
        while (true)
        {


            yield return new WaitForSeconds(0.1f);
        }
    }

    void Start()
    {
        foreach (Transform child in gameObject.transform) // Remove editor placeholder
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void Update()
    {
        if (actionItems != null)
        {
            for (int i = actionItems.Count - 1; i >= 0; i--) // Process current deliveries and orders
            {
                actionItems[i].AutoStep(); // If step complete, move to next
                if (actionItems[i].NeedWorkers()) // If task needs workers
                {
                    if (!actionItems[i].CanDoStep(Warehouse.StockRacked))
                    {
                        continue;
                    }
                    for (int w = 0; w < workers.Count; w++)
                    {
                        if (!workers[w].Busy()) // If worker not busy
                        {
                            workers[w].Task.ActionItem = actionItems[i]; // Assign task to this worker
                            actionItems[i].WorkerCount += 1;
                        }
                    }
                }
                if (actionItems[i].Status == "complete")
                {
                    Warehouse.RemoveActionItem(actionItems[i]);
                }
            }
            for (int w = 0; w < workers.Count; w++)
            {
                if (Warehouse.StockUnloaded - StockRacking > 0) // If there is stock waiting to be racked
                {
                    if (!workers[w].Busy()) // If worker not busy
                    {
                        workers[w].Task.QtyToRack = 1; // Tell worker to rack
                        StockRacking += 1;
                    }
                }
            }
        }
    }

    public void Init(List<ActionItem> ai)
    {
        actionItems = ai;
        StartCoroutine(HandleTasks());
    }
}