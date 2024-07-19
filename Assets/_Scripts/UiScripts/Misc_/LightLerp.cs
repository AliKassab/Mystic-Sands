using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLerp : MonoBehaviour
{
    Light jarLight;
    [SerializeField] float intensityAmplitude = 1.5f;
    [SerializeField] float delayTime = 3f; // Delay time in seconds

    void OnDisable()
    {
        StopAllCoroutines();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(StartLerp), delayTime);
    }

    void StartLerp()
    {   
        jarLight = GetComponent<Light>();
        StartCoroutine(LerpIntensity());
    }

    IEnumerator LerpIntensity()
    {
        while (true)
        {
            jarLight.intensity = MathF.Abs(Mathf.Sin(Time.time) * intensityAmplitude);
            yield return null;
        }
    }


}
