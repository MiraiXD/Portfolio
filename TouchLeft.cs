using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Networking;
// TODO: this script CAN be used to detect the events of a left networked hand touching a shared object
// fill in the implementation and communicate touching events to either LeapGrab and ViveGrab by setting the rightHandTouching variable
// ALTERNATIVELY, implement the verification of the grabbing conditions in a way  your prefer
// TO REMEMBER: only the localPlayer (networked hands belonging to the localPlayer) should be able to "touch" shared objects

public class TouchLeft : NetworkBehaviour
{

    public bool vive;
    public bool leap;

    private LeapGrab leapGrab;
    private ViveGrab viveGrab;
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
