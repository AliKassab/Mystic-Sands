using DialogueEditor;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels; // Array of UI panels
    private NPCConversation currentConversation; // Store the conversation to start after closing
    private int currentPanelIndex;
    public PlacementManager placementManager; // Reference to the Placement Manager

    // Method to open a panel based on the index and pass the conversation
    public void OpenPanel(int panelIndex, NPCConversation conversation)
    {
        if (panelIndex >= 0 && panelIndex < panels.Length)
        {
            // Store the current conversation and panel index
            currentConversation = conversation;
            currentPanelIndex = panelIndex;

            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(i == panelIndex); // Activate the selected panel and deactivate others
            }
        }
        else
        {
            Debug.LogWarning("Panel index out of range: " + panelIndex);
        }
    }

    // Method to close the currently open panel
    public void ClosePanel(int panelIndex)
    {
        if (panelIndex >= 0 && panelIndex < panels.Length)
        {
            // Close the specific panel
            Debug.Log("Closing panel with index: " + panelIndex);

            panels[panelIndex].SetActive(false);
            if (panelIndex == currentPanelIndex)
            {
                ConversationManager.Instance.StartConversation(currentConversation);
            }


            // Reset the placement status for this panel (if needed)

            // Start the corresponding conversation

        }
        else
        {
            Debug.LogWarning("Panel index out of range: " + panelIndex);
        }
    }

    public void conv()
    {
        if (currentConversation != null)
        {
            ConversationManager.Instance.StartConversation(currentConversation);
        }
        else
        {
            Debug.LogWarning("No conversation assigned to this panel.");
        }
    }

    // Method to close the currently open panel without specifying the index
    public void CloseCurrentPanel()
    {
        ClosePanel(currentPanelIndex); // Close the currently active panel by index
    }
    
}
