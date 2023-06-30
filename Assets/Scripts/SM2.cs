using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM2 : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    public void closeClick()
    {
        canvas.SetActive(false);
    }
}
