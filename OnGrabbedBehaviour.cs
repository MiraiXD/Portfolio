using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
// TODO: define the behaviour of a shared object when it is manipulated by a client

public class OnGrabbedBehaviour : NetworkBehaviour {

    Transform target;
    Rigidbody rigidbody;
    bool grabbed;
    public bool vive;
    public bool leap;
    bool onReleased = false;
    

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        string activeScene = SceneManager.GetActiveScene().name;
        if (activeScene == "VivePlayerScene") vive = true;
        else if (activeScene == "LeapPlayerScene") leap = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (isServer) return;

        if (!hasAuthority) return;
        
            if (vive)
        {            
            if (target == null) target = (GameObject.Find("PlayerController/Player/ViveHands/MiddlePoint") as GameObject).transform;
        }
        else if(leap)
        {
            if (target == null) target = (GameObject.Find("PlayerController/Player/LeapHands/MiddlePoint") as GameObject).transform;
        }
        //print(GameObject.Find("PlayerController/Player/ViveHands/MiddlePoint"));
        

        // GO´s behaviour when it is in a grabbed state (owned by a client) should be defined here
        if (grabbed)
        {                        
                transform.position = target.transform.position;
                transform.rotation = target.transform.rotation;            
        }
    }

    // called first time when the GO gets grabbed by a player
    public void OnGrabbed()
    {        
        if (isClient)
        {            
            CmdSetRB(true);           
            grabbed = true;           
        }
    }
    
    [Command]
    public void CmdSetRB(bool isGrabbed)
    {        
        SetRB(isGrabbed);
    }

    [Server]
    public void SetRB(bool isGrabbed)
    {        
        rigidbody.useGravity = !isGrabbed;
        grabbed = isGrabbed;
        RpcSetRB(isGrabbed);
    }

    [ClientRpc]
    public void RpcSetRB(bool isGrabbed)
    {        
        rigidbody.useGravity = !isGrabbed;        
        grabbed = isGrabbed;        
    }

    // called when the GO gets released by a player
    public void OnReleased()
    {
        if (isClient)
        {
            CmdSetRB(false);
            grabbed = false;
        }
    }
}
