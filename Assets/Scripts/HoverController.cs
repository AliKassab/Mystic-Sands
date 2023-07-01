using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class HoverController : MonoBehaviour
{
    
 
    [SerializeField] Light L1;
    [SerializeField] Light L2;
    [SerializeField] Light L3;
    [SerializeField] Light L4;


    void OnMouseOver()
        {
            L1.enabled= true;
            L2.enabled= true;
            L3.enabled= true;
            L4.enabled= true;
        }

        void OnMouseExit()
         {      
        L1.enabled = false;
        L2.enabled = false;
        L3.enabled = false;
        L4.enabled = false;
          }

}
