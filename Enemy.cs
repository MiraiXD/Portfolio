using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class Enemy : NetworkBehaviour {

    NavMeshAgent navMeshAgent;
    public Transform boxes;
    Animation anim;
    //public AnimationClip anim;
	// Use this for initialization
	void Start () {
		//if(isServer)
        
        navMeshAgent = GetComponent<NavMeshAgent>();        
        navMeshAgent.SetDestination(boxes.position);            
        anim = GetComponent<Animation>();        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "shared")
        {
            print("HIT BY OBJECT");
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;
            //Fade out 
            StartCoroutine(FadeCoroutine());
        }
        else if(other.tag == "Boxes")
        {
            print("Boxes");
            // Fade out
            StartCoroutine(FadeCoroutine());
        }

    }
    
    IEnumerator FadeCoroutine()
    {
        anim.Play("FadeOut");
        yield return new WaitForSeconds(anim.clip.length);
        Destroy(gameObject);
        
    }
    
    // Update is called once per frame
    void Update () {
        
        //foreach (Renderer r in renderers)
            //print(r.material.color.a);
	}
}
