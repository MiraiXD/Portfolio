using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

// This script is placed on door object, lets you describe its opened and closed positions and by interacting - open and close at runtime 
public class DoorScript : Interactable {
    // Pos/Rot of the closed/opened door
    public Vector3 closedPosition;
    public Vector3 openedPosition;
    public Vector3 closedRotation;
    public Vector3 openedRotation;
    // Used to store rotation converted from Vector3 to Quaternion
    Quaternion closedRot;
    Quaternion openedRot;
    // Is the door opened by default?
    public bool isOpen = false;
    // How smooth is the open/close process
    public float smoothing = 0.03f;
    // How much left till desired position/rotation
    float distLeft = 1f;
    // Coroutine now being handled
    IEnumerator currentCoroutine = null;
    
    // Does this door require a key to be opened?    
    public bool requiresFirstSentryKey;
    public bool requiresSecondSentryKey;
    //Has the player acquired a key?
    static bool firstSentryKeyAcquired;
    static bool secondSentryKeyAcquired;
    
    new void Start () {
        base.Start();
        closedRot = Quaternion.Euler(closedRotation);
        openedRot = Quaternion.Euler(openedRotation);                
    }

    // Which key was picked up by the player
    public static void AcquireKey(Key.Keys key)
    {
        switch(key)
        {
            case Key.Keys.FirstSentryKey:
                firstSentryKeyAcquired = true;
                break;
            case Key.Keys.SecondSentryKey:
                secondSentryKeyAcquired = true;
                break;
        }
    }

    // Check if the player has the keys required to open this door
    bool CheckKeys()
    {
        if (requiresFirstSentryKey)
            if (!firstSentryKeyAcquired) return false;

        if (requiresSecondSentryKey)
            if (!secondSentryKeyAcquired) return false;

        return true;
    }

    // Overridden Interact method, checks keys and starts the open/close process
    public override void Interact()
    {
        base.Interact();
        if (!CheckKeys())
        {
            SetText("You need a key to open this door");
            return;
        }
        if (currentCoroutine == null)
        {
            if (isOpen) CloseDoor();
            else OpenDoor();
        }
    }

    // Method invoking opening coroutine
    public void OpenDoor()
    {        
        currentCoroutine = OpenCoroutine(smoothing);
        StartCoroutine(OpenCoroutine(smoothing));
    }

    // Method invoking closing coroutine
    public void CloseDoor()
    {
        currentCoroutine = CloseCoroutine(smoothing);
        StartCoroutine(CloseCoroutine(smoothing));
    }

    IEnumerator OpenCoroutine(float _smoothing)
    {        
        if (!isOpen)
        {
            if (transform.localPosition != openedPosition)
            {
                distLeft = 1f;           
                while (transform.localPosition != openedPosition)
                {

                    transform.localPosition = Vector3.Lerp(transform.localPosition, openedPosition, _smoothing / distLeft);
                    distLeft -= _smoothing;
                    yield return null;
                }
            }
            if (transform.localRotation != openedRot)
            {
                distLeft = 1f;
                while (transform.localRotation != openedRot)
                {
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, openedRot, _smoothing / distLeft);
                    distLeft -= _smoothing;
                    yield return null;
                }
            }
        }
        isOpen = true;
        currentCoroutine = null;
    }

    IEnumerator CloseCoroutine(float _smoothing)
    {        
        if (isOpen)
        {
            if (transform.localPosition != closedPosition)
            {
                distLeft = 1f;
                while (transform.localPosition != closedPosition)
                {

                    transform.localPosition = Vector3.Lerp(transform.localPosition, closedPosition, _smoothing / distLeft);
                    distLeft -= _smoothing;
                    yield return null;
                }
            }
            if (transform.localRotation != closedRot)                
            {
                distLeft = 1f;
                while (transform.localRotation != closedRot)
                {
                    print("close");
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, closedRot, _smoothing / distLeft);
                    distLeft -= _smoothing;
                    yield return null;
                }
            }
        }
        isOpen = false;
        currentCoroutine = null;
    }
}
