using UnityEngine;

public class MiniEnemyController : EnemyController
{
	[SerializeField] private float moveSpeed = 3f;
	[SerializeField] private float lifeTime = 5f; // Th?i gian t?n t?i t?i ?a (5 gi�y)

	private float timer = 0f;

	protected override void Update()
	{
		//base.Update();

		// Di chuy?n d� theo Player
		if (player != null)
		{
			Vector2 direction = (player.transform.position - transform.position).normalized;
			transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
		}

		// ??m th?i gian s?ng
		timer += Time.deltaTime;
		if (timer >= lifeTime)
		{
			Destroy(gameObject); // T? hu? sau 5 gi�y
		}
	}

	public void SetAsMiniEnemy()
	{
		// KH�NG c?n set scale n?a, v� ?� set l�c spawn

		// Gi?m m�u c�n 1 ph?n (v� d? 25% m�u g?c)
		maxHp *= 0.25f;
		currentHp = maxHp;

		// Gi?m s�t th??ng ho?c t?c ?? n?u c?n (tu? b?n)
		enemySpeed = moveSpeed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			player.TakeDamage(enterDamage);
		}
	}
}
