using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class VignetteBySpeed : MonoBehaviour
{
    [SerializeField] private Car car;
    private PostProcessVolume m_PostProcessVolume;
    // Start is called before the first frame update
    void Start()
    {
        m_PostProcessVolume = GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        m_PostProcessVolume.weight = car.NormalizedinearVelocity;
    }
}
