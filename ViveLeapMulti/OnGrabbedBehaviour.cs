// This script defines behaviour of an object grabbed by one the players 
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class OnGrabbedBehaviour : NetworkBehaviour {

    Transform target;
    // This obj RB
    Rigidbody rigidbody;
    // Is this obj grabbed?
    bool grabbed;
    // Vive instance of the game?
    public bool vive;
    // Leap instance of the game?
    public bool leap;
    // Has just been released?
    bool onReleased = false;
        
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        string activeScene = SceneManager.GetActiveScene().name;
        if (activeScene == "VivePlayerScene") vive = true;
        else if (activeScene == "LeapPlayerScene") leap = true;
    }
		
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
        

        // GO´s behaviour when it is in a grabbed state (owned by a client)
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
