using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentElement : MonoBehaviour
{
    void Awake() => DontDestroyOnLoad(gameObject);
}
