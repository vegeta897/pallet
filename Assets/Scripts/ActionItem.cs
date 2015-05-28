using UnityEngine;
using System.Collections;

public class ActionItem : ScriptableObject
{
    internal string Type;
    internal int Quantity;
    internal int ID;
    public virtual string Status
    {
        get;set;
    }

    public virtual float TimeRemaining()
    {
        return 0;
    }

    public void Init(string type, int id, int qty)
    {
        Type = type;
        Quantity = qty;
        ID = id;
    }
}