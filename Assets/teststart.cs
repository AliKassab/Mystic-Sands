using DialogueEditor;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using DialogueEditor;
public class TestStart : MonoBehaviour
{
    public float targetYPosition = 5f; // Target Y position
    public float speed = 2f;           // Speed of movement
    private bool shouldMove = false;   // Flag to control movement
    public float delayTimer = 1f;
    private bool isInteractable = true;
    private PanelManager panelManager; // Reference to the Panel Manager
    public int panelIndex;             // Index for the panel to open, set when instantiated
    [SerializeField] private NPCConversation myConversation;
    public void Initialize(PanelManager manager, int index, NPCConversation conversation)
    {
        panelManager = manager;
        panelIndex = index;
        myConversation = conversation; // Set the conversation for this instance
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && isInteractable)
        {
            Debug.Log("PRESS E");

            if (Input.GetKeyDown(KeyCode.E))
            {
                shouldMove = true; // Start moving the object

                // Open the specific panel and pass the conversation to the PanelManager
                panelManager.OpenPanel(panelIndex, myConversation);
                //if (myConversation != null)
                //{
                //    ConversationManager.Instance.StartConversation(myConversation);
                //}
            }
        }
    }

    void Update()
    {
        if (shouldMove)
        {
            // Move the object towards the target position
            Vector3 targetPosition = new Vector3(transform.position.x, targetYPosition, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

            // Stop moving if close enough to the target position
            if (Mathf.Abs(transform.position.y - targetYPosition) < 0.01f)
            {
                shouldMove = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            panelManager.ClosePanel();
        }
    }

}
