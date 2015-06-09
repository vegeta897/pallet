using UnityEngine;
using System.Collections;

public class Worker : MonoBehaviour
{
    public event StockProcessed OnStockProcessed;

    public int ID;
    public WorkerTask Task;
    public decimal Wage;
    public int StartTime;
    public int EndTime;

    private float fatigue = 0;

    public bool Busy()
    {
        return Task.ActionItem != null || Task.QtyToRack > 0;
    }
    public string Status()
    {
        if (Task.ActionItem != null)
        {
            return Task.ActionItem.Status.Substring(0, 1).ToUpper() + Task.ActionItem.Status.Substring(1, Task.ActionItem.Status.Length - 1);
        }
        else if (Task.QtyToRack > 0)
        {
            return "Racking";
        }
        else
        {
            return OnTheClock() ? "Idle" : "Off Work";
        }
    }
    public bool OnTheClock()
    {
        if(StartTime < EndTime)
        {
            return Busy() || (Utility.Hour() >= StartTime && Utility.Hour() < EndTime);
        }
        else
        {
            return Busy() || (Utility.Hour() >= StartTime || Utility.Hour() < EndTime);
        }
    }
    IEnumerator TaskWork()
    {
        while (true)
        {
            while (OnTheClock())
            {
                while (Busy())
                {
                    yield return new WaitForSeconds(Task.Interval() * Random.Range(0.8f, 1.2f) + fatigue);

                    if (Task.ActionItem != null)
                    {
                        Task.ActionItem.ProcessStock(1);
                        fatigue += 0.01f;
                        Task.ActionItem.WorkerCount -= 1;
                    }
                    else
                    {
                        Task.QtyToRack = Mathf.Max(Task.QtyToRack - 1, 0);
                        OnStockProcessed("racked", 1);
                        fatigue += 0.01f;
                    }
                    Task.ActionItem = null;
                    Task.QtyToRack = 0;
                }
                fatigue = Mathf.Max(0, fatigue - 0.002f);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.4f);
        }
    }
    public decimal DailyWage()
    {
        return ((EndTime + (StartTime < EndTime ? 0 : 24)) - StartTime) * Wage;
    }

    void Awake()
    {
        Task = ScriptableObject.CreateInstance("WorkerTask") as WorkerTask;
    }

    void Update()
    {
        Task.ActionItem = Task.ActionItem != null && Task.ActionItem.NeedWorkers() ? Task.ActionItem : null;
    }

    public void Init(int id, decimal wage, int startTime, int endTime)
    {
        ID = id;
        Wage = wage;
        StartTime = startTime;
        EndTime = endTime;
        StartCoroutine(TaskWork());
    }
}