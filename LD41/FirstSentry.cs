using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Script sitting on the first sentry that player meets in the game. He's supposed to react when the player kills
// the FirstPrisoner - open the door and attack him
public class FirstSentry : MonoBehaviour {
    // The door to open
    public Transform doorTransform;
    // DeathEvent of the FirstPrisoner
    public DeathEvent firstPrisonerDeathEvent;
    // Player transform
    Transform player;
    // 
    NavMeshAgent navMeshAgent;
    // Max speed he can move at
    public float speed = 1f;
    //
    Animator anim;
    // Attack animation HashId
    int attackHashId = Animator.StringToHash("Attack");
    // Should the sentry attack the player?
    bool attackPlayer = false;
    // Distance below which the sentry will attack the player, above which he will try to move closer
    float distanceToAttack = 0.8f;
	
	void Start () {
        firstPrisonerDeathEvent.deathEvent += OnPrisonerDeath;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
	}
	
    void OnPrisonerDeath()
    {
        firstPrisonerDeathEvent.deathEvent -= OnPrisonerDeath;        
        doorTransform.GetComponent<DoorScript>().OpenDoor();
        attackPlayer = true;
        navMeshAgent.stoppingDistance = 0.8f;                
    }
	
	void Update () {
        // Set the parameter that steer the animation blend tree
        anim.SetFloat("speedPercentage", navMeshAgent.velocity.magnitude / speed);
        
        if(attackPlayer)
        {
            // Should I move closer?
            if (Vector3.Distance(transform.position, player.position) > distanceToAttack)
            {
                if (anim.GetBool(attackHashId))
                    anim.SetBool(attackHashId, false);
                navMeshAgent.SetDestination(player.position);
            }
            // If not then attack
            else Attack();
        }
    }


    void Attack()
    {
        if(!anim.GetBool(attackHashId))        
        anim.SetBool(attackHashId, true);
    }
}
