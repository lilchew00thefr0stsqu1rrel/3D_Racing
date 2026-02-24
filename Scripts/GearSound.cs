using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GearSound : MonoBehaviour
{
   
    private AudioSource m_Source;

    public void Play()
    {
        m_Source.Play();    
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
