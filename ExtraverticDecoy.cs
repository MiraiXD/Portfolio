using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraverticDecoy : AbilityButton
{

    public float _rechargeTime = 5;

    // Use this for initialization
    public void Start()
    {
        base.Start();
        SetRechargeTime(_rechargeTime);
        
    }

    public override void Ability()
    {
        print("EXTRAVERTICDECOY");
    }
}
