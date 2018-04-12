using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
// TODO: this script CAN be used to detect the events of a right networked hand touching a shared object
// fill in the implementation and communicate touching events to either LeapGrab and ViveGrab by setting the rightHandTouching variable
// ALTERNATIVELY, implement the verification of the grabbing conditions in a way  your prefer
// TO REMEMBER: only the localPlayer (networked hands belonging to the localPlayer) should be able to "touch" shared objects

public class TouchRight : NetworkBehaviour
{

    // the implementation of a touch condition might be different for Vive and Leap 
    public bool vive;
    public bool leap;

    private LeapGrab leapGrab;
    private ViveGrab viveGrab;
    NetworkIdentity actorNetID;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "shared")
        {
            if (actorNetID.isLocalPlayer)
            {

                /*if (leap)
                {
                    leapGrab = transform.root.GetComponent<LeapGrab>();
                    leapGrab.OnRightHandTouchingActivate(collider.GetComponent<AuthorityManager>());
                }
                else if (vive)
                {
                    viveGrab = transform.parent.parent.GetComponent<ViveGrab>();
                    viveGrab.OnRightHandTouchingActivate(collider.GetComponent<AuthorityManager>());
                    
                }*/
                print("righttouching");
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
               /* if (leap)
                    leapGrab.OnRightHandTouchingDeactivate();
                else if (vive)
                    viveGrab.OnRightHandTouchingDeactivate();
                    */
            }
        }
    }
    
}
