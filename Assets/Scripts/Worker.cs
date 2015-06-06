using UnityEngine;
using System.Collections;

public class Worker : MonoBehaviour
{
    public event StockProcessed OnStockProcessed;

    public int ID;
    public WorkerTask Task;

    private float fatigue = 0;

    public bool Busy()
    {
        return Task.ActionItem != null || Task.QtyToRack > 0;
    }

    public string Status()
    {
        if (Task.ActionItem != null)
        {
            return Task.ActionItem.Status.Substring(0, 1).ToUpper() + Task.ActionItem.Status.Substring(1, Task.ActionItem.Status.Length - 1) + " Stock";
        }
        else if (Task.QtyToRack > 0)
        {
            return "Racking Stock";
        }
        else
        {
            return "Idle";
        }
    }

    IEnumerator TaskWork()
    {
        while (true)
        {
            while (Busy())
            {
                yield return new WaitForSeconds(Task.Interval() * Random.Range(0.8f, 1.2f) + fatigue);

                if (Task.ActionItem != null)
                {
                    Task.ActionItem.ProcessStock(1);
                    fatigue += 0.05f;
                    Task.ActionItem.WorkerCount -= 1;
                }
                else
                {
                    Task.QtyToRack = Mathf.Max(Task.QtyToRack - 1, 0);
                    OnStockProcessed("racked", 1);
                    fatigue += 0.05f;
                }
                Task.ActionItem = null;
                Task.QtyToRack = 0;
            }
            fatigue = Mathf.Max(0, fatigue - 0.002f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Awake()
    {
        Task = ScriptableObject.CreateInstance("WorkerTask") as WorkerTask;
    }

    void Update()
    {
        Task.ActionItem = Task.ActionItem != null && Task.ActionItem.NeedWorkers() ? Task.ActionItem : null;
    }

    public void Init(int id)
    {
        ID = id;
        StartCoroutine(TaskWork());
    }
}