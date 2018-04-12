// This script manages authority for a shared object
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AuthorityManager : NetworkBehaviour
{
    // NetworkIdentity component attached to this game object
    NetworkIdentity netID;
    // Actor that is steering this player 
    Actor localActor;

    // if this is true client authority for the object should be requested
    private bool grabbed = false;
    // private "grabbed" field can be accessed from other scripts through grabbedByPlayer
    public bool grabbedByPlayer 
    {
        get { return grabbed; }
        set { grabbed = value; }
    }
    // Has just been grabbed
    private bool onGrabbed = false;
    // Has just been released
    private bool onReleased = false;
    
    void Start()
    {
        netID = GetComponent<NetworkIdentity>();               
    }
    
    void Update()
    {        
        if (isServer) return;
        if (localActor == null) AssignActor(GameObject.Find("PlayerController").GetComponentInChildren<Actor>());
        
            if (grabbed)
            {
            localActor.RequestObjectAuthority(netID);
            if (!onGrabbed)
                    if (hasAuthority && isClient)
                    {
                        onGrabbed = true;
                        onReleased = false;
                    }                            
            }
        else
        {
            if (onGrabbed && !onReleased)
            {              
                onReleased = true;
                onGrabbed = false;
            }
            localActor.ReturnObjectAuthority(netID);            
        }        
    }

    // assign localActor here
    public void AssignActor(Actor actor)
    {
        localActor = actor;
    }

    // should only be called on server (by an Actor)
    // assign the authority over this game object to a client with NetworkConnection conn
    [Server]
    public void AssignClientAuthority(NetworkConnection conn)
    {
        netID.AssignClientAuthority(conn);        
    }

    // should only be called on server (by an Actor)
    // remove the authority over this game object from a client with NetworkConnection conn
    [Server]
    public void RemoveClientAuthority(NetworkConnection conn)
    {        
        netID.RemoveClientAuthority(conn);
    }

}
