using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IDamageable, IPoisonable {

    // Max HP of the enemy
    public float maxHealth = 100;
    // Current health
    private float health;
    // Default dmg
    public float damage = 10f;
    // Is dead
    protected bool dead = false;
    // Should drop a soul after death?
    public bool dropsSoul = false;
    // Prefab of the soul to be dropped
    public GameObject soulPrefab;
    // Where to drop the soul from
    public Transform dropSpot;
    // Should patrol the area around when no enemies spotted
    public bool patrollingEnemy = true;
    // Waiting time before going back during patrolling
    public float waitingTime = 3f;
    // Patrol starting position
    Vector2 startingPos;
    // End position of the patrolling enemy
    public Transform endPos;
    Vector2 EndPos; // { get { return transform.TransformVector( new Vector2(endPos.position.x, endPos.position.y) ); } }
    // Current destination of the enemy
    Vector2 destination;
    // Animation controlling script
    protected EnemyAnimator enemyAnimator;
    // Rigibody2D of the enemy
    protected Rigidbody2D rb;
    // Normal movement speed
    public float speed = 1f;
    // Speed when chasing the player
    public float chasingSpeed = 1.8f;
    // Attack Speed
    public float attackSpeed = 1f;
    // Attack Range
    public float attackRange = 2f;
    // Cooldown between attacks
    public float attackCooldown = 2f;
    // Measures cooldown
    float attackTimer = 0f;
    // Ready to attack?
    bool canAttack = true;
    // Is on the ground. Prevents from falling off the ledge
    bool isGrounded;
    // Target to be chased down
    protected Transform target;
    // Is chasing the player?
    bool chasing = false;
    public bool canChase = true; //can chase the player and go after it
    // Waiting on patrol
    bool waiting = false;
    // Is attacking
    protected bool attacking = false;
    // Audio source of the enemy
    AudioSource enemyAudioSource;
    // Type of the enemy
    public Types.CharacterType characterType;
    // Width of the default collider
    float colliderWidth;
    // Force the player is pushed back with, at a collision
    public float pushbackForce;
    // Has dropped a soul already?
    bool droppedAlready = false;
    // Script controlling health
    private EnemyHealthController enemyHealthController;
    private bool startTimer = false;
    private float fadingTime = 0; //Uses for calculating fading out time
    // UI healthbar
    protected GameObject healthBarObject;
    private Image healthBar;    
    // When should the enemy healthBar disappear after being hit
    public float healthBarFadingTime = 3.0f;

    public float Health {
        get {
            return this.health;
        }
        set {
            this.health = value;

            //Setting health to heathBar text
            healthBar.GetComponentInChildren<Image> ().fillAmount = health / maxHealth;

            //Showing the healthbar when taking damage
            enemyHealthController.FadingIN (0.15f);
            fadingTime = Time.time + healthBarFadingTime;
            startTimer = true;
        }
    }

    // Poison effect applied by eel-whip
    Coroutine poisonCoroutine;

    protected virtual void Awake () {
        enemyAnimator = GetComponent<EnemyAnimator> ();
        rb = GetComponent<Rigidbody2D> ();
        enemyAudioSource = GetComponent<AudioSource> ();
        startingPos = rb.position;

        if (canChase) //if enemy can chase the player
            EndPos = new Vector2 (endPos.position.x, endPos.position.y);

        if (patrollingEnemy && canChase) destination = EndPos;

        if (GetComponent<CapsuleCollider2D> () != null)
            colliderWidth = GetComponent<CapsuleCollider2D> ().bounds.extents.x;
    }
    protected virtual void Start () {
        FloatingTextController.Initialize ();
        SetHealth ();
    }

    void SetHealth () {
        //Setting healthbar
        healthBarObject = Resources.Load ("cans") as GameObject;
        GameObject instance = Instantiate (healthBarObject, transform.position, transform.rotation);

        instance.transform.SetParent (gameObject.transform, true);

        //Finding heatlh bar from Resources
        healthBar = transform.Find ("cans(Clone)/healthbar/Image").GetComponent<Image> ();
        enemyHealthController = healthBar.GetComponent<EnemyHealthController> ();
        enemyHealthController.LocateBars (task => {
            Health = maxHealth;
        });
    }
    void CheckIfGrounded () {
        Vector2 raycastPos;
        if (enemyAnimator.FacingRight)
            raycastPos = transform.position + colliderWidth * transform.right;
        else
            raycastPos = transform.position - colliderWidth * transform.right;
        isGrounded = Physics2D.Linecast (raycastPos, raycastPos + Vector2.down, 1 << LayerMask.NameToLayer ("Floor") | 1 << LayerMask.NameToLayer ("Platform"));
        Debug.DrawLine (raycastPos, raycastPos + Vector2.down);
    }

    protected virtual void Update () {
        CheckIfGrounded ();
        if (fadingTime < Time.time && startTimer) //If last time enemy take demage is over waiting time it fades away
        {
            startTimer = false;
            enemyHealthController.FadingOut (0.5f);
        }
    }

    protected virtual void FixedUpdate () {
        if (dead) return;
        if (!isGrounded) {
            if (chasing) {
                FaceTarget ();
                //rb.velocity = new Vector2(0, rb.velocity.y);
                SetVelocity (0f);
                return;
            }
        }
        if (!enemyAnimator.CanWalk) return;
        if (chasing) {
            float dir = target.position.x > rb.position.x ? 1f : -1f;
            float distance = Mathf.Abs (target.position.x - rb.position.x); //Vector2.Distance(target.position, rb.position);
            if (distance > attackRange)
                //rb.velocity = new Vector2(dir * chasingSpeed, rb.velocity.y);
                SetVelocity (dir * chasingSpeed);
            else {
                //rb.velocity = new Vector2(0, rb.velocity.y);
                SetVelocity (0f);
                Attack ();
            }
        } else {
            if (!patrollingEnemy) return;
            if (waiting) return;
            if (Mathf.Abs (destination.x - rb.position.x) > 0.1f) {
                float dir = destination.x > rb.position.x ? 1f : -1f;
                //rb.velocity = new Vector2(dir * speed, rb.velocity.y);
                SetVelocity (dir * speed);
            } else {
                //rb.velocity = new Vector2(0, rb.velocity.y);
                SetVelocity (0f);
                StartCoroutine (WaitAndReturn ());
            }
        }
    }

    void SetVelocity (float velocity) {
        if (rb.velocity.x == 0f && velocity != 0f) {
            if (!enemyAudioSource.isPlaying)
                EnemySound (Types.SoundType.Walk, true);
        } else if (rb.velocity.x != 0f && velocity == 0f) {
            enemyAudioSource.Stop ();
        }
        rb.velocity = new Vector2 (velocity, rb.velocity.y);
    }

    IEnumerator AttackCooldown () {
        canAttack = false;
        while (attackTimer < attackCooldown) {
            attackTimer += Time.deltaTime;
            yield return null;
        }
        canAttack = true;
        attackTimer = 0f;
    }

    // On patrol
    IEnumerator WaitAndReturn () {
        waiting = true;
        yield return new WaitForSeconds (waitingTime);
        destination = Vector2.Distance (rb.position, startingPos) < Vector2.Distance (rb.position, EndPos) ? EndPos : startingPos;
        waiting = false;
    }

    private void OnEnable () {
        if (waiting) {
            StartCoroutine (WaitAndReturn ());
            enemyAudioSource.Stop ();
        }
    }

    protected virtual void OnTriggerEnter2D (Collider2D collision) {
        if (collision.tag == "Player") {
            StartChasing (collision.transform);
        }
    }

    //protected virtual void OnTriggerStay2D(Collider2D collision)
    //{

    //        if (collision.tag == "Player")
    //        {
    //            StartChasing(collision.transform);
    //        }
    //}    

    private void OnTriggerExit2D (Collider2D collision) {
        if (collision.tag == "Player") {
            StopChasing ();
        }
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            SetVelocity (0f);
            Rigidbody2D otherRB = collision.gameObject.GetComponent<Rigidbody2D> ();
            //Vector2 dir = otherRB.position - rb.position;
            Vector2 dir = new Vector2 (1, 1);
            //collision.otherRigidbody.AddForce(dir * pushbackForce, ForceMode2D.Impulse);                     
            otherRB.velocity = dir * pushbackForce;
            collision.gameObject.GetComponent<PlayerController> ().Damage (damage);
        }
    }

    protected virtual void StartChasing (Transform _target) {
        chasing = true;
        target = _target;
    }
    protected void StopChasing () {
        chasing = false;
        target = null;
    }

    protected virtual void Attack () {
        if (!canAttack) return;
        attacking = true;
        FaceTarget ();
        enemyAnimator.onAttackEvent += OnAttackEvent;
        enemyAnimator.onAnimFinishedEvent += OnAnimFinished;
        enemyAnimator.AttackAnim (attackSpeed);
    }

    protected virtual void FaceTarget () {
        if ((rb.position.x > target.position.x && enemyAnimator.FacingRight) || (rb.position.x < target.position.x && !enemyAnimator.FacingRight))
            enemyAnimator.Flip ();
    }
    public void Die (bool falldeath = false) {
        dead = true;
        enemyAnimator.Dead = true;
        StopChasing ();
        enemyAnimator.onAnimFinishedEvent += OnAnimFinished;
        enemyAnimator.DeathAnim ();
        rb.isKinematic = true;

        if (dropsSoul && !droppedAlready) {
            droppedAlready = true;
            Drop ();
        }
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D> ()) {
            collider.enabled = false;
        }
        EnemySound (Types.SoundType.Death, false);

    }

    public virtual void Damage (float _damage, bool playWoundAnim = true) {
        SetVelocity (0f);

        FloatingTextController.CreateFloatingText (_damage, transform);
        //if (attacking) InterruptAttack();
        Health -= _damage;
        if (Health <= 0f) {
            if (gameObject.GetComponent<SporeMovement> () != null) gameObject.GetComponent<SporeMovement> ().DeacreaseSpore ();

            Die ();
        } else {
            if (playWoundAnim) {
                enemyAnimator.WoundAnim ();
                EnemySound (Types.SoundType.Hit, false);
            }
        }
    }
    void EnemySound (Types.SoundType soundType, bool loop) {
        if (enemyAudioSource.isPlaying) enemyAudioSource.Stop ();
        SoundController.Instance.EnemySound (characterType, soundType, enemyAudioSource, loop);
    }

    void OnAnimFinished (Types.AnimType animType) {
        enemyAnimator.onAnimFinishedEvent -= OnAnimFinished;
        if (animType == Types.AnimType.Death) {
            Destroy (gameObject);
        } else if (animType == Types.AnimType.Attack) {
            StartCoroutine (AttackCooldown ());
            attacking = false;
        }
    }

    protected void OnAttackEvent (bool attackBeginEvent) {
        if (attackBeginEvent) OnAttackBegin ();
        else OnAttackEnd ();
    }

    protected virtual void OnAttackBegin () {
        EnemySound (Types.SoundType.Attack, false);
    }

    protected virtual void OnAttackEnd () {
        enemyAnimator.onAttackEvent -= OnAttackEvent;
    }

    void Drop () {
        if (dropSpot != null)
            Instantiate (soulPrefab, dropSpot);
        else
            Instantiate (soulPrefab, new Vector3 (transform.position.x, transform.position.y + 2f, transform.position.z), transform.rotation);
    }

    public void Poison (float poisonDamage, float poisonTime) {
        if (poisonCoroutine != null) StopCoroutine (poisonCoroutine);
        poisonCoroutine = StartCoroutine (PoisonCoroutine (poisonDamage, poisonTime));

    }

    IEnumerator PoisonCoroutine (float poisonDamage, float poisonTime) {
        float poisonInterval = 0.2f;
        float damagePortion = poisonDamage / (poisonTime / poisonInterval);
        int damageCounter = 1;
        float t = 0f;
        while (t < poisonTime) {
            t += Time.deltaTime;
            if (t >= damageCounter * poisonInterval) {
                damageCounter++;
                Damage (damagePortion, false); // Don't play woundAnim, cause it would be happening too often
                if (dead) StopCoroutine (poisonCoroutine);
            }
            yield return null;
        }
    }

    protected void InterruptAttack () {
        OnAttackEnd ();
        OnAnimFinished (Types.AnimType.Attack);
    }

}