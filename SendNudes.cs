using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendNudes : AbilityButton {

    public float _rechargeTime = 5;
    public int damage = 5;
    MoveCustomCursor cursor;

	// Use this for initialization
	public void Start () {
        base.Start();
        SetRechargeTime(_rechargeTime);
        cursor = FindObjectOfType<MoveCustomCursor>();
	}
	
    public override void Ability()
    {
        cursor.SetStrength(damage);
    }    
}
