//This script is used to detect the events of a right networked hand touching a shared object
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class TouchRight : NetworkBehaviour
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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "shared")
        {
            if (actorNetID.isLocalPlayer)
            {

                if (leap)
                {
                    leapGrab = transform.root.GetComponent<LeapGrab>();
                    leapGrab.OnRightHandTouchingActivate(collider.GetComponent<AuthorityManager>());
                }
                else if (vive)
                {
                    viveGrab = transform.parent.parent.GetComponent<ViveGrab>();
                    viveGrab.OnRightHandTouchingActivate(collider.GetComponent<AuthorityManager>());
                    
                }
                
            }
        }
    }

    private void Update()
    {
        if (actorNetID == null) actorNetID = transform.parent.parent.GetComponent<NetworkIdentity>();

    }

    void OnTriggerExit(Collider collider)
    {
        if (actorNetID.isLocalPlayer)
        {
            if (collider.tag == "shared")
            {
                print("riughtexitting");
               if (leap)
                    leapGrab.OnRightHandTouchingDeactivate();
                else if (vive)
                    viveGrab.OnRightHandTouchingDeactivate();
                    
            }
        }
    }
    
}
