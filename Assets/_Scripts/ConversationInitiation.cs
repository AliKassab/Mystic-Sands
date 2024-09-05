using System.Collections;
using UnityEngine;
using DialogueEditor;

public class ConversationInitiation : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    public float delayTimer = 1f;
    public GameObject smoke;
    public GameObject skullModel;
    public Animator skullAnimation;
    public AnimationClip animationClip;
    public TMPro.TextMeshProUGUI interactionText;
    private bool isInteractable = true;

    private void Start()
    {
        ToggleObject(skullModel, false);
        ToggleObject(interactionText.gameObject, false);  
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isInteractable)
        {
            ToggleObject(interactionText.gameObject, true); 
            
            if (Input.GetKeyDown(KeyCode.E) && isInteractable)
            {
                isInteractable = false;
                InitiateInteractionAction();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleObject(interactionText.gameObject, false);
        }
    }

    private void InitiateInteractionAction()
    {
        ToggleObject(skullModel, true);
        skullAnimation.Play(animationClip.name);
        StartCoroutine(InstantiateDelayedSmoke());
        ConversationManager.Instance.StartConversation(myConversation);
        ToggleObject(interactionText.gameObject, false);
    }


    private IEnumerator InstantiateDelayedSmoke()
    {
        yield return new WaitForSeconds(delayTimer);
        Instantiate(smoke, transform.position, Quaternion.identity);
    }

    void ToggleObject(GameObject gameObject, bool boolean) => gameObject.SetActive(boolean);
        
}
