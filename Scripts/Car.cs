using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CarChassis))]
public class Car : MonoBehaviour
{

    [SerializeField] private float m_MaxSteerAngle;
    [SerializeField] private float m_MaxBrakeTorque;


    [Header("Engine")]
    [SerializeField] private AnimationCurve m_EngineTorqueCurve;
    [SerializeField] private float m_EngineMaxTorque;
    // DEBUG
    [SerializeField] private float m_EngineTorque;
    // DEBUG
    [SerializeField] private float m_EngineRpm;
    public float EngineRpm => m_EngineRpm;
    [SerializeField] private float m_EngineMinRpm;
    [SerializeField] private float m_EngineMaxRpm;
    public float EngineMaxRpm => m_EngineMaxRpm;    
    [Header("Gearbox")]
    [SerializeField] private float[] m_Gears;
    [SerializeField] private float m_FinalDriveRatio;
    // DEBUG
    [SerializeField] private int m_SelectedGearIndex;
    // DEBUG
    [SerializeField] private float m_SelectedGear;
    [SerializeField] private float m_RearGear;
    [SerializeField] private float m_UpShiftEngineRpm;
    [SerializeField] private float m_DownShiftEngineRpm;

    [SerializeField] private int m_MaxSpeed;

    [Header("UI")]
    [SerializeField] private Text m_SpeedText;
    [SerializeField] private Image m_RpmImage;
    [SerializeField] private Text m_GearText;

    [SerializeField] private GearSound m_GearSound;

    public float LinearVelocity => m_Chassis.LinearVelocity;
    public float NormalizedinearVelocity => m_Chassis.LinearVelocity / m_MaxSpeed;
    public float WheelSpeed => m_Chassis.GetWheelSpeed();
    public float MaxSpeed => m_MaxSpeed;

    private CarChassis m_Chassis;
    public Rigidbody Rigidbody => m_Chassis == null? GetComponent<CarChassis>().Rigidbody : m_Chassis.Rigidbody;
    // DEBUG
    [SerializeField] private float linearVelocity;



    public float ThrottleControl;
    public float SteerControl;
    public float BrakeControl;

    private void Start()
    {
        m_Chassis = GetComponent<CarChassis>();
    }

    private void Update()
    {
        linearVelocity = LinearVelocity;

        // UI
        m_SpeedText.text = ((int)LinearVelocity).ToString();   

        UpdateEngineTorque();

        AutoGearShift();

        if (LinearVelocity >= m_MaxSpeed)
            m_EngineTorque = 0;

        m_Chassis.MotorTorque = m_EngineTorque * ThrottleControl;
        m_Chassis.SteerAngle = m_MaxSteerAngle * SteerControl;
        m_Chassis.BrakeTorque = m_MaxBrakeTorque * BrakeControl;
    }

    // GearBox

    private void AutoGearShift()
    {
        if (m_SelectedGear < 0) return;

        if (m_EngineRpm >= m_UpShiftEngineRpm)
            UpGear();
        if (m_EngineRpm <= m_DownShiftEngineRpm)
            DownGear();

    }
    public void UpGear()
    {
        ShiftGear(m_SelectedGearIndex + 1);

        m_GearSound.Play();
    }
    public void DownGear()
    {
        ShiftGear(m_SelectedGearIndex - 1);

        m_GearSound.Play();

    }
    public void ShiftToReverseGear()
    {
        m_SelectedGear = m_RearGear;
        m_SelectedGearIndex = -1;

        m_GearText.text = "R";

        m_GearSound.Play();

    }
    public void ShiftToFirstGear()
    {
        ShiftGear(0);


    }
    public void ShiftToNeutral()
    {
        m_SelectedGear = 0;
        m_SelectedGearIndex = -1;

        m_GearText.text = "N";

        m_GearSound.Play();

    }
    private void ShiftGear(int gearIndex)
    {


        gearIndex = Mathf.Clamp(gearIndex, 0, m_Gears.Length - 1);


        m_SelectedGear = m_Gears[gearIndex];
        m_SelectedGearIndex = gearIndex;

        // UI
        m_GearText.text = (m_SelectedGearIndex + 1).ToString();

    }

    private void UpdateEngineTorque()
    {
        m_EngineRpm = m_EngineMinRpm + Mathf.Abs(m_Chassis.GetAverageRpm() * m_SelectedGear * m_FinalDriveRatio);

        m_EngineRpm = Mathf.Clamp(m_EngineRpm, m_EngineMinRpm, m_EngineMaxRpm);

        m_EngineTorque = m_EngineTorqueCurve.Evaluate(m_EngineRpm / m_EngineMaxRpm) * m_EngineMaxTorque * m_FinalDriveRatio * Mathf.Sign(m_SelectedGear) * m_Gears[0];

        // UI
        m_RpmImage.fillAmount = (m_EngineRpm - m_EngineMinRpm) / (m_EngineMaxRpm - m_EngineMinRpm);
    }
    public void Reset()
    {
        m_Chassis.Reset();

        m_Chassis.MotorTorque = 0;
        m_Chassis.BrakeTorque = 0;
        m_Chassis.SteerAngle = 0;

        ThrottleControl = 0;
        BrakeControl = 0;
        SteerControl = 0;
    }

    public void Respawn(Vector3 position, Quaternion rotation)
    {
        Reset();

        transform.position = position;
        transform.rotation = rotation;
    }

   
}
