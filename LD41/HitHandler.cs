using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script decides when a certain part of body has hit a 'Hittable' thing
// and notifies the Damage script
public class HitHandler : MonoBehaviour {
    // Tag of the hittable object
    public string hittableTag = "Hittable";
    // Player tag
    public string playerTag = "Player";
    // Is the part of the body inside a hittable?
    bool isInside = false;
    // Ref to a Damage script to be notified
    Damage damageScript;

    void Start()
    {
        damageScript = transform.root.GetComponent<Damage>();
    }

    // Hit a hittable or the player?
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == hittableTag || collider.tag == playerTag)
        {
            // Notify Damage script
            damageScript.weaponHit = true;
            // Tell Damage script which script to notify of taking damage
            damageScript.enemyHealth = collider.GetComponent<Health>();            
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == hittableTag || collider.tag == playerTag)
        {
            damageScript.weaponHit = false;            
        }
    }

    

}
