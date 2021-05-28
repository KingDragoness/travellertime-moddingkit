using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObjectActivator : MonoBehaviour
{
    public float timer = 1f;
    public GameObject targetGameobject;
    private float timerCurrent = 0;
    private void OnEnable()
    {
        timerCurrent = timer;
    }

    private void Update()
    {
        timerCurrent -= Time.deltaTime;

        if (timerCurrent < 0)
        {
            targetGameobject.SetActive(true);
        }
    }
}
