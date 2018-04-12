using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour {    
    Rigidbody RB;
    public float maxSpeed = 5;

	// Use this for initialization
	void Start () {        
        RB = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float moveV = Input.GetAxis("Vertical");
        float moveH = Input.GetAxis("Horizontal");
        RB.MovePosition(RB.position + moveV * transform.forward * maxSpeed * Time.deltaTime);
        RB.MoveRotation(RB.rotation * Quaternion.Euler(new Vector3(0, 5*moveH, 0)));
        //transform.position += moveV * Time.deltaTime * transform.forward;// Vector3.Scale(Vector3.one, transform.forward);
        print(GameObject.Find("handles"));

	}
}
