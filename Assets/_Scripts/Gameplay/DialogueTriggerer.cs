using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.SceneManagement;

public class DialogueTriggerer : MonoBehaviour
{
    private NPCConversation conversation;
    private MoveControl playerMovement;
    public Flagger flagger;
    private void OnTriggerEnter(Collider other)
    {
        playerMovement.enabled = false;
        if (SceneManager.GetActiveScene().buildIndex == 2) return;
        ConversationManager.Instance.StartConversation(conversation);
        
        if(conversation.gameObject.name == "PotsTrader1")
            flagger.MetPotTrader = true;
        else if (conversation.gameObject.name == "Commander1")
            flagger.MetCommander = true;
        else if (conversation.gameObject.name == "WriterKey")
            flagger.InitiateEnding = true;
        gameObject.SetActive(false);
        
    }

    private void Start()
    {
        playerMovement = FindObjectOfType<MoveControl>().GetComponent<MoveControl>();
        if (SceneManager.GetActiveScene().buildIndex == 2) return;
        conversation = GetComponentInParent<NPCConversation>();
        flagger = FindObjectOfType<Flagger>().GetComponent<Flagger>();
    }

    public void TriggerNextDialogue(GameObject DialogueTrigger)
    {
        if (DialogueTrigger != null) 
        DialogueTrigger.gameObject.SetActive(true);
    }

    public void BlockDialogue(GameObject DialogueTrigger)
    {
        DialogueTrigger.gameObject.SetActive(false);
    }

    public void enablePlayerMovement()
    {
        playerMovement.enabled = true;
    }

}
