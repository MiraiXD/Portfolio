using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Elevator : NetworkBehaviour {

    public Lift lift;
    public LiftButton liftButton;
    bool canElevate = true;
    public Actor localActor;
    AuthorityManager am;
    bool waitForAuthority = false;
	// Use this for initialization
	void Start () {
        am = GetComponent<AuthorityManager>();
	}
	
	// Update is called once per frame
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
