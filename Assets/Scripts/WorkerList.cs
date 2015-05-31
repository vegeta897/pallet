using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorkerList : MonoBehaviour
{
    public BtnWorker PrefabBtnWorker;

    public void AddWorker(Worker newWorker)
    {
        BtnWorker newBtnWorker = Instantiate(PrefabBtnWorker) as BtnWorker;
        newBtnWorker.transform.SetParent(gameObject.transform, false);
        newBtnWorker.Worker = newWorker;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}