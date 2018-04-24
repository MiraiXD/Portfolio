using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

// Script describing the characters' health, death and healthbar of the player
public class Health : MonoBehaviour {

    Animator anim;
    // Die animation HashId
    int dieHashId = Animator.StringToHash("Die");
    // Max health of the character, also its starting health
    public int maxHealth = 100;
    // Current health, after taking damage etc.
    public int currentHealth { set;  get; }
    // The player's health slider
    public Slider slider;
    // Is this character the player's character?
    public bool isPlayer = false;
   
    void Start () {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        if(isPlayer)
        slider.value = 1.0f;
	}
	
    // Method for taking damage
    public void Damage(int dmg)
    {
        currentHealth -= dmg;
        if (isPlayer)
            UpdateHealthBar();
        if (currentHealth <= 0) Die();
    }

    // Dying function, play animation and make the corpse not impede player's movement
    void Die()
    {
        anim.SetTrigger(dieHashId);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.isStopped = true;
        GetComponent<Damage>().enabled = false;
        this.enabled = false;
    }

    // Function for healing, i.e. when picking up an item
    public void HealthBoost(int healthBoost)
    {
        currentHealth += healthBoost;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        //maxHealth += healthBoost;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {        
        slider.value = (float)currentHealth / (float)maxHealth;
    }
}
