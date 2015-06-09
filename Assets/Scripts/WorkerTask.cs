using UnityEngine;
using System.Collections;

public class WorkerTask : ScriptableObject
{
    public ActionItem ActionItem = null;
    public int QtyToRack = 0;

    public float Interval()
    {
        return ActionItem == null ? Utility.GameMinsToRealSecs(5) : ActionItem.QtyTime();
    }
}