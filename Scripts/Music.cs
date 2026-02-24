using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Car car;
    [SerializeField] private float m_MinVolume;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        musicSource.volume = Mathf.Max(m_MinVolume, car.NormalizedinearVelocity);
    }
}
