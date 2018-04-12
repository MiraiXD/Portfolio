using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float speed = 1;
    Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;
    float fadeOutDuration = 1.5f;

    
    public bool alive = true;
    public bool boy;
    PlayerAbilities playerAbilities;

    public AnimationClip sadAnimation;
    Animator animator;

    public enum Enemies
    {
        EnemyEasy,
        EnemyMedium,
        EnemyHard
    }
    public Enemies enemyType;
    //int[] coinsForEnemyType = { 50, 200, 500 };
    int coinReward;
    int maxLives;
    int currentLives;
    // Use this for initialization
    void Start () {
        playerAbilities = FindObjectOfType<PlayerAbilities>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(speed, 0);
        Initialize();

	}

    private void Initialize()
    {
        switch (enemyType)
        {
            case Enemies.EnemyEasy:
                coinReward = 50;
                maxLives = 1;
                break;
            case Enemies.EnemyMedium:
                coinReward = 200;
                maxLives = 2;
                break;
            case Enemies.EnemyHard:
                coinReward = 500;
                maxLives = 3;
                break;
        }
        currentLives = maxLives;
    }

    public void GetHit(int livesTaken)
    {
        if (alive)
        {
            currentLives -= livesTaken;
            if (currentLives <= 0)
                Die();
        }
    }

    void Die()
    {
        rigidbody.velocity = new Vector2(speed / 2, 0);
        alive = false;        
        playerAbilities.AddCoins(coinReward);
        StartCoroutine(SadAnimationCoroutine());
    }

    IEnumerator SadAnimationCoroutine()
    {
        animator.SetTrigger("killedTrigger");
        yield return new WaitForSeconds(sadAnimation.length);
        FlipDirection();
    }

    void FlipDirection()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        rigidbody.velocity = new Vector2(-speed, 0);
    }

    void Finish()
    {
        alive = false;
        FindObjectOfType<Player>().NewFriend();        
        StartCoroutine(FadeOutCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "HouseBorder")
        {
            Finish();
        }
    }

    IEnumerator FadeOutCoroutine()
    {
        float startTime = Time.time;
        while(spriteRenderer.color.a > 0)
        {            
            float t = (Time.time - startTime) / fadeOutDuration;
            spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, t));
            yield return null;
        }
        Destroy(gameObject);
    }

}
