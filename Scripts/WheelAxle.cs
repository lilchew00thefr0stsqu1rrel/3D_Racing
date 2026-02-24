using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WheelAxle
{
    [SerializeField] private WheelCollider m_LeftWheelCollider;
    [SerializeField] private WheelCollider m_RightWheelCollider;

    [SerializeField] private Transform m_LeftWheelMesh;
    [SerializeField] private Transform m_RightWheelMesh;

    [SerializeField] private bool m_IsMotor;
    [SerializeField] private bool m_IsSteer;


    [SerializeField] private float m_WheelWidth;

    [SerializeField] private float m_AntiRollForce;

    [SerializeField] private float m_AdditionalWheelDownForce;

    [SerializeField] private float m_BaseForwardStiffness = 1.5f;
    [SerializeField] private float m_StabilityForwardFactor = 1.0f;


    [SerializeField] private float m_BaseSidewaysStiffness = 1.5f;
    [SerializeField] private float m_StabilitySidewaysFactor = 1.0f;


    private WheelHit m_LeftWheelHit;
    private WheelHit m_RightWheelHit;

    public bool IsMotor => m_IsMotor;
    public bool IsSteer => m_IsSteer;

    // Public API.
    public void Update()
    {
        UpdateWheelHits();

        ApplyAntiRoll();
        ApplyDownForce();
        CorrectStiffness();
        
        SyncMeshTransform();

        // TODO
    }

    public void ConfigureVehicleSubsteps(float speedThreshold, int speedBelowThreshold, int stepsAboveThreshold)
    {
        m_LeftWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveThreshold);
        m_RightWheelCollider.ConfigureVehicleSubsteps(speedThreshold, speedBelowThreshold, stepsAboveThreshold);
    }

    private void UpdateWheelHits()
    {
        m_LeftWheelCollider.GetGroundHit(out m_LeftWheelHit);
        m_RightWheelCollider.GetGroundHit(out m_RightWheelHit);
    }

    private void CorrectStiffness()
    {
        WheelFrictionCurve leftForward = m_LeftWheelCollider.forwardFriction;
        WheelFrictionCurve rightForward = m_RightWheelCollider.forwardFriction;

        WheelFrictionCurve leftSideways = m_LeftWheelCollider.sidewaysFriction;
        WheelFrictionCurve rightSideways = m_RightWheelCollider.sidewaysFriction;

        leftForward.stiffness = m_BaseForwardStiffness + Mathf.Abs(m_LeftWheelHit.forwardSlip) * m_StabilityForwardFactor;
        rightForward.stiffness = m_BaseForwardStiffness + Mathf.Abs(m_RightWheelHit.forwardSlip) * m_StabilityForwardFactor;


        leftSideways.stiffness = m_BaseSidewaysStiffness + Mathf.Abs(m_LeftWheelHit.sidewaysSlip) * m_StabilitySidewaysFactor;
        rightSideways.stiffness = m_BaseSidewaysStiffness + Mathf.Abs(m_RightWheelHit.sidewaysSlip) * m_StabilitySidewaysFactor;
    }

    private void ApplyDownForce()
    {
        if (m_LeftWheelCollider.isGrounded == true)
            m_LeftWheelCollider.attachedRigidbody.AddForceAtPosition(m_LeftWheelHit.normal
                * -m_AdditionalWheelDownForce * m_LeftWheelCollider.attachedRigidbody.velocity.magnitude,
                m_LeftWheelCollider.transform.position);

        if (m_RightWheelCollider.isGrounded == true)
            m_RightWheelCollider.attachedRigidbody.AddForceAtPosition(m_RightWheelHit.normal
                * -m_AdditionalWheelDownForce * m_RightWheelCollider.attachedRigidbody.velocity.magnitude,
                m_RightWheelCollider.transform.position);
        
    }

    private void ApplyAntiRoll()
    {
        float travelL = 1.0f;
        float travelR = 1.0f;

        if (m_LeftWheelCollider.isGrounded == true)
        {
            travelL = (-m_LeftWheelCollider.transform.InverseTransformPoint(m_LeftWheelHit.point).y 
                - m_LeftWheelCollider.radius) / m_LeftWheelCollider.suspensionDistance;
        }
        if (m_RightWheelCollider.isGrounded == true)
        {
            travelR = (-m_RightWheelCollider.transform.InverseTransformPoint(m_RightWheelHit.point).y
                - m_RightWheelCollider.radius) / m_RightWheelCollider.suspensionDistance;
        }

        float forceDir = (travelL - travelR);

        if (m_LeftWheelCollider.isGrounded)
        {
            m_LeftWheelCollider.attachedRigidbody.AddForceAtPosition(m_LeftWheelCollider.transform.up * -forceDir * m_AntiRollForce,
                m_LeftWheelCollider.transform.position);
        }
        if (m_RightWheelCollider.isGrounded)
        {
            m_RightWheelCollider.attachedRigidbody.AddForceAtPosition(m_RightWheelCollider.transform.up * forceDir * m_AntiRollForce,
                m_RightWheelCollider.transform.position);
        }
    }

    public void ApplySteerAngle(float steerAngle, float wheelBaseLength)
    {
        if (m_IsSteer == false) { return; }

        float radius = Mathf.Abs(wheelBaseLength * Mathf.Tan(Mathf.Deg2Rad * (90 - Mathf.Abs(steerAngle))));
        float angleSing = Mathf.Sign(steerAngle);

        if (steerAngle > 0)
        {
            m_LeftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (radius + (m_WheelWidth * 0.5f))) * angleSing;
            m_RightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (radius - (m_WheelWidth * 0.5f))) * angleSing;
        }

        else if (steerAngle < 0)
        {
            m_LeftWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (radius - (m_WheelWidth * 0.5f))) * angleSing;
            m_RightWheelCollider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBaseLength / (radius + (m_WheelWidth * 0.5f))) * angleSing;
        }

        else
        {
            m_LeftWheelCollider.steerAngle = 0;
            m_RightWheelCollider.steerAngle = 0;
        }

    }

    public void ApplyMotorTorque(float motorTorque)
    {
        if (m_IsMotor == false) { return; }

        m_LeftWheelCollider.motorTorque = motorTorque;
        m_RightWheelCollider.motorTorque = motorTorque;

    }
    public void ApplyBrakeTorque(float brakeTorque)
    {
        m_LeftWheelCollider.brakeTorque = brakeTorque;
        m_RightWheelCollider.brakeTorque = brakeTorque;

    }

    public float GetAverageRpm()
    {
        return (m_LeftWheelCollider.rpm +  m_RightWheelCollider.rpm) * 0.5f;
    }

    public float GetRadius()
    {
        return m_LeftWheelCollider.radius;
    }

    // Private.
    private void SyncMeshTransform()
    {
        UpdateWheelTransform(m_LeftWheelCollider, m_LeftWheelMesh);
        UpdateWheelTransform(m_RightWheelCollider, m_RightWheelMesh);
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);

        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}
