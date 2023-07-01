using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLerp : MonoBehaviour
{

    Light l;
    [SerializeField] float intensityF1 = 1f;
    [SerializeField] float intensityF2 = 1f;
    [SerializeField] float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        l = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        l.intensity = Mathf.Lerp(intensityF1, intensityF2, speed);
    }
}
