// This script spawns objects on the server and clients
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ServerSpawner : NetworkBehaviour
{
    // Shared object prefab to spawn
    public GameObject prefab;
    // Enemy prefab to spawn
    public GameObject enemyPrefab;
    // Elevator prefab to spawn
    public GameObject elevatorPrefab;
    // Basketball thrown prefab to spawn
    public GameObject ballPrefab;
    // Spawnlocations of the enemies
    public Transform[] enemySpawnPoints;
    // Table with spawnpoints of the shared objects
    public Transform[] transformTable;
    // Position of the elevator
    public Transform elevatorTransform;
    // Spawn freq of the enemies
    public float spawnFrequency = 10;
    // Time of the next spawn of an enemy
    float nextSpawn;

    public override void OnStartServer()    
    {        
        base.OnStartServer();
        Spawn();
        nextSpawn = spawnFrequency;
    }

    private void Update()
    {        
        if(Time.time >= nextSpawn)
        {
            nextSpawn += spawnFrequency;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 position = enemySpawnPoints[ (int)Mathf.Ceil(Random.Range(0, enemySpawnPoints.Length))].position;
        GameObject go = Instantiate(enemyPrefab, position, Quaternion.identity);
        NetworkServer.Spawn(go);
    }

    void Spawn()
    {
        foreach (Transform transform in transformTable)
        {
            GameObject go = Instantiate(prefab, transform.position, transform.rotation);
            NetworkServer.Spawn(go);
        }

        GameObject elevator = Instantiate(elevatorPrefab, elevatorTransform.position, elevatorTransform.rotation);
        NetworkServer.Spawn(elevator);
    }

    public void SpawnBall(Vector3 pos, Quaternion rot)
    {
        GameObject go = Instantiate(ballPrefab, pos, rot);
        NetworkServer.Spawn(go);
    }
}