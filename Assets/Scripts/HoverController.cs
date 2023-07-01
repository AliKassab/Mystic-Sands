using UnityEngine;

public class HoverController : MonoBehaviour
{
    
    [SerializeField] Light L1;
    
    void OnMouseOver()
        {
            L1.enabled= true;
        }

    void OnMouseExit()
        {      
            L1.enabled = false;
        }

}
