// Manages the local instance of the game
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Actor : NetworkBehaviour
{
    public Character character;
    public new Transform transform;
    [SyncVar]
    private string prefabName = "";
    
    protected virtual void Awake()
    {
        transform = base.transform;
    }

    // Use this for initialization
    void Start()
    {
        if (isServer || isLocalPlayer)
        {
            if (isLocalPlayer)
            {
                // Inform the local player about his new character
                LocalPlayerController.Singleton.SetActor(this);
                CmdInitialize(prefabName);
            }
        }
        else
        {
            // Initialize on startup
            Initialize(prefabName);
        }

    }

    //runs only on LocalPlayer
    public void UpdateActorLeft(Vector3 leftPos, Quaternion leftRot) 
    {
        if (character != null)
        {
            character.UpdateCharacterLeft(leftPos, leftRot);
        }
    }

    //runs only on LocalPlayer
    public void UpdateActorRight(Vector3 rightPos, Quaternion rightRot) 
    {
        if (character != null)
        {
            character.UpdateCharacterRight(rightPos, rightRot);
        }
    }

    public void SetRightCharacterActive(bool active)
    {
        character.SetRightActive(active);
    }

    public void SetLeftCharacterActive(bool active)
    {
        character.SetLeftActive(active);
    }
    
    // Initialize the player locally.        
    public void Initialize(string prefab)
    {
        prefabName = prefab;
        name = name.Replace("(Clone)", "");
    }
    
    // Spawns the character of actor on all clients.
    // This runs on server only.    
    private void SpawnCharacter(string prefab)
    {
        // Spawn character
        GameObject modelPrefab = Resources.Load("Prefabs/" + prefab) as GameObject;
        GameObject model = (GameObject)Instantiate(modelPrefab, transform.position, transform.rotation) as GameObject;
        NetworkServer.Spawn(model);

        // Attach character to player
        AttachCharacter(model.GetComponent<Character>());
    }
    
    // Initializes the character on server to inform all clients. 
    /// This command calls the Initialize() method and spawns the character.    
    [Command]
    public void CmdInitialize(string prefab)
    {
        if (prefab.Length > 0)
        {
            CreateCharacter(prefab);
        }
    }
        
    // Creates the character and initializes on server.    
    [Server]
    public void CreateCharacter(string prefab)
    {
        SpawnCharacter(prefab);
        Initialize(prefab);
    }

    
    // Main routine to attach the character to this actor
    // This runs only on Server.    
    [Server]
    public void AttachCharacter(Character newCharacter)
    {
        newCharacter.AttachToActor(netId);
    }

    // Should only be run on localPlayer (by the AuthorityManager of a shared object)
    // Asks the server for the authority over an object with NetworkIdentity netID
    public void RequestObjectAuthority(NetworkIdentity netID)
    {
        if (isLocalPlayer)
            CmdAssignObjectAuthorityToClient(netID);
    }

    // Should only be run on localPlayer (by the AuthorityManager of a shared object)
    // Asks the server to remove the authority over an object with NetworkIdentity netID
    public void ReturnObjectAuthority(NetworkIdentity netID)
    {
        if (isLocalPlayer)
            CmdRemoveObjectAuthorityFromClient(netID);
    }

    // run on the server
    // netID is NetworkIdentity of a shared object the authority if which should be passed to the client
    [Command]
    void CmdAssignObjectAuthorityToClient(NetworkIdentity netID)
    {       
        NetworkServer.FindLocalObject(netID.netId).GetComponent<AuthorityManager>().AssignClientAuthority(connectionToClient);               
    }

    // run on the server
    // netID is NetworkIdentity of a shared object the authority if which should be removed from the client
    [Command]
    void CmdRemoveObjectAuthorityFromClient(NetworkIdentity netID)
    {
        NetworkServer.FindLocalObject(netID.netId).GetComponent<AuthorityManager>().RemoveClientAuthority(connectionToClient);
    }    
}
