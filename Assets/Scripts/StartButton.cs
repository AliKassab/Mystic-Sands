using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [SerializeField] Light l;
    private void Update()
    {
        if (l.enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(1);
            }
        }
 
    }


}
