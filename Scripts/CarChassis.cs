using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class CarChassis : MonoBehaviour
{
    [SerializeField] private WheelAxle[] m_WheelAxles;
    [SerializeField] private float m_WheelBaseLength;

    [SerializeField] private Transform m_CenterOfMass;

    [Header("Down Force")]
    [SerializeField] private float m_DownForceMin;
    [SerializeField] private float m_DownForceMax;
    [SerializeField] private float m_DownForceFactor;

    [Header("AngularDrag")]
    [SerializeField] private float m_AngularDragMin;
    [SerializeField] private float m_AngularDragMax;
    [SerializeField] private float m_AngularDragFactor;

    // DEBUG
    public float MotorTorque;
    public float BrakeTorque;
    public float SteerAngle;

    public float LinearVelocity => rigidbody.velocity.magnitude * 3.6f;

    private new Rigidbody rigidbody;
    public Rigidbody Rigidbody => rigidbody == null? GetComponent<Rigidbody>() : rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        if (m_CenterOfMass != null)
            rigidbody.centerOfMass = m_CenterOfMass.localPosition;

        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            m_WheelAxles[i].ConfigureVehicleSubsteps(50, 50, 50);
        }
    }

    private void FixedUpdate()
    {
        UpdateAngularDrag();

        UpdateDownForce();


        UpdateWheelAxles();
    }
    public float GetAverageRpm()
    {
        float sum = 0;

        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            sum += m_WheelAxles[i].GetAverageRpm();
        }

        return sum / m_WheelAxles.Length;
    }
    public float GetWheelSpeed()
    {
        return GetAverageRpm() * m_WheelAxles[0].GetRadius() * 2 * 0.1885f;
    }

    private void UpdateAngularDrag()
    {
        rigidbody.angularDrag = Mathf.Clamp(m_AngularDragFactor * LinearVelocity, m_AngularDragMin, m_AngularDragMax);
    }
    private void UpdateDownForce()
    {
        float downForce = Mathf.Clamp(m_DownForceFactor * LinearVelocity, m_DownForceMin, m_DownForceMax);
        rigidbody.AddForce(-transform.up * downForce);
    }
    private void UpdateWheelAxles()
    {
        int amountMotorWheel = 0;

        for (int i = 0; i < m_WheelAxles.Length; i++) 
        {
            if (m_WheelAxles[i].IsMotor == true)
                amountMotorWheel += 2;
            
        };

        for (int i = 0; i < m_WheelAxles.Length; i++)
        {
            m_WheelAxles[i].Update();

            m_WheelAxles[i].ApplyMotorTorque(MotorTorque / amountMotorWheel);
            // i == 0 => +; i == 1 => -
            // Rear wheels - 
            m_WheelAxles[i].ApplySteerAngle(SteerAngle * (Mathf.Pow(-1, i)), m_WheelBaseLength);
            m_WheelAxles[i].ApplyBrakeTorque(BrakeTorque);
        }
    }

    public void Reset()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}
