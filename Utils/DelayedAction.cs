using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAction
{

    

    public static void InvokeDelayed(Action action, float delay)
    {
        GameLogic.instance.StartCoroutine(delayCoroutine(action, delay));
    }

    public static void StopAllCoroutine()
    {
        GameLogic.instance.StopAllCoroutines();
    }

    private static IEnumerator delayCoroutine(Action action, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        action();
    }

}
