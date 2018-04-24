using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for dropping loot when an enemy dies
// DeathEvent class notifies you when the enemy has died
[RequireComponent(typeof(DeathEvent))]
public class EnemyDrop : MonoBehaviour {
    // Prefab of the dropped item
    public GameObject droppedItem;
    // A spot where the object is spawned, relative to the character
    public Transform droppedItemSpawnSpot;
	
    // Subscribe to the death event
	void Start () {
        GetComponent<DeathEvent>().deathEvent += OnDeath;        
	}
	
    void OnDeath()
    {
        Instantiate(droppedItem, droppedItemSpawnSpot.position, droppedItemSpawnSpot.rotation);
    }
}
