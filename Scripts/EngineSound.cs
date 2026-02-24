using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EngineSound : MonoBehaviour
{
    [SerializeField] private Car car;

    [SerializeField] private float m_PitchModifier;
    [SerializeField] private float m_VolumeModifier;
    [SerializeField] private float m_RpmModifier;

    [SerializeField] private float m_BasePitch = 1.0f;
    [SerializeField] private float m_BaseVolume = 0.4f;


    [SerializeField] private AudioSource m_EngineAudioSource;

    private void Start()
    {
        m_EngineAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        m_EngineAudioSource.pitch = m_BasePitch + m_PitchModifier * ((car.EngineRpm / car.EngineMaxRpm) * m_RpmModifier);
        m_EngineAudioSource.volume = m_BaseVolume + m_BaseVolume * ((car.EngineRpm / car.EngineMaxRpm));
    }
}
