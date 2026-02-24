using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraFovCorrector : CarCameraComponent
{

    [SerializeField] private float m_MinFieldOfView;
    [SerializeField] private float m_MaxFieldOfView;

    private float defaultFov = 70;

    private void Start()
    {
        camera.fieldOfView = defaultFov;
    }

    private void Update()
    {
        camera.fieldOfView = Mathf.Lerp(m_MinFieldOfView, m_MaxFieldOfView, car.NormalizedinearVelocity);
    }
}
