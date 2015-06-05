using UnityEngine;
using System.Collections;

public class WorkerTask : ScriptableObject
{
    public ActionItem ActionItem = null;
    public int QtyToRack = 0;

    public float Interval()
    {
        return ActionItem == null ? 2 : ActionItem.QtyTime();
    }
}