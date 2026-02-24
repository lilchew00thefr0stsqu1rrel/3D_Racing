using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputControl : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private AnimationCurve m_BrakeCurve;
    [SerializeField] private AnimationCurve m_SteerCurve;

    [SerializeField][Range(0.0f, 1.0f)] private float m_AutoBrakeStrength = 0.5f;

    private float m_WheelSpeed;
    private float m_VerticalAxis;
    private float m_HorizontalAxis;
    private float m_HandBrakeAxis;


    private void Update()
    {
        m_WheelSpeed = car.WheelSpeed;

        UpdateAxis();

        UpdateThrottleAndBrake();
        UpdateSteer();


        UpdateAutoBrake();

        // Debug
        if (Input.GetKeyDown(KeyCode.E))
            car.UpGear(); 
        
        if (Input.GetKeyDown(KeyCode.Q))
            car.DownGear();


    }

    private void UpdateSteer()
    {
        car.SteerControl = m_SteerCurve.Evaluate(car.WheelSpeed / car.MaxSpeed) * m_HorizontalAxis;
    }



    private void UpdateThrottleAndBrake()
    {
        if (Mathf.Sign(m_VerticalAxis) == Mathf.Sign(m_WheelSpeed) || Mathf.Abs(m_WheelSpeed) < 0.5f)
        {
            car.ThrottleControl = Mathf.Abs(m_VerticalAxis);
            car.BrakeControl = 0;
        }
        else
        {
            car.ThrottleControl = 0;
            car.BrakeControl = m_BrakeCurve.Evaluate(m_WheelSpeed / car.MaxSpeed);
        }

        // Gears
        if (m_VerticalAxis < 0 && m_WheelSpeed > -0.5f && m_WheelSpeed <= 0.5f)
        {
            car.ShiftToReverseGear();
        }
        if (m_VerticalAxis > 0 && m_WheelSpeed > -0.5f && m_WheelSpeed < 0.5f)
        {
            car.ShiftToFirstGear();
        }
    }
    private void UpdateAutoBrake()
    {
        if (Input.GetAxis("Vertical") == 0)
        {
            car.BrakeControl = m_BrakeCurve.Evaluate(car.WheelSpeed / car.MaxSpeed) * m_AutoBrakeStrength;
        }
    }
    private void UpdateAxis()
    {
        m_VerticalAxis = Input.GetAxis("Vertical");
        m_HorizontalAxis = Input.GetAxis("Horizontal");
        m_HandBrakeAxis = Input.GetAxis("Jump");
    }
    public void Reset()
    {
        m_VerticalAxis = 0;
        m_HorizontalAxis = 0;
        m_HandBrakeAxis = 0;

        car.ThrottleControl = 0;
        car.SteerControl = 0;
        car.BrakeControl = 0;
    }

    public void Stop()
    {
        Reset();

        car.BrakeControl = 1;
    }

  
}
