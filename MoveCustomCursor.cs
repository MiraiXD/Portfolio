using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCustomCursor : MonoBehaviour {

    public CursorRecharge cursorRecharge;
    public Player player;
    Image cursor;
    //float rechargeTime = 3;
    bool recharging = false;
    public int defaultStrength = 1;
    int strength;
    bool strengthAltered = false;

	// Use this for initialization
	void Start () {
        cursor = GetComponent<Image>();
        Cursor.visible = false;
        strength = defaultStrength;
	}
	
    bool GamePaused()
    {
        return Time.timeScale == 0;
    }

    public void SetStrength(int _strength)
    {
        strength = _strength;
        strengthAltered = true;
    }

	// Update is called once per frame
	void Update () {
        if (GamePaused()) return;

        cursor.transform.position = Input.mousePosition;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!cursorRecharge.recharging)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        Enemy enemy = hit.collider.GetComponent<Enemy>();
                        if(enemy.alive)
                        {
                            enemy.GetHit(strength);
                            if(strengthAltered)
                            {
                                strengthAltered = false;
                                strength = defaultStrength;
                            }
                            cursorRecharge.Recharge();
                        }                        
                    }
                }
            }
        }
	}
}
