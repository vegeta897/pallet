using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utility : object
{
    private static List<string> WeekDays = new List<string>
    {
        "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"
    };
    public static int Hour() // Returns 0-23 on 24-hour clock, +7 hour offset
    {
        return (Mathf.FloorToInt(GetTime() / 2.5f) + 7) % 24;
    }
    public static string WeekDay()
    {
        return WeekDays[Mathf.FloorToInt(((GetTime() / 2.5f) + 7) / 24) % 7];
    }
    public static float GetTime()
    {
        return Time.timeSinceLevelLoad;
    }
    public static float GameMinsToRealSecs(float m)
    {
        return m * 0.0416667f; // (Game minutes / 60) * 2.5
    }
}
