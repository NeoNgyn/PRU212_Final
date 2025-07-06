using UnityEngine;

public class BanditEnemy : EnemyController
{
    private Animator animator;
    private bool isAttacking = false;
    private float attackCooldown = 1f;
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
            if (distance < 1.5f && Time.time - lastAttackTime > attackCooldown)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        if (isDead) return;
        isAttacking = true;
        lastAttackTime = Time.time;

        animator?.SetTrigger("Attack");

        // Gây sát thương sau 0.5s (tùy theo frame animation)
        Invoke(nameof(DealDamageToPlayer), 0.3f);

        // Cho phép attack lại sau cooldown
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void DealDamageToPlayer()
    {
        if (player != null && Vector2.Distance(transform.position, player.transform.position) < 1.5f)
        {
            player.TakeDamage(enterDamage); // Hoặc attackDamage riêng
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public override void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        base.TakeDamage(damage, knockbackDirection); 
        if (currentHp > 0)
        {
            animator?.SetTrigger("TakeHit");
        }
    }

    protected override void Die()
    {
        isDead = true;
        animator?.SetTrigger("Die");
        Destroy(gameObject, 2f);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        player.TakeDamage(enterDamage);
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        player.TakeDamage(stayDamage);
    //    }
    //}
}
