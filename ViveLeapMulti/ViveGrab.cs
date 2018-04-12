// This script defines conditions that are necessary for the Vive player to grab a shared object
using UnityEngine;
using System.Collections;

public class ViveGrab : MonoBehaviour
{
    // to communicate the fulfillment of grabbing conditions
    AuthorityManager am; 

    public SteamVR_TrackedObject trackedObjectLeft;
    public SteamVR_TrackedObject trackedObjectRight;
    private SteamVR_Controller.Device deviceLeft { get { return SteamVR_Controller.Input((int)trackedObjectLeft.index); } }
    private SteamVR_Controller.Device deviceRight { get { return SteamVR_Controller.Input((int)trackedObjectRight.index); } }
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    // conditions for the object control here
    bool leftHandTouching = false;
    bool rightHandTouching = false;
    bool leftTriggerDown = false;
    bool rightTriggerDown = false;

        
    void Update()
    {
        if (deviceRight.GetPressDown(triggerButton)) rightTriggerDown = true;
        if (deviceRight.GetPressUp(triggerButton)) rightTriggerDown = false;

        if (deviceLeft.GetPressDown(triggerButton)) leftTriggerDown = true;
        if (deviceLeft.GetPressUp(triggerButton)) leftTriggerDown = false;        

        if (leftHandTouching && rightHandTouching && leftTriggerDown && rightTriggerDown)
        {
            // notify AuthorityManager that grab conditions are fulfilled
            if (am != null)
                am.grabbedByPlayer = true;
        }
        else
        {
            // grab conditions are not fulfilled
            if (am != null)
                am.grabbedByPlayer = false;
        }

    }

    public void OnRightHandTouchingActivate(AuthorityManager _am)
    {
        rightHandTouching = true;
        am = _am;
    }
    public void OnLeftHandTouchingActivate(AuthorityManager _am)
    {
        leftHandTouching = true;
        am = _am;
    }
    public void OnRightHandTouchingDeactivate()
    {
        rightHandTouching = false;
        am = null;
    }
    public void OnLeftHandTouchingDeactivate()
    {
        leftHandTouching = false;
        am = null;
    }
}
