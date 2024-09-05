using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using System;

public class convStart : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;

    public GameObject smoke;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E)){
                Console.WriteLine("EEEEEEEEE");
                print("E pressed");
                Instantiate(smoke, transform.position, Quaternion.identity);
                ConversationManager.Instance.StartConversation(myConversation);
            }
           
        }
    }

    
}
