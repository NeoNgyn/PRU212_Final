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


	[SerializeField] protected float knockbackForce = 5f; // L?c ??y l�i
	[SerializeField] protected float knockbackDuration = 0.2f; // Th?i gian ??y l�i
	protected bool isKnockedBack = false; // C? ?? ki?m so�t tr?ng th�i ??y l�i
	protected Rigidbody2D rb; // Tham chi?u ??n Rigidbody2D c?a k? ??ch

	private bool isDead = false;
	protected virtual void Awake()
	{
		// L?y tham chi?u Rigidbody2D c?a k? ??ch
		rb = GetComponent<Rigidbody2D>();
		if (rb == null)
		{
			Debug.LogWarning($"Enemy {gameObject.name} is missing a Rigidbody2D component, knockback will not work.", this);
		}

		// T�m tham chi?u ??n PlayerController ngay trong Awake
		player = FindAnyObjectByType<PlayerController>();
		if (player == null)
		{
			Debug.LogWarning($"PlayerController not found for enemy {gameObject.name} in Awake!", this);
		}
	}
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

    protected virtual void MoveToPlayer()
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

	//public virtual void TakeDamage(float damage)
	//{
	//    currentHp -= damage;
	//    currentHp = Mathf.Max(currentHp, 0);
	//    UpdateHealthBar();

	//    if (currentHp <= 0)
	//        Die();
	//}

	public virtual void TakeDamage(float damage, Vector2 knockbackDirection) // Th�m tham s? knockbackDirection
	{
        if (isDead) return;
        currentHp -= damage;
		currentHp = Mathf.Max(currentHp, 0);
		UpdateHealthBar();

		if (currentHp <= 0)
		{
			Die();
		}
		else // Ch? ??y l�i n?u ch?a ch?t
		{
			ApplyKnockback(knockbackDirection);
		}
	}

	// --- H�M �P D?NG ??Y L�I ---
	protected void ApplyKnockback(Vector2 direction)
	{
		// Ki?m tra rb kh�ng null v� kh�ng ph?i l� Static Rigidbody
		if (rb != null && rb.bodyType != RigidbodyType2D.Static)
		{
			isKnockedBack = true;
			// X�a b? v?n t?c hi?n t?i ?? l?c ??y l�i kh�ng b? ?nh h??ng b?i v?n t?c tr??c ?�
			rb.linearVelocity = Vector2.zero;
			// �p d?ng l?c ??y l�i t?c th�
			rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);

			// D?ng ??y l�i sau m?t kho?ng th?i gian
			Invoke("StopKnockback", knockbackDuration);
		}
	}

	// --- H�M D?NG ??Y L�I ---
	protected void StopKnockback()
	{
		isKnockedBack = false;
		if (rb != null)
		{
			rb.linearVelocity = Vector2.zero; // ??m b?o d?ng m?i v?n t?c c�n l?i sau khi ??y l�i
		}
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
		isDead = true;
        Destroy(gameObject, 2f);
    }
    public bool IsDead()
    {
        return isDead;
    }
}
