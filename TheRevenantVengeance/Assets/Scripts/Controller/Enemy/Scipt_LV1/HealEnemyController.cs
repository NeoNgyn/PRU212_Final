//using UnityEngine;

//public class HealEnemyController : EnemyController
//{
//    [SerializeField] private GameObject heartObject;
//    [SerializeField] private float healValue = 20f;

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            player.TakeDamage(enterDamage);
//        }
//    }

//    private void OnTriggerStay2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            player.TakeDamage(stayDamage);
//        }
//    }

//    protected override void Die()
//    {
//        if (heartObject != null)
//        {
//            GameObject heart = Instantiate(heartObject, transform.position, Quaternion.identity);
//            Destroy(heart, 5f);
//        }

//        base.Die();
//    }

//    private void HealPlayer()
//    {
//        if (player != null)
//            player.Heal(healValue);
//    }
//}

using UnityEngine;

public class HealEnemyController : EnemyController
{
	[SerializeField] private GameObject heartObject;
	[SerializeField] private GameObject fireballPrefab;
	[SerializeField] private float healValue = 20f;
	[SerializeField] private float fireRate = 2f;
	[SerializeField] private float fireSpeed = 5f;
	[SerializeField] private float stoppingDistance = 5f; // Khoảng cách dừng lại bắn

	private float fireCooldown;

	protected override void Update()
	{
		base.Update();

		if (player != null)
		{
			float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

			// Nếu trong khoảng bắn thì bắn fireball
			if (distanceToPlayer <= stoppingDistance)
			{
				fireCooldown -= Time.deltaTime;
				if (fireCooldown <= 0f)
				{
					ShootFireball();
					fireCooldown = fireRate;
				}
			}
		}
	}

	protected override void MoveToPlayer()
	{
		if (player != null)
		{
			float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

			// Chỉ di chuyển nếu còn quá xa player
			if (distanceToPlayer > stoppingDistance)
			{
				transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);
			}

			FlipEnemy();
		}
	}

	private void ShootFireball()
	{
		if (fireballPrefab != null)
		{
			GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
			Vector2 direction = (player.transform.position - transform.position).normalized;
			Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
			if (rb != null)
			{
				rb.linearVelocity = direction * fireSpeed;
			}
		}
	}

	protected override void Die()
	{
		if (heartObject != null)
		{
			GameObject heart = Instantiate(heartObject, transform.position, Quaternion.identity);
			Destroy(heart, 5f);
		}

		base.Die();
	}
}

