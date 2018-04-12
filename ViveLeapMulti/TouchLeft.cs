//This script is used to detect the events of a left networked hand touching a shared object
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Networking;

public class TouchLeft : NetworkBehaviour
{
    // Vive player instance of the game?
    public bool vive;
    // Leap player instance of the game?
    public bool leap;
    // Leapgrab scipt of the LeapMotion hand
    private LeapGrab leapGrab;
    // ViveGrab script of the Vive controller
    private ViveGrab viveGrab;
    // NetID of the controller
    NetworkIdentity actorNetID;
    
    private void Update()
    {
        if(actorNetID == null) actorNetID = transform.parent.parent.GetComponent<NetworkIdentity>();
    }

    void OnTriggerEnter(Collider collider)
    {        
        
        if (actorNetID.isLocalPlayer)
        {
            if (collider.tag == "shared")
            {
                if (leap)
                {
                    leapGrab = transform.root.GetComponent<LeapGrab>();
                    leapGrab.OnLeftHandTouchingActivate(collider.GetComponent<AuthorityManager>());
                }
                else if (vive)
                {
                    viveGrab = transform.root.GetComponent<ViveGrab>();
                    viveGrab.OnLeftHandTouchingActivate(collider.GetComponent<AuthorityManager>());
                }
            }
        }

    }

    void OnTriggerExit(Collider collider)
    {
        if (actorNetID.isLocalPlayer)
        {
            if (collider.tag == "shared")
            {
                if (leap)
                    leapGrab.OnLeftHandTouchingDeactivate();
                else if (vive)
                    viveGrab.OnLeftHandTouchingDeactivate();
            }

        }
    }
}
