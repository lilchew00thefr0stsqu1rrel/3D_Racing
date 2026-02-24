using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelEffects : MonoBehaviour
{
    [SerializeField] private WheelCollider[] m_Wheels;
    [SerializeField] private ParticleSystem[] m_WheelsSmoke;

    [SerializeField] private float m_ForwardSlipLimit;
    [SerializeField] private float m_SidewaysSlipLimit;

    [SerializeField] private new AudioSource m_Audio;

    [SerializeField] private GameObject m_SkidPrefab;

    private WheelHit m_WheelHit;
    private Transform[] m_SkidTrail;

    private void Start()
    {
        m_SkidTrail = new Transform[m_Wheels.Length];
    }

    private void Update()
    {
        bool isSlip = false;

        for (int i = 0; i < m_Wheels.Length; i++)
        {
            m_Wheels[i].GetGroundHit(out m_WheelHit);

            if (m_Wheels[i].isGrounded == true)
            {
                if (m_WheelHit.forwardSlip > m_ForwardSlipLimit || m_WheelHit.sidewaysSlip > m_SidewaysSlipLimit)
                {
                    if (m_SkidTrail[i] == null)
                        m_SkidTrail[i] = Instantiate(m_SkidPrefab).transform;

                    if (m_Audio.isPlaying == false)
                        m_Audio.Play(); 

                    if (m_SkidTrail[i] != null)
                    {
                        m_SkidTrail[i].transform.position = m_Wheels[i].transform.position - m_WheelHit.normal * m_Wheels[i].radius;
                        m_SkidTrail[i].forward = -m_WheelHit.normal;

                        m_WheelsSmoke[i].transform.position = m_SkidTrail[i].position;
                        m_WheelsSmoke[i].Emit(1);
                    }

                    isSlip = true;

                    continue;
                }
            }

            m_SkidTrail[i] = null;
            m_WheelsSmoke[i].Stop();
        }

        if (isSlip == false)
            m_Audio.Stop();
    }
}
