using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ServerSpawner : NetworkBehaviour
{

    public GameObject prefab;
    public GameObject enemyPrefab;
    public GameObject elevatorPrefab;
    public GameObject ballPrefab;
    public Transform[] enemySpawnPoints;
    public Transform[] transformTable;
    public Transform elevatorTransform;
    public float spawnFrequency = 10;
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