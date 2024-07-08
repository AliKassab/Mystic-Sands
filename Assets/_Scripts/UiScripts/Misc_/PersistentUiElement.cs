using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentUiElement : MonoBehaviour
{
    void Awake() => DontDestroyOnLoad(gameObject);
}
