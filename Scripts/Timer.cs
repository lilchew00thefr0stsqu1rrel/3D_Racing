using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public event UnityAction Finished;

    [SerializeField] private float time;

    private float value;
    public float Value => value;


    // Start is called before the first frame update
    void Start()
    {
        value = time;
    }

    // Update is called once per frame
    void Update()
    {
        value -= Time.deltaTime;

        if (value <= 0)
        {
            enabled = false;

            Finished?.Invoke();
        }
    }

    public void Restart()
    {
        value = time;
    }
}
