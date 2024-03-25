using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static void MeasureTime(Action callback)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        callback?.Invoke();
        stopwatch.Stop();
        UnityEngine.Debug.Log("MeasureTime: " + stopwatch.ElapsedMilliseconds + "ms");
    }

    public static IEnumerator DelayCallback(float duration, Action callback)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke();
    }
}
