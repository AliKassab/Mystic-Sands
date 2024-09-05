using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using System;
using UnityEngine.UI;

public class convStart : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    public float delay = 1f;
    public GameObject smoke;
    public GameObject skullModel;
    public Animator anim_Skull;
    public string animationClipName = "skull";
    public TMPro.TextMeshProUGUI interactText;
    private void Start()
    {
        skullModel.SetActive(false);
        interactText.gameObject.SetActive(false);  
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.gameObject.SetActive(true); 

            if (Input.GetKeyDown(KeyCode.E))
            {
                print("E pressed");

                skullModel.SetActive(true);
                anim_Skull.Play(animationClipName);

                StartCoroutine(DelayedSmokeInstantiation());

                ConversationManager.Instance.StartConversation(myConversation);

                interactText.gameObject.SetActive(false);  
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.gameObject.SetActive(false); 
        }
    }

    private IEnumerator DelayedSmokeInstantiation()
    {
        yield return new WaitForSeconds(delay);
        Instantiate(smoke, transform.position, Quaternion.identity);
    }
}
