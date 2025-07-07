using UnityEngine;

public class ExplosionEnemyController : EnemyController
{
	[SerializeField] private GameObject miniEnemyPrefab;
	[SerializeField] private int spawnCount = 3;
	[SerializeField] private float spawnCooldown = 8f;

	private float cooldownTimer = 0f;

	protected override void Update()
	{
		base.Update();

		cooldownTimer -= Time.deltaTime;

		float distance = Vector2.Distance(transform.position, player.transform.position);
		if (distance <= 6f && cooldownTimer <= 0f)
		{
			SpawnMiniEnemies();
			cooldownTimer = spawnCooldown;
		}
	}

	private void SpawnMiniEnemies()
	{
		for (int i = 0; i < spawnCount; i++)
		{
			GameObject mini = Instantiate(miniEnemyPrefab, transform.position, Quaternion.identity);
			mini.transform.localScale = new Vector3(0.15f, 0.15f, 1f);

			MiniEnemyController miniScript = mini.GetComponent<MiniEnemyController>();
			if (miniScript != null)
			{
				miniScript.SetAsMiniEnemy();
			}
		}
	}

}
//using UnityEngine;

//public class TeleportExplodeEnemyController : EnemyController
//{
//	[SerializeField] private GameObject explosionPrefab;
//	[SerializeField] private float teleportInterval = 5f;  // Kho?ng th?i gian gi?a m?i l?n teleport
//	[SerializeField] private float teleportDistance = 4f;  // Kho?ng cách quanh Player khi teleport
//	[SerializeField] private float explosionDelay = 2f;    // Th?i gian ch? tr??c khi n?

//	private float teleportCooldown;
//	private float explosionCountdown;
//	private bool isPreparingToExplode = false;

//	protected override void Update()
//	{
//		base.Update();

//		if (isPreparingToExplode)
//		{
//			// ?ang ch? n?
//			explosionCountdown -= Time.deltaTime;
//			if (explosionCountdown <= 0f)
//			{
//				Explode();
//			}
//		}
//		else
//		{
//			// ??i ??n lúc teleport
//			teleportCooldown -= Time.deltaTime;
//			if (teleportCooldown <= 0f)
//			{
//				TeleportNearPlayer();
//			}
//		}
//	}

//	private void TeleportNearPlayer()
//	{
//		if (player != null)
//		{
//			Vector2 randomDirection = Random.insideUnitCircle.normalized;
//			Vector2 targetPosition = (Vector2)player.transform.position + randomDirection * teleportDistance;
//			transform.position = targetPosition;

//			// B?t ??u ??m ng??c n?
//			isPreparingToExplode = true;
//			explosionCountdown = explosionDelay;
//		}

//		teleportCooldown = teleportInterval; // Reset cooldown (n?u mu?n teleport l?n n?a sau khi n?)
//	}

//	private void Explode()
//	{
//		// N?
//		if (explosionPrefab != null)
//		{
//			Instantiate(explosionPrefab, transform.position, Quaternion.identity);
//		}

//		Die();
//	}

//	protected override void Die()
//	{
//		base.Die(); // G?i Die() g?c, s? Destroy(gameObject)
//	}
//}

