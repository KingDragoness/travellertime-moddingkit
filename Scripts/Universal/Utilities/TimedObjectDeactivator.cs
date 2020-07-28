using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObjectDeactivator : MonoBehaviour
{
    public float timer = 1f;
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
            gameObject.SetActive(false);
        }
    }
}
