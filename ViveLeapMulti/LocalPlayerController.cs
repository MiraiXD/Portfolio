// Control the player on the local instance of the game
using UnityEngine;
using System.Collections;

public class LocalPlayerController : MonoBehaviour {

    // The network actor runs on all clients
    public Actor actor; 
    // Left LeapMotion hand/Vive controller
    public Transform left;
    // Right LeapMotion hand/Vive controller
    public Transform right;
    // One instance of this script allowed
    public static LocalPlayerController Singleton { get; private set; }   

    // Sets the player on start up.    
    public void SetActor(Actor newActor)
    {
        actor = newActor;

        // Initialize locally to update on all clients
        Actor actorPrefab = (Resources.Load("Prefabs/" + actor.name.Replace("(Clone)", "")) as GameObject).GetComponent<Actor>();
        actor.transform.SetParent(transform);

        var prefabName = "";
        
        prefabName = actorPrefab.character != null ? actorPrefab.character.name : "";

        actor.Initialize(prefabName);
    }

    private void Awake()
    {
        Singleton = this;
    }


    public void UpdateActorLeft(Vector3 leftPos, Quaternion leftRot)
    {
        actor.UpdateActorLeft(leftPos, leftRot);
    }

    public void UpdateActorRight(Vector3 rightPos, Quaternion rightRot)
    {
        actor.UpdateActorRight(rightPos, rightRot);
    }
    
	void Update () {

        if(actor)
        {
            if (left.gameObject.activeSelf)
                UpdateActorLeft(left.position, left.rotation);

            if (right.gameObject.activeSelf)
                UpdateActorRight(right.position, right.rotation);
        }
      
    }
}
