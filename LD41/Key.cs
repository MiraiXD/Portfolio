using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class describing key objects, picked up and used for opening doors
public class Key : Interactable {
    // Type of key
    public enum Keys { FirstSentryKey, SecondSentryKey};
    public Keys keyType;

    // Find Text object
    new void Start()
    {
        base.Start();
    }

    // When picking up, check the type of the key and tell the DoorScript which key was picked up
    public override void Interact()
    {
        base.Interact();
        switch(keyType)
        {
            case Keys.FirstSentryKey:
                DoorScript.AcquireKey(Keys.FirstSentryKey);
                break;
            case Keys.SecondSentryKey:
                DoorScript.AcquireKey(Keys.SecondSentryKey);
                break;
        }
        DisableText();
        Destroy(gameObject);
    }
    
}
