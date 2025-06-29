using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] protected float enemySpeed = 1f;
    protected PlayerController player;

    [SerializeField] protected float maxHp = 50f;
    protected float currentHp;
    [SerializeField] private Image healthBar;
    [SerializeField] protected float enterDamage = 10f;
    [SerializeField] protected float stayDamage = 10f;



    protected virtual void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        currentHp = maxHp;
        UpdateHealthBar();
    }

    protected virtual void Update()
    {
        MoveToPlayer();
    }

    protected void MoveToPlayer()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
            FlipEnemy();
        }
    }

    protected void FlipEnemy()
    {
        if (player != null)
            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHealthBar();

        if (currentHp <= 0)
            Die();
    }

    protected void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHp / maxHp;
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
