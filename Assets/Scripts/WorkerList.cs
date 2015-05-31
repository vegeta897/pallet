using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WorkerList : MonoBehaviour
{
    public BtnWorker PrefabBtnWorker;

    private Dictionary<Worker, BtnWorker> workers = new Dictionary<Worker, BtnWorker>();

    public BtnWorker AddWorker(Worker newWorker)
    {
        BtnWorker newBtnWorker = Instantiate(PrefabBtnWorker) as BtnWorker;
        newBtnWorker.transform.SetParent(gameObject.transform, false);
        newBtnWorker.Worker = newWorker;
        return newBtnWorker;
    }

    public void RemoveWorker(Worker removedWorker)
    {
        GameObject.Destroy(workers[removedWorker].gameObject);
        workers.Remove(removedWorker);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}