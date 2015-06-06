using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void StockProcessed(string destination, int qty);

public abstract class ActionItem : ScriptableObject
{

    public abstract event StockProcessed OnStockProcessed;

    protected abstract List<string> progression { get; set; }
    public abstract string Type { get; set; }
    internal int Quantity;
    internal int ID;
    internal int WorkerCount = 0;
    public virtual string Status { get; set; }

    protected abstract void BeginStep();
    public abstract float TimeRemaining();
    public abstract bool NeedWorkers(); // If true, the current step needs workers
    public abstract void AutoStep(); // Move to next step if current complete
    public abstract int QtyTime(); // Amount of time it takes to process 1 quantity
    public abstract void ProcessStock(int qty); // Process the specified qty in current step
    public abstract bool CanDoStep(int stockRacked); // Enough stock to pick for an order
    public abstract string ForwardText(); // Text to display on "accept" button for current step
    public abstract bool WaitingForInput(); // If true, user input needed to step forward

    public void StepForward() // Move to the next step in the progression
    {
        Status = progression.IndexOf(Status) < progression.Count - 1 ? progression[progression.IndexOf(Status) + 1] : Status;
        BeginStep();
    }
    public void Init(int id, int qty)
    {
        Quantity = qty;
        ID = id;
    }
}