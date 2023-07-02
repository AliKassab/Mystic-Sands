using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLerp : MonoBehaviour
{

    Light l;
    [SerializeField] float Amplitude = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        l = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        l.intensity = MathF.Abs( Mathf.Sin(Time.time) * Amplitude);
    }
}
