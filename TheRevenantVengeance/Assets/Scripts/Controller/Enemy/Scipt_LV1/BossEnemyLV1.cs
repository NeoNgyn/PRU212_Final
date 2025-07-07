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

	[Header("Health Settings")]
	[SerializeField] private float maxHealth = 100f;
	[SerializeField] private float regenRate = 2f;
	[SerializeField] private GateTriggerBoss gateTrigger;

	private float currentHealth;
	private Transform player;
	private Vector3 nextPatrolTarget;
	private float fireballTimer = 0f;
	private float radialFireballTimer = 0f;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player")?.transform;
		nextPatrolTarget = patrolPointRight.position;
		currentHealth = maxHealth;
		Debug.Log("Boss spawned, HP: " + currentHealth);
	}

	private void Update()
	{
		if (player == null) return;

		float distanceToPlayer = Vector2.Distance(transform.position, player.position);
		fireballTimer -= Time.deltaTime;
		radialFireballTimer -= Time.deltaTime;

		if (distanceToPlayer <= attackRange)
		{
			AttackPlayer();
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
		transform.position = Vector2.MoveTowards(transform.position, nextPatrolTarget, patrolSpeed * Time.deltaTime);

		if (Vector2.Distance(transform.position, nextPatrolTarget) < 0.1f)
		{
			nextPatrolTarget = (nextPatrolTarget == patrolPointLeft.position) ? patrolPointRight.position : patrolPointLeft.position;
		}

		FlipSprite(nextPatrolTarget);
	}

	void AttackPlayer()
	{
		FlipSprite(player.position);

		if (fireballTimer <= 0f)
		{
			ShootFireballAtPlayer();
			fireballTimer = fireballCooldown;
		}

		if (radialFireballTimer <= 0f)
		{
			ShootRadialFireballs();
			radialFireballTimer = radialFireballCooldown;
		}
	}

	void ShootFireballAtPlayer()
	{
		if (fireballPrefab != null)
		{
			Vector2 direction = (player.position - transform.position).normalized;
			GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
			Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
			if (rb != null)
			{
				rb.linearVelocity = direction * 5f;
			}
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
			}
		}
	}

	void FlipSprite(Vector3 targetPos)
	{
		Vector3 localScale = transform.localScale;
		localScale.x = targetPos.x < transform.position.x ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x);
		transform.localScale = localScale;
	}

	public override void TakeDamage(float damage, Vector2 knockback)
	{
		Debug.Log($"Boss nhận {damage} damage, HP trước khi trừ: {currentHealth}"); // Hoặc currentHp nếu bạn đã bỏ biến riêng

		currentHealth -= damage; // Hoặc currentHp -= damage;
		currentHealth = Mathf.Max(currentHealth, 0); // Hoặc currentHp = Mathf.Max(currentHp, 0);

		// Nếu bạn có thanh máu riêng cho boss, hãy cập nhật ở đây.
		// Nếu bạn dùng thanh máu của EnemyController, hãy gọi base.UpdateHealthBar();
		// Ví dụ: base.UpdateHealthBar();

		Debug.Log("HP sau khi trừ: " + currentHealth); // Hoặc currentHp

		if (currentHealth <= 0) // Hoặc currentHp
		{
			Debug.Log("Boss chết, gọi Die()");
			Die();
		}
		else
		{
			// *** ĐẢM BẢO CHỈ GỌI DÒNG NÀY ***
			base.ApplyKnockback(knockback); // Sử dụng hệ thống knockback của EnemyController
		}
		// *** KHÔNG CÓ DÒNG NÀO KIỂU NHƯ:
		// Rigidbody2D rb = GetComponent<Rigidbody2D>();
		// if (rb != null) { rb.AddForce(knockback * 200f); }
		// Ở ĐÂY NỮA! ***
	}

	void Die()
	{
		Debug.Log("Boss đã chết, mở cổng!");
		if (gateTrigger != null)
		{
			gateTrigger.OpenGate();
		}
		else
		{
			Debug.LogWarning("GateTrigger chưa được gán vào BossEnemy!");
		}

		Destroy(gameObject);
	}
}

