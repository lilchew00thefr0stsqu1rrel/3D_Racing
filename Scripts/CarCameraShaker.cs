using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraShaker : CarCameraComponent
{
    [SerializeField] [Range(0.0f, 1.0f)] private float m_NormalizeSpeedShake;      
    [SerializeField] private float m_ShakeAmount;

    // Update is called once per frame
    void Update()
    {
        if (car.NormalizedinearVelocity >= m_NormalizeSpeedShake)
            transform.localPosition += Random.insideUnitSphere * m_ShakeAmount * Time.deltaTime;
    }
}
