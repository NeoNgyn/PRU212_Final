using UnityEngine;
using UnityEngine.UI;

public class PoisonBossEnemy : EnemyController
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform patrolPointLeft;
    [SerializeField] private Transform patrolPointRight;
    [SerializeField] private float patrolSpeed = 1f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private GameObject poisonBallPrefab;
    [SerializeField] private float fireballCooldown = 3f;
    [SerializeField] private float radialFireballCooldown = 5f;
    [SerializeField] private int radialFireballCount = 8;
    [SerializeField] private float attackAnimationDuration = 0.5f;

    [Header("Orbit Fireball Settings")]
    [SerializeField] private int orbitFireballCount = 4;
    [SerializeField] private float orbitRadius = 2f;
    [SerializeField] private float orbitSpeed = 100f;
    [SerializeField] private float orbitDuration = 5f;

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

    [SerializeField] private bool bossActivated = false;

    private Vector3 nextPatrolTarget;
    private float fireballTimer = 0f;
    private float radialFireballTimer = 0f;
    private GameObject[] orbitFireballs;
    private float orbitTimer = 0f;

    private Animator animator;
    private bool isAttacking = false;
    protected bool isDead = false;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        maxHp = maxHealth;
        currentHp = maxHp;

        nextPatrolTarget = patrolPointRight.position;
        UpdateHealthBar();
    }

    protected override void Update()
    {
        if (!bossActivated || isDead) return;
        base.Update();

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        fireballTimer -= Time.deltaTime;
        radialFireballTimer -= Time.deltaTime;

        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                FlipSprite(player.transform.position);
                AttemptAttack();
            }
        }
        else
        {
            Patrol();
        }

        if (currentHp < maxHp)
        {
            currentHp += regenRate * Time.deltaTime;
            currentHp = Mathf.Min(currentHp, maxHp);
            UpdateHealthBar();
        }

        HandleOrbitFireballs();
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
        if (isAttacking) return;

        isAttacking = true;
        animator?.SetTrigger("Attack");

        int randomSkill = Random.Range(0, 3);
        Debug.Log($"Poison Boss dùng skill {randomSkill}");

        if (randomSkill == 0 && fireballTimer <= 0f)
        {
            Invoke(nameof(ShootPoisonBallAtPlayer), attackAnimationDuration * 0.5f);
            fireballTimer = fireballCooldown;
        }
        else if (randomSkill == 1 && radialFireballTimer <= 0f)
        {
            Invoke(nameof(ShootRadialPoisonBalls), attackAnimationDuration * 0.5f);
            radialFireballTimer = radialFireballCooldown;
        }
        else if (randomSkill == 2 && orbitTimer <= 0f)
        {
            Invoke(nameof(ActivateOrbitPoisonBalls), attackAnimationDuration * 0.5f);
        }

        if (attackSound != null && audioSource != null)
            audioSource.PlayOneShot(attackSound);

        Invoke(nameof(ResetAttack), attackAnimationDuration);
    }

    void ShootPoisonBallAtPlayer()
    {
        if (poisonBallPrefab != null && player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            GameObject fireball = Instantiate(poisonBallPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * 5f;
            }

            if (fireballSound != null && audioSource != null)
                audioSource.PlayOneShot(fireballSound);
        }
    }

    void ShootRadialPoisonBalls()
    {
        if (poisonBallPrefab != null)
        {
            for (int i = 0; i < radialFireballCount; i++)
            {
                float angle = i * (360f / radialFireballCount);
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                GameObject fireball = Instantiate(poisonBallPrefab, transform.position, Quaternion.identity);
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

    void ActivateOrbitPoisonBalls()
    {
        if (poisonBallPrefab == null) return;

        orbitFireballs = new GameObject[orbitFireballCount];

        for (int i = 0; i < orbitFireballCount; i++)
        {
            float angle = i * (360f / orbitFireballCount);
            Vector2 offset = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ) * orbitRadius;

            GameObject fireball = Instantiate(poisonBallPrefab, transform.position + (Vector3)offset, Quaternion.identity);
            fireball.transform.parent = transform;
            orbitFireballs[i] = fireball;
        }

        orbitTimer = orbitDuration;
        Debug.Log("Poison Boss kích hoạt Orbit Poison Balls!");
    }

    void HandleOrbitFireballs()
    {
        if (orbitTimer > 0f)
        {
            orbitTimer -= Time.deltaTime;

            float angleOffset = orbitSpeed * Time.deltaTime;
            for (int i = 0; i < orbitFireballs.Length; i++)
            {
                if (orbitFireballs[i] != null)
                {
                    float currentAngle = Mathf.Atan2(
                        orbitFireballs[i].transform.localPosition.y,
                        orbitFireballs[i].transform.localPosition.x
                    );
                    currentAngle += angleOffset * Mathf.Deg2Rad;

                    Vector2 newPos = new Vector2(
                        Mathf.Cos(currentAngle),
                        Mathf.Sin(currentAngle)
                    ) * orbitRadius;

                    orbitFireballs[i].transform.localPosition = newPos;
                }
            }

            if (orbitTimer <= 0f)
            {
                foreach (GameObject fireball in orbitFireballs)
                {
                    if (fireball != null)
                        Destroy(fireball);
                }
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

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);

        UpdateHealthBar();

        if (currentHp > 0)
        {
            animator?.SetTrigger("TakeHit");

            if (takeHitSound != null && audioSource != null)
                audioSource.PlayOneShot(takeHitSound);

            base.ApplyKnockback(knockback);
        }

        if (currentHp <= 0)
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

    public void ActivateBoss()
    {
        bossActivated = true;
    }
}
