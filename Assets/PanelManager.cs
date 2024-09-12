using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels; // Array of UI panels
    private NPCConversation currentConversation; // Store the conversation to start after closing
    private int currentPanelIndex;

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

            Debug.Log("Opening panel: " + panelIndex);
        }
        else
        {
            Debug.LogWarning("Panel index out of range: " + panelIndex);
        }
    }

    // Method to close the currently open panel and start the conversation
    public void ClosePanel()
    {
        if (currentPanelIndex >= 0 && currentPanelIndex < panels.Length)
        {
            // Close the active panel
            panels[currentPanelIndex].SetActive(false);

            // Start the corresponding conversation
            if (currentConversation != null)
            {
                ConversationManager.Instance.StartConversation(currentConversation);
                Debug.Log("Starting conversation for panel: " + currentPanelIndex);
            }
            else
            {
                Debug.LogWarning("No conversation assigned to this panel.");
            }
        }
    }
}
