using UnityEngine;

public class EnergyEnemyController : EnemyController
{
    [SerializeField] private GameObject energyObject;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackDistance = 1.5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip takeHitSound;
    [SerializeField] private AudioClip dieSound;

    private Animator animator;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    protected bool isDead = false;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (isDead) return;
        base.Update();

        if (player != null && !isAttacking)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < attackDistance && Time.time - lastAttackTime > attackCooldown)
            {
                Attack();
            }
        }
    }

    protected override void MoveToPlayer()
    {
        if (player != null && !isAttacking && !isDead)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer > attackDistance * 0.8f)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
            }
            FlipEnemy();
        }
    }

    private void Attack()
    {
        if (isDead) return;
        isAttacking = true;
        lastAttackTime = Time.time;
        animator?.SetTrigger("Attack");

        // Phát âm thanh Attack
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDead)
        {
            player.TakeDamage(enterDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDead && isAttacking)
        {
            player.TakeDamage(stayDamage);
        }
    }

    public override void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        base.TakeDamage(damage, knockbackDirection);
        if (currentHp > 0)
        {
            animator?.SetTrigger("TakeHit");

            // Phát âm thanh Take Hit
            if (takeHitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(takeHitSound);
            }
        }
    }

    protected override void Die()
    {
        isDead = true;
        animator?.SetTrigger("Die");

        // Phát âm thanh Die
        if (dieSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(dieSound);
        }

        if (energyObject != null)
        {
            GameObject energy = Instantiate(energyObject, transform.position, Quaternion.identity);
            Destroy(energy, 5f);
        }

        Destroy(gameObject, 2f);
    }
}
