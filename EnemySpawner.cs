using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {    

    public GameObject[] enemyPrefabs;
    // public GameObject enemyEasyBoy;
    // public GameObject enemyEasyGirl;
    // public GameObject enemyMediumBoy;
    // public GameObject enemyMediumGirl;
    // public GameObject enemyHardBoy;
    // public GameObject enemyHardGirl;

    List<GameObject> enemies;
    public float spawnRate = 5f;
    float nextSpawnTime;
    public int maxPerWave = 3;
    float timeModifier = -2f;
    float delta = 0.1f;
    // Use this for initialization
    void Start () {
        enemies = new List<GameObject>();
        nextSpawnTime = spawnRate;
	}
	
	// Update is called once per frame
	void Update () {

        //int r = (int)Mathf.Floor(Random.Range(0f, enemyPrefabs.Length / 2) + timeModifier);
        //print(r);        

        if (Time.time > nextSpawnTime)
        {            
            nextSpawnTime += spawnRate;
            if(timeModifier < 0)
            timeModifier += delta;            
            int howMany = (int)Mathf.Ceil(Random.Range(0f, maxPerWave));
            //Spawn( (int) Mathf.Ceil( Random.Range( 0f, maxPerWave)));
            Spawn(howMany);
            print(howMany);
        }
	}
    
    void Spawn(int howMany)
    {        
        StartCoroutine(SpawnCoroutine(howMany));
    }

    IEnumerator SpawnCoroutine(int howMany)
    {        
        for (int i=0; i < howMany; i++)
        {
            int r = (int) Mathf.Floor( Random.Range(0f, enemyPrefabs.Length / 2) + timeModifier );
            if (r < 0) r = 0;            

            GameObject go = (GameObject)Instantiate( enemyPrefabs [ Random.Range( 2*r, 2*r + 2 ) ] );
            go.transform.position = transform.position;
            enemies.Add(go);

            yield return new WaitForSeconds( Random.Range( 1.5f, spawnRate/maxPerWave ) );
        }
    }

    public List<GameObject> GetEnemies()
    {
        return enemies;
    }
}
