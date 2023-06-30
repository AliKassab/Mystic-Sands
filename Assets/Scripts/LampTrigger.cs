using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampTrigger : MonoBehaviour
{

    Animator anm;

    // Start is called before the first frame update
    void Start()
    {
        anm = GetComponent<Animator>();
        anm.enabled= false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anm.enabled = true;
        }
    }
}
