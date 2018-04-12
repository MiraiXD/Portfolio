// This script manages enemies, sets their target to reach and fades them away when they die
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class Enemy : NetworkBehaviour {
    // Reference to the navMeshAgent
    NavMeshAgent navMeshAgent;
    // Transform of the target to reach - a pile of boxes
    public Transform boxes;
    // Fade out animation
    Animation anim;
    
	void Start () {	        
        navMeshAgent = GetComponent<NavMeshAgent>();        
        navMeshAgent.SetDestination(boxes.position);            
        anim = GetComponent<Animation>();        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "shared")
        {            
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;           
            StartCoroutine(FadeCoroutine());
        }
        else if(other.tag == "Boxes")
        {
            StartCoroutine(FadeCoroutine());
        }
    }
    
    IEnumerator FadeCoroutine()
    {
        anim.Play("FadeOut");
        yield return new WaitForSeconds(anim.clip.length);
        Destroy(gameObject);
        
    }    
}
