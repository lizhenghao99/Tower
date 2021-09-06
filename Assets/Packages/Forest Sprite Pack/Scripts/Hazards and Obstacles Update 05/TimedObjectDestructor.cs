using System;
using UnityEngine;


public class TimedObjectDestructor : MonoBehaviour
{
    public enum WhenToCountdown {onTriggering, onWake};
    public WhenToCountdown whenToCountdown;

    [Tooltip("This is only used if the countdown happens on wake")]
    public float onWakeTimeOut;

    private void Start()
    {
        if (whenToCountdown== WhenToCountdown.onWake)
            Invoke("DestroyNow", onWakeTimeOut);
    }

    public void Destroy(float timeOut)
    {
        if (whenToCountdown == WhenToCountdown.onTriggering)
            Invoke("DestroyNow", timeOut);
        else
            Debug.LogWarning("whenToCountdown is set to OnWake. Calling this method only works if set to onTriggering");
    }

    private void DestroyNow()
    {
        DestroyObject(gameObject);
    }
}

