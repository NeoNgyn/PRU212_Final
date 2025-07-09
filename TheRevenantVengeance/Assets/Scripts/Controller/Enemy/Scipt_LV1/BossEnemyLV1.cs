using UnityEngine;

public class BossEnemyLV1 : EnemyController
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform patrolPointLeft;
    [SerializeField] private Transform patrolPointRight;
    [SerializeField] private float patrolSpeed = 1f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireballCooldown = 3f;
    [SerializeField] private float radialFireballCooldown = 5f;
    [SerializeField] private int radialFireballCount = 8;
    [SerializeField] private float attackAnimationDuration = 0.5f;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float regenRate = 2f;
    [SerializeField] private GateTriggerBoss gateTrigger;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private AudioClip takeHitSound;
    [SerializeField] private AudioClip dieSound;

    private float currentHealth;
    private Transform player;
    private Vector3 nextPatrolTarget;
    private float fireballTimer = 0f;
    private float radialFireballTimer = 0f;

    private Animator animator;
    private bool isAttacking = false;
    protected bool isDead = false;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        nextPatrolTarget = patrolPointRight.position;
        currentHealth = maxHealth;
    }

    protected override void Update()
    {
        if (isDead) return;
        base.Update();

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        fireballTimer -= Time.deltaTime;
        radialFireballTimer -= Time.deltaTime;

        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                FlipSprite(player.position);
                AttemptAttack();
            }
        }
        else
        {
            Patrol();
        }

        if (currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
    }

    void Patrol()
    {
        if (isAttacking || isDead) return;

        transform.position = Vector2.MoveTowards(transform.position, nextPatrolTarget, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, nextPatrolTarget) < 0.1f)
        {
            nextPatrolTarget = (nextPatrolTarget == patrolPointLeft.position) ? patrolPointRight.position : patrolPointLeft.position;
        }

        FlipSprite(nextPatrolTarget);
    }

    void AttemptAttack()
    {
        bool attackedThisFrame = false;

        if (fireballTimer <= 0f)
        {
            isAttacking = true;
            animator?.SetTrigger("Attack");

            if (attackSound != null && audioSource != null)
                audioSource.PlayOneShot(attackSound);

            Invoke(nameof(ShootFireballAtPlayer), attackAnimationDuration * 0.5f);
            fireballTimer = fireballCooldown;
            attackedThisFrame = true;
        }

        if (radialFireballTimer <= 0f && !attackedThisFrame)
        {
            isAttacking = true;
            animator?.SetTrigger("Attack");

            if (attackSound != null && audioSource != null)
                audioSource.PlayOneShot(attackSound);

            Invoke(nameof(ShootRadialFireballs), attackAnimationDuration * 0.5f);
            radialFireballTimer = radialFireballCooldown;
            attackedThisFrame = true;
        }

        if (attackedThisFrame)
        {
            Invoke(nameof(ResetAttack), attackAnimationDuration);
        }
    }

    void ShootFireballAtPlayer()
    {
        if (fireballPrefab != null && player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * 5f;
            }

            if (fireballSound != null && audioSource != null)
                audioSource.PlayOneShot(fireballSound);
        }
    }

    void ShootRadialFireballs()
    {
        if (fireballPrefab != null)
        {
            for (int i = 0; i < radialFireballCount; i++)
            {
                float angle = i * (360f / radialFireballCount);
                float radian = angle * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

                GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = direction * 4f;
                }

                if (fireballSound != null && audioSource != null)
                    audioSource.PlayOneShot(fireballSound);
            }
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void FlipSprite(Vector3 targetPos)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = targetPos.x < transform.position.x ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    public override void TakeDamage(float damage, Vector2 knockback)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHp > 0)
        {
            animator?.SetTrigger("TakeHit");

            if (takeHitSound != null && audioSource != null)
                audioSource.PlayOneShot(takeHitSound);

            base.ApplyKnockback(knockback);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;
        animator?.SetTrigger("Die");

        if (dieSound != null && audioSource != null)
            audioSource.PlayOneShot(dieSound);

        if (gateTrigger != null)
        {
            gateTrigger.OpenGate();
        }

        Destroy(gameObject, 3f);
    }
}
