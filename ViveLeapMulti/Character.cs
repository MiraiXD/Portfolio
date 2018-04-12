// This script handles local instances of the players, attaches local actors, updates positions of hand models
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Character : NetworkBehaviour {

    [SyncVar(hook = "SetActor")]
    // NetworkInstanceId of this object
    private NetworkInstanceId actorId;
    // Local actor
    private Actor actor;
    // Left hand/controller
    public Transform left;
    // Right hand/controller
    public Transform right;

    private bool leftActive = false;
    private bool rightActive = false;

    private void Awake()
    {
        name = name.Replace("(Clone)", "");        
    }
            
    private void Start()
    {
        if (isClient)
        {
            SetActor(actorId);
        }       
    }

    public void UpdateCharacterLeft(Vector3 leftPos, Quaternion leftRot)
    {
        if (!leftActive)
            SetLeftActive(true);
        left.position = leftPos;
        left.rotation = leftRot;
    }

    public void UpdateCharacterRight(Vector3 rightPos, Quaternion rightRot)
    {
        if (!rightActive)
            SetRightActive(true);
        right.position = rightPos;
        right.rotation = rightRot;
    }

    public void SetLeftActive(bool active)
    {
        left.gameObject.SetActive(active);
        leftActive = active;
    }

    public void SetRightActive(bool active)
    {
        right.gameObject.SetActive(active);
        rightActive = active;
    }

    
    // Sets the player as parent and initialize.    
    public void SetActor(NetworkInstanceId id)
    {
        actorId = id;
        if (!id.IsEmpty())
        {
            // Set new Player
            GameObject actorObject = NetworkServer.FindLocalObject(id);
            if (!actorObject)
            {
                actorObject = ClientScene.FindLocalObject(id);
            }

            actor = actorObject.GetComponent<Actor>();
            actor.character = this;
            transform.SetParent(actor.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
    
    // Attaches the character to the player. This runs only on server.    
    [Server]
    public void AttachToActor(NetworkInstanceId id)
    { 
        SetActor(id);

        if (actor.connectionToClient != null)
        {
            var networkIdentity = GetComponent<NetworkIdentity>();
            if (networkIdentity.localPlayerAuthority)
                networkIdentity.AssignClientAuthority(actor.connectionToClient);
        }
    }

}
