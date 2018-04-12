using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorRecharge : MonoBehaviour {

    Image cursor;
    public float maxRechargeTime = 5f;
    float rechargeTime;
    public bool recharging = false;
    IEnumerator currentCoroutine;

	// Use this for initialization
	void Start () {
        cursor = GetComponent<Image>();
        rechargeTime = maxRechargeTime;
	}

    public void Recharge()
    {
        //rechargeTime = _rechargeTime;
        if(currentCoroutine == null)
        {
            currentCoroutine = RechargeCoroutine();
            StartCoroutine(RechargeCoroutine());            
        }
    }

    IEnumerator RechargeCoroutine()
    {
        recharging = true;
        cursor.fillAmount = 0;
        while (cursor.fillAmount < 1)
            {
                cursor.fillAmount += Time.deltaTime / rechargeTime;
                yield return null;
            }
        currentCoroutine = null;
        recharging = false;
    } 

    public void SetRechargeTime(float _rechargeTime)
    {
        rechargeTime = _rechargeTime;
    }
}

