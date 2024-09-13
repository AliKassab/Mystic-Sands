using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class DialogueTriggerer : MonoBehaviour
{
    [SerializeField] private NPCConversation NPCConversation;

    private void OnTriggerEnter(Collider other)
    {
        ConversationManager.Instance.StartConversation(NPCConversation);
        gameObject.SetActive(false);
    }
}
