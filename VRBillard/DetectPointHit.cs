// This script runs when the player decides to strike the ball and, when detected, applies a velocity-based force to it

using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class DetectPointHit : MonoBehaviour
{
    // The strength of the strike
	[SerializeField]
	private float strength=5f;
    // Vector pointing at the ball, used for applying force to it
	private Vector3 headingVector;
    // Unit vector: direction = headingVector / headingVector.magnitude;
    private Vector3 direction;
    // The object of the cuestick
    private GameObject cueStick;
    // Cuestick's RB
    private Rigidbody rigidbody;
    // Left controller
    private GameObject left;
    //Right controller
    private GameObject right;
    // WandController script sitting on the left controller
    private WandController controller1;
    // WandController script sitting on the right controller
    private WandController controller2;



    private void Start()
    {
        cueStick = GameObject.Find("CueStickPrefab");
        rigidbody = cueStick.GetComponent<Rigidbody>();

        left = GameObject.Find("[CameraRig]").transform.Find("Controller(left)").gameObject;
        right = GameObject.Find("[CameraRig]").transform.Find("Controller(right)").gameObject;
        controller1 = left.GetComponent<WandController>();
        controller2 = right.GetComponent<WandController>();
    }
    

    void OnTriggerEnter(Collider collider)
    {
		if (collider.tag == "Ball") 
		{           
           if (controller1 != null && controller2 != null)
            {
                //Is in the shooting phase?
                if(controller1.shooting) 
                {
                    // Start vibration of the controller
                    controller2.StartHapticVibration(5f, 1);
                }
                //Is in the shooting phase?
                else if (controller2.shooting)
                {
                    // Start vibration of the controller
                    controller1.StartHapticVibration(5f, 1);
                }
                
            }           
            headingVector = collider.transform.position - transform.position;
			direction = headingVector / headingVector.magnitude;
            // Strength equals present cuestick's velocity
			strength = rigidbody.velocity.magnitude;            
			collider.GetComponent<Rigidbody> ().AddForce (strength * direction, ForceMode.Impulse);        

		}
    }
}
