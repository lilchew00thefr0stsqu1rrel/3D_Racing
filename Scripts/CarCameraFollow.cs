using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraFollow : CarCameraComponent
{

    [Header("Offset")]
    [SerializeField] private float m_ViewHeight;
    [SerializeField] private float m_Height;
    [SerializeField] private float m_Distance;

    [Header("Damping")]
    [SerializeField] private float m_RotationDamping;
    [SerializeField] private float m_HeightDamping;
    [SerializeField] private float m_SpeedThreshold;

    private Transform m_Target;
    private new Rigidbody m_Rigidbody;
    private void FixedUpdate()
    {
        Vector3 velocity = m_Rigidbody.velocity;
        Vector3 targetRotation = m_Target.eulerAngles;

        if (velocity.magnitude > m_SpeedThreshold)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up).eulerAngles;
        }

        // Lerp
        float currentAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation.y, m_RotationDamping * Time.fixedDeltaTime);
        float currentHeight = Mathf.Lerp(transform.position.y, m_Target.position.y + m_Height, m_HeightDamping * Time.fixedDeltaTime);

        // Position
        Vector3 positionOffset = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * m_Distance;
        transform.position = m_Target.position - positionOffset;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);   

        // Rotation
        transform.LookAt(m_Target.position + new Vector3(0, m_ViewHeight, 0));
    }

    public override void SetProperties(Car car, Camera camera)
    {
        base.SetProperties(car, camera);

        m_Target = car.transform;
        m_Rigidbody = car.Rigidbody;
    }
}
