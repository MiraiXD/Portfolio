using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Base class for all objects, that can be interacted with, picked up, like doors, armor, loot
[RequireComponent(typeof(CapsuleCollider))]
public class Interactable : MonoBehaviour {
    // Is the player in range - able to interact?
    protected bool canInteract = false;
    // Player tag
    public string playerTag = "Player";
    // Text to display on the screen when ready to interact
    public string interactText = "Press 'F' to interact";
    // Ref to Text object to display interactText
    Text text;

    // Virtual function to be overridden in children to specify the way of interaction
    public virtual void Interact()
    {        
    }

    // Find the Text object in the scene
    public void Start()
    {
        text = GameObject.Find("Canvas/InteractText").GetComponent<Text>();        
    }

    // Set the Text's value
    public void SetText(string _text)
    {        
        text.text = _text;
        if(text.enabled == false)
        text.enabled = true;
    }

    public void DisableText()
    {
        text.enabled = false;
    }

    // Check if player is in range of interaction
    void OnTriggerStay(Collider collider)
    {

        if (collider.tag == playerTag)
        {
            if (!canInteract)
            {
                canInteract = true;
                SetText(interactText);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == playerTag)
        {
            canInteract = false;
            DisableText();
        }
    }
	
    // If the player can interact - check for input 	
	void Update () {        
        if (canInteract)
        {            
            if (Input.GetKeyDown(KeyCode.F)) Interact();
        }
	}
}
