// This script defines conditions that are necessary for the Leap player to grab a shared object
using UnityEngine;
using System.Collections;

public class LeapGrab : MonoBehaviour
{
    // AutorityManager script of the shared object we want to move
    AuthorityManager am;

    // Is left Leap hand touching an object?
    bool leftHandTouching = false;
    // Is right Leap hand touching an object?
    bool rightHandTouching = false;
    // Is left Leap hand doing the pinch gesture?
    bool leftPinch = false;
    // Is rightLeap hand doing the pinch gesture?
    bool rightPinch = false;
        
    void Update()
    {
        if (leftHandTouching && rightHandTouching && leftPinch && rightPinch)
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

    public void OnLeftPinchActivate()
    {
        leftPinch = true;
    }
    public void OnRightPinchActivate()
    {
        rightPinch = true;
    }
    public void OnLeftPinchDeactivate()
    {
        leftPinch = false;
    }
    public void OnRightPinchDeactivate()
    {
        rightPinch = false;
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
