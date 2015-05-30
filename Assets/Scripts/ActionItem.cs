using UnityEngine;
using System.Collections;

public abstract class ActionItem : ScriptableObject
{
    internal string Type;
    internal int Quantity;
    internal int ID;
    public virtual string Status
    {
        get;set;
    }

    public abstract float TimeRemaining();

    public void Init(string type, int id, int qty)
    {
        Type = type;
        Quantity = qty;
        ID = id;
    }
}