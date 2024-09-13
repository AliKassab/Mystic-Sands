using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueResume : MonoBehaviour
{
    [SerializeField] private GameObject DialogueTrigger;
    [SerializeField] private List<GameObject> TriggerList;

    private void OnTriggerEnter(Collider other)
    {
        DialogueTrigger.SetActive(true);
        foreach (var trigger in TriggerList)
        trigger.SetActive(false);
    }
}
