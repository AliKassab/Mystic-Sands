using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DialogueEditor;

public class DialogueInputManager : MonoBehaviour
{
    private void Update()
    {
        if (ConversationManager.Instance != null)
        {
            if (ConversationManager.Instance.IsConversationActive)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    ConversationManager.Instance.SelectPreviousOption();

                else if (Input.GetKeyDown(KeyCode.DownArrow))
                    ConversationManager.Instance.SelectNextOption();

                else if (Input.GetKeyDown(KeyCode.F))
                    ConversationManager.Instance.PressSelectedOption();
            }
        }
    }
}
