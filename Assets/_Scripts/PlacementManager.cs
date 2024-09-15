using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public List<RectTransform> items;  // List of items for panel 1
    public List<RectTransform> items2; // List of items for panel 2
    public List<RectTransform> items3; // List of items for panel 3
    public List<RectTransform> items4; // List of items for panel 4

    public List<RectTransform> correctPositions;  // Correct positions for panel 1
    public List<RectTransform> correctPositions2; // Correct positions for panel 2
    public List<RectTransform> correctPositions3; // Correct positions for panel 3
    public List<RectTransform> correctPositions4; // Correct positions for panel 4

    public PanelManager panelManager; // Reference to the Panel Manager

    private void Start()
    {
        ResetAllPlacementFlags();
    }

    public void CheckPlacement()
    {
        // Check item placement for each panel
        bool allItemsPlacedCorrectly1 = CheckItemsPlacement(items, correctPositions);
        bool allItemsPlacedCorrectly2 = CheckItemsPlacement(items2, correctPositions2);
        bool allItemsPlacedCorrectly3 = CheckItemsPlacement(items3, correctPositions3);
        bool allItemsPlacedCorrectly4 = CheckItemsPlacement(items4, correctPositions4);

        Debug.Log($"Panel 1 Placement Correct: {allItemsPlacedCorrectly1}");
        Debug.Log($"Panel 2 Placement Correct: {allItemsPlacedCorrectly2}");
        Debug.Log($"Panel 3 Placement Correct: {allItemsPlacedCorrectly3}");
        Debug.Log($"Panel 4 Placement Correct: {allItemsPlacedCorrectly4}");

        // Trigger the conversation and close panels only if all items are placed correctly
        if (allItemsPlacedCorrectly1)
        {
            panelManager.ClosePanel(0); // Close panel 1
            
        }

        if (allItemsPlacedCorrectly2)
        {
            panelManager.ClosePanel(1); // Close panel 2
        }

        if (allItemsPlacedCorrectly3)
        {
            panelManager.ClosePanel(2); // Close panel 3
        }

        if (allItemsPlacedCorrectly4)
        {
            panelManager.ClosePanel(3); // Close panel 4
        }
    }


    private bool CheckItemsPlacement(List<RectTransform> itemList, List<RectTransform> correctList)
    {
        // Check each item's position against its corresponding correct position
        for (int i = 0; i < itemList.Count; i++)
        {
            if (Vector2.Distance(itemList[i].anchoredPosition, correctList[i].anchoredPosition) > 1f)
            {
                return false; // If any item is out of place, return false
            }
        }
        return true; // All items placed correctly
    }

    private void ResetAllPlacementFlags()
    {
        // Reset the placement flags for all panels if needed (this can be extended)
    }

}
