using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using System;

public class convStart : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
                Console.WriteLine("EEEEEEEEE");
                print("E pressed");
                ConversationManager.Instance.StartConversation(myConversation);
           
        }
    }
}
