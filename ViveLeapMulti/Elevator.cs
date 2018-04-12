// This script handles the elevator on the server and clients
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Elevator : NetworkBehaviour {
    // Lift script
    public Lift lift;
    // LiftButton script
    public LiftButton liftButton;
    // Ready to elevate?
    bool canElevate = true;
    // Local actor on this instance of the game
    public Actor localActor;
    // AuthorityManager of the elevated object
    AuthorityManager am;
    // Are we waiting for the authority over an object?
    bool waitForAuthority = false;
	
	void Start () {
        am = GetComponent<AuthorityManager>();
	}
	
	
	void Update () {
        if (waitForAuthority)
            if(hasAuthority)
            {
                waitForAuthority = false;
                CmdElevate();
            }
	}

    [Command]
    public void CmdElevate()
    {
        Elevate();
        RpcElevate();
    }

    [ClientRpc]
    public void RpcElevate()
    {
        Elevate();
    }

    public void Elevate()
    {        
        StartCoroutine(ElevateCoroutine(liftButton.pressTime, lift.liftTime));
    }
    
    // Move the lift and the button
    IEnumerator ElevateCoroutine(float pressTime, float liftTime)
    {
        canElevate = false;
        liftButton.PressButton();
        yield return new WaitForSeconds(pressTime);
        lift.LiftMove();
        yield return new WaitForSeconds(liftTime);
        liftButton.UnpressButton();
        yield return new WaitForSeconds(pressTime);
        am.grabbedByPlayer = false;
        canElevate = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isClient)
        if (canElevate)
            if (other.tag == "Controller")
                {
                    am.grabbedByPlayer = true;
                    waitForAuthority = true;
                }
                
    }
}
