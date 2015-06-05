using UnityEngine;
using System.Collections;

public static class Utility : object
{
    public static int Hour() // Returns 0-23 on 24-hour clock, +7 hour offset
    {
        return (Mathf.FloorToInt(Time.time / 2.5f) + 7) % 24;
    }
}
