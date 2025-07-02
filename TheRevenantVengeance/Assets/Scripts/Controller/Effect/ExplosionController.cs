using UnityEngine;

public class ExplosionController : MonoBehaviour
{
	[SerializeField] private float damage = 30f;
	// --- TH�M BI?N CHO L?C ??Y L�I C?A V? N? ---
	[SerializeField] private float explosionKnockbackForce = 10f; // L?c ??y l�i khi b? n?

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// X? l� s�t th??ng cho Player (ch?a c?n knockback cho Player)
		PlayerController player = collision.GetComponent<PlayerController>();
		if (player != null && collision.CompareTag("Player")) // Ki?m tra null v� tag
		{
			player.TakeDamage(damage);
		}

		// X? l� s�t th??ng v� ??y l�i cho Enemy
		EnemyController enemy = collision.GetComponent<EnemyController>();
		if (enemy != null && collision.CompareTag("Enemy")) // Ki?m tra null v� tag
		{
			// T�nh to�n h??ng ??y l�i t? t�m v? n? ??n Enemy
			Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
			// G?i TakeDamage v?i s�t th??ng v� h??ng ??y l�i
			enemy.TakeDamage(damage, knockbackDirection * explosionKnockbackForce); // Truy?n damage v� h??ng ??y l�i
																					// L?u �: h�m TakeDamage nh?n m?t Vector2 l�m tham s? th? hai, ??i di?n cho h??ng.
																					// knockbackDirection.normalized * explosionKnockbackForce c� th? ???c s? d?ng l�m tham s? th? hai
																					// ho?c b?n c� th? ch? truy?n knockbackDirection v� ?? enemy t? nh�n v?i knockbackForce c?a n�.
																					// T�i ?� s?a EnemyController.TakeDamage ?? ch? nh?n h??ng, v� EnemyController.ApplyKnockback s? nh�n v?i knockbackForce.
																					// V?y n�n, ch? c?n truy?n knockbackDirection.
		}
	}

	public void DestroyExplosion()
	{
		Destroy(gameObject);
	}
}
