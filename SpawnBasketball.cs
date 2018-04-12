using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnBasketball : NetworkBehaviour {

    public GameObject ballPrefab;
    public Transform ballPosition;

    public void Spawn()
    {
        CmdSpawn();
    }

    [Command]
    public void CmdSpawn()
    {
        GameObject.Find("ServerSpawner").GetComponent<ServerSpawner>().SpawnBall( ballPosition.position , Quaternion.identity);
    }
}
