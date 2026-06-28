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

    // Shared across all triggerers so the scene is scanned once total, not once per instance.
    private static MoveControl _cachedPlayerMovement;
    private static Flagger _cachedFlagger;

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
        // Unity's fake-null makes a destroyed cached object compare == null, so this
        // re-scans automatically on a fresh scene load but reuses the result otherwise.
        if (_cachedPlayerMovement == null)
            _cachedPlayerMovement = FindAnyObjectByType<MoveControl>();
        playerMovement = _cachedPlayerMovement;

        if (SceneManager.GetActiveScene().buildIndex == 2) return;
        conversation = GetComponentInParent<NPCConversation>();

        if (_cachedFlagger == null)
            _cachedFlagger = FindAnyObjectByType<Flagger>();
        flagger = _cachedFlagger;
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
