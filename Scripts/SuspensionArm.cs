using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspensionArm : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_Factor;

    private float m_BaseOffset;

    private void Start()
    {
        m_BaseOffset = m_Target.localPosition.y;
    }

    private void Update()
    {
        transform.localEulerAngles = new Vector3(0, 0, (m_Target.localPosition.y - m_BaseOffset) * m_Factor);
    }
}
