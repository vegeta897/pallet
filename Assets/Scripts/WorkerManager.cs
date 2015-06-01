using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkerManager : MonoBehaviour
{
    public Worker PrefabWorker;
    private int workerCount = 0;
    private int lastWorkerID = 0;
    private List<Worker> workers = new List<Worker>();

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
        workers.Add(newWorker);
        return newWorker;
    }

    public void FireWorker(Worker firedWorker)
    {
        workerCount -= 1;
        workers.Remove(firedWorker);
        GameObject.Destroy(firedWorker.gameObject);
    }

    public void AddTask(ActionItem newTask)
    {
        // TODO: New actionable tasks arrive here from Warehouse
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

    }
}