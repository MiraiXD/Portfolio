using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayPost : AbilityButton
{

    public float _rechargeTime = 5;
    public int damage = 3;
    EnemySpawner enemySpawner;

    // Use this for initialization
    public void Start()
    {
        base.Start();
        SetRechargeTime(_rechargeTime);
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    public override void Ability()
    {        
        List<GameObject> enemies = enemySpawner.GetEnemies();
        foreach(GameObject go in enemies)
        {
            Enemy enemy = go.GetComponent<Enemy>();
            if (enemy.boy) enemy.GetHit(damage);
        }
    }
}
