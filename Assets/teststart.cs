using DialogueEditor;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using DialogueEditor;
public class TestStart : MonoBehaviour
{
    public float targetYPosition = 5f; 
    public float speed = 2f;          
    private bool shouldMove = false;   
    public float delayTimer = 1f;
    private bool isInteractable = true;
    private PanelManager panelManager; 
    public int panelIndex;             
    [SerializeField] private NPCConversation myConversation;
    //public PlacementManager placementManager; 
    public void Initialize(PanelManager manager, int index, NPCConversation conversation)
    {
        panelManager = manager;
        panelIndex = index;
        myConversation = conversation; // Set the conversation for this instance
    }

    private void OnTriggerEnter(Collider other)
    {
        shouldMove = true;
        panelManager.OpenPanel(panelIndex, myConversation);
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

        //if (placementManager.allItemsPlacedCorrectly)
        //{
            
        //    panelManager.ClosePanel();
        //}
    }

}
