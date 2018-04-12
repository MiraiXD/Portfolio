// This script controls the behaviour of the pot holes - pulls the balls inside when they approach and spits them out on the other side

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotScript : MonoBehaviour {
    // Vector pointing out the pot - away from the box
	private Vector3 outwardDirection;
    // Public table containing walls of the cube
	public GameObject[] boxWalls {
		get;
		set;
	}
    // Strength of the force applied to the balls, used for movement inside the pot sphere
	private float strength = 1f;
	// Use this for initialization
	void Start () {        		
		outwardDirection = transform.position/transform.position.magnitude;
	}
		
	void OnTriggerEnter(Collider collider)
	{		
		if (collider.tag == "Ball") 
		{
            // Make the balls ignore boxwalls
			foreach (GameObject wall in boxWalls)
				Physics.IgnoreCollision (wall.GetComponent<BoxCollider> (), collider);
			
			collider.GetComponent<Rigidbody> ().velocity = outwardDirection * strength;			
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "Ball") 
		{			
			collider.GetComponent<Rigidbody> ().velocity = Vector3.zero;			
			collider.GetComponent<Rigidbody> ().useGravity = true;
		}
	}
}
