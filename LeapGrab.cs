using UnityEngine;
using System.Collections;

// This script defines conditions that are necessary for the Leap player to grab a shared object
// TODO: values of these four boolean variables can be changed either directly here or through other components
// AuthorityManager of a shared object should be notifyed from this script

public class LeapGrab : MonoBehaviour
{

    AuthorityManager am;

    // conditions for the object control here
    bool leftHandTouching = false;
    bool rightHandTouching = false;
    bool leftPinch = false;
    bool rightPinch = false;

    

    // Update is called once per frame
    void Update()
    {
        print("lp" + leftPinch + "rp" + rightPinch + "lt" + leftHandTouching + "rt" + rightHandTouching);
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
