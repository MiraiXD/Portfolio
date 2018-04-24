using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class generates an event when the character has died - used for dropping loot and notifying FirstSentry about FirstPrisoner's death
public class DeathEvent : MonoBehaviour {
    // This object's Health script
    Health health;
    // Death event 
    public delegate void DeathDelegate();
    public event DeathDelegate deathEvent;
    // Has already generated an event?
    bool invoked = false;
    
    void Start()
    {
        health = GetComponent<Health>();
    }

    void Update()
    {
        if(!invoked)
        if (health.currentHealth <= 0)
        {
            deathEvent();
                invoked = true;
        }
    }
}
