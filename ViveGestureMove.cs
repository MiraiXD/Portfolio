using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveGestureMove : MonoBehaviour {

    /* public SteamVR_TrackedObject trackedObjectLeft;
     public SteamVR_TrackedObject trackedObjectRight;
     private SteamVR_Controller.Device deviceLeft { get { return SteamVR_Controller.Input((int)trackedObjectLeft.index); } }
     private SteamVR_Controller.Device deviceRight { get { return SteamVR_Controller.Input((int)trackedObjectRight.index); } }
     private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

     bool leftTriggerDown = false;
     bool rightTriggerDown = false;
     */

    public Transform playerController;
    SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObject.index); } }
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    bool triggerDown = false;
    Vector3 prevPos;

    private void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {

        if (device.GetPressDown(triggerButton))
        {
            triggerDown = true;
            prevPos = transform.position;
        }
        if (device.GetPressUp(triggerButton)) triggerDown = false;

        if(triggerDown)
        {
            Vector3 shift = transform.position - prevPos;
            playerController.position += shift;
            prevPos = transform.position;
        }
       

    }
}
