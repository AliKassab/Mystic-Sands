using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementManager : MonoBehaviour
{
    public List<RectTransform> items; // List of items that need to be placed
    public List<RectTransform> correctPositions; // List of correct positions
    public bool allItemsPlacedCorrectly;
    public PanelManager panelManager; // Reference to the Panel Manager

    private void Start()
    {
        allItemsPlacedCorrectly = false;
    }

    public void CheckPlacement()
    {
        for (int i = 0; i < items.Count; i++)
        {
            // Assuming exact match is required; you might need a tolerance for floating-point values
            if (Vector2.Distance(items[i].anchoredPosition, correctPositions[i].anchoredPosition) < 1f)
            {
                allItemsPlacedCorrectly = true;
                panelManager.ClosePanel();
            }
        }

        //if (allItemsPlacedCorrectly)
        {

            // Trigger any additional actions needed
           // panelManager.ClosePanel();
            
        }
    }
}
