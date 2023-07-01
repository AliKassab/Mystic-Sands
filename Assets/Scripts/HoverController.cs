using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

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
