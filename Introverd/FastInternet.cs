using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastInternet : MonoBehaviour {

    public float rechargeTimeMultiplier = 0.1f;
    Image image;
    Text text;

    private void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        image.enabled = false;
        text.enabled = false;
    }

    public void Enable()
    {
        image.enabled = true;
        text.enabled = true;
        CursorRecharge cursorRecharge = FindObjectOfType<CursorRecharge>();
        cursorRecharge.SetRechargeTime(cursorRecharge.maxRechargeTime * rechargeTimeMultiplier);
    }
}
