// This script allows the Vive player to move by swinging his hands, as if running
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveGestureMove : MonoBehaviour {    

    // Transform of the controller
    public Transform playerController;
    //SteamVR_TrackedObject of this obj
    SteamVR_TrackedObject trackedObject;
    // Used for input
    private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObject.index); } }
    // Trigget button ID
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    // Is the trigger button pressed?
    bool triggerDown = false;
    // Previous position of the controller
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
