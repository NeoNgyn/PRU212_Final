using UnityEngine;

public class ArcherEnemy : EnemyController
{
    private Animator animator;
    private bool isAttacking = false;
    private float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;
    //protected bool isDead = false;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    protected override void Update()
    {
        if (IsDead()) return;
        base.Update();
        if (player != null && !isAttacking)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < 15f && Time.time - lastAttackTime > attackCooldown)
            {
                Shoot();
            }
        }
    }
    protected override void MoveToPlayer()
    {
        if (player == null) return;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < 5f)
        {
            Vector2 dirAway = (transform.position - player.transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position,
                                                     (Vector2)transform.position + dirAway,
                                                     enemySpeed * Time.deltaTime);
        }
        else
        {
            base.MoveToPlayer();
        }
    }
    private void Shoot()
    {
        if (IsDead()) return;
        isAttacking = true;
        lastAttackTime = Time.time;

        animator?.SetTrigger("Shoot");

        Invoke(nameof(SpawnArrow), 0.5f); 

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void SpawnArrow()
    {
        if (arrowPrefab != null && player != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.SetTarget(player.transform);
            }
            else
            {
                Debug.LogWarning("Prefab ko cc script Arrow gan vao");
            }
        }
        else
        {
            Debug.LogWarning("arrowPrefab, player hoac firePoint bi null!");
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    protected override void Die()
    {
        //isDead = true;
        animator?.SetTrigger("Death");
        base.Die();
        //Destroy(gameObject, 2f);
    }
}
