using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebBot : MonoBehaviour {

    Image image;
    Image disabledImage;
    Text text;
    bool enabled = false;
    IEnumerator currentCoroutine;
    public float rechargeTime;
    public int damage = 1;
    EnemySpawner enemySpawner;

	// Use this for initialization
	void Start () {
        Image[] images = GetComponentsInChildren<Image>();
        image = images[0];
        disabledImage = images[1];
        text = GetComponentInChildren<Text>();
        enemySpawner = FindObjectOfType<EnemySpawner>();

        image.enabled = false;
        disabledImage.enabled = false;
        text.enabled = false;
	}

    public void Enable()
    {
        image.enabled = true;
        disabledImage.enabled = true;
        text.enabled = true;
        enabled = true;
    }

    // Update is called once per frame
    void Update () {
        if (enabled) Ability();
	}

    void Ability()
    {
        List<GameObject> enemies = enemySpawner.GetEnemies();
        foreach (GameObject go in enemies) go.GetComponent<Enemy>().GetHit(damage);
        Recharge();
    }

    public void Recharge()
    {
        //rechargeTime = _rechargeTime;

        if (currentCoroutine == null)
        {
            currentCoroutine = RechargeCoroutine();
            StartCoroutine(RechargeCoroutine());
        }
    }

    public IEnumerator RechargeCoroutine()
    {
        enabled = false;
        disabledImage.fillAmount = 1;
        while (disabledImage.fillAmount > 0)
        {
            disabledImage.fillAmount -= Time.deltaTime / rechargeTime;
            yield return null;
        }
        currentCoroutine = null;
        enabled = true;
        //recharging = false;
    }
}
