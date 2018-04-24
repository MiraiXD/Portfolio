using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class describes characters' ability to attack and deal damage, also handles attack animations
public class Damage : MonoBehaviour {
    // Animator component for managing attack animations
    Animator anim;
    // For reading current animation state
    AnimatorStateInfo stateInfo;
    // HashIds for identifying and invoking attack animations
    int rightKickHash = Animator.StringToHash("Base Layer.RightKick");
    int leftKickHash = Animator.StringToHash("Base Layer.LeftKick");
    int leftPunchHash = Animator.StringToHash("Base Layer.LeftPunch");
    int rightPunchHash = Animator.StringToHash("Base Layer.RightPunch");

    // Did the weapon hit something?
    [HideInInspector]
    public bool weaponHit = false;
    // Did you hit someone during this attack animation?
    bool hitDuringThisAttack = false;
    // Used to deal damage to hit enemy
    public Health enemyHealth { get; set; }
    // Amount of dmg dealt
    public int damage = 10;
    
    void Start () {
        anim = GetComponent<Animator>();
	}
		
	void Update () {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        // If we're in attack state
        if(stateInfo.fullPathHash == rightKickHash || stateInfo.fullPathHash == leftKickHash || stateInfo.fullPathHash == leftPunchHash || stateInfo.fullPathHash == rightPunchHash)
        {
            if (weaponHit)
            {
                if (!hitDuringThisAttack)
                {                    
                    enemyHealth.Damage(damage);
                    hitDuringThisAttack = true;
                }
            }
        }
	}

    public void HitEvent()
    {        
        hitDuringThisAttack = false;
    }
    
}
