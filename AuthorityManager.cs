using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


// TODO: this script should manage authority for a shared object
public class AuthorityManager : NetworkBehaviour
{
    NetworkIdentity netID; // NetworkIdentity component attached to this game object

    // these variables should be set up on a client
    //**************************************************************************************************
    Actor localActor; // Actor that is steering this player 

    private bool grabbed = false; // if this is true client authority for the object should be requested
    public bool grabbedByPlayer // private "grabbed" field can be accessed from other scripts through grabbedByPlayer
    {
        get { return grabbed; }
        set { grabbed = value; }
    }
    
    private bool onGrabbed = false;
    private bool onReleased = false;

   // OnGrabbedBehaviour onb; // component defining the behaviour of this GO when it is grabbed by a player
                            // this component can implement different functionality for different GO´s


    //***************************************************************************************************

    // these variables should be set up on the server

    // TODO: implement a mechanism for storing consequent authority requests from different clients
    // e.g. manage a situation where a client requests authority over an object that is currently being manipulated by another client

    //*****************************************************************************************************

    // TODO: avoid sending two or more consecutive RemoveClientAuthority or AssignClientAUthority commands for the same client and shared object
    // a mechanism preventing such situations can be implemented either on the client or on the server

    // Use this for initialization
    void Start()
    {
        netID = GetComponent<NetworkIdentity>();
       // onb = GetComponent<OnGrabbedBehaviour>();
        
    }

    // Update is called once per frame
    void Update()
    {        
        if (isServer) return;
        if (localActor == null) AssignActor(GameObject.Find("PlayerController").GetComponentInChildren<Actor>());


        //if (!hasAuthority)
            if (grabbed)
            {
            localActor.RequestObjectAuthority(netID);
            if (!onGrabbed)
                    if (hasAuthority && isClient)
                    {
                  //      onb.OnGrabbed();
                        onGrabbed = true;
                        onReleased = false;
                    }            
                
            }
        else
        {
            if (onGrabbed && !onReleased)
            {
              //  onb.OnReleased();
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
