using UnityEngine;

public class AttackDetector : MonoBehaviour
{
	[SerializeField] private float attackDamage = 10f; // S�t th??ng c?a ?�n ch�m

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Ki?m tra xem ??i t??ng va ch?m c� Tag "Enemy" kh�ng
		//if (collision.CompareTag("Enemy"))
		//{
		//	// L?y component EnemyController t? ??i t??ng va ch?m
		//	// V� t?t c? c�c k? ??ch c?a b?n ??u k? th?a/l� EnemyController, ch�ng ta c� th? l?y component n�y.
		//	EnemyController enemy = collision.GetComponent<EnemyController>();

		//	if (enemy != null)
		//	{
		//		// G?i h�m TakeDamage tr�n ??i t??ng Enemy
		//		enemy.TakeDamage(attackDamage);
		//	}
		//	// else
		//	// {
		//	//     Debug.LogWarning($"Enemy with tag 'Enemy' on {collision.gameObject.name} does not have an EnemyController component!");
		//	// }
		//}
		if (collision.CompareTag("Enemy"))
		{
			// L?y component EnemyController t? ??i t??ng va ch?m
			// D� l� HealEnemyController hay BasicEnemyController, n?u ch�ng k? th?a t? EnemyController,
			// th� GetComponent<EnemyController>() v?n s? tr? v? ?�ng component c?a l?p con.
			EnemyController enemy = collision.GetComponent<EnemyController>();

			if (enemy != null)
			{
				// --- T�NH TO�N H??NG ??Y L�I ---
				// L?y h??ng m� Player ?ang quay m?t ?? ??y l�i k? ??ch
				// Gi? ??nh GameObject cha c?a AttackHitbox l� Player
				Vector2 knockbackDirection;
				if (transform.parent.localScale.x < 0) // Player quay m?t sang tr�i
				{
					knockbackDirection = new Vector2(-1, 0); // ??y k? ??ch sang tr�i
				}
				else // Player quay m?t sang ph?i
				{
					knockbackDirection = new Vector2(1, 0); // ??y k? ??ch sang ph?i
				}

				// T�y ch?n: N?u b?n mu?n ??y l�i c� th�m m?t ch�t "n?y" l�n tr�n
				// knockbackDirection = new Vector2(knockbackDirection.x, 0.5f).normalized; 

				// G?i h�m TakeDamage tr�n ??i t??ng Enemy, truy?n s�t th??ng v� h??ng ??y l�i
				enemy.TakeDamage(attackDamage, knockbackDirection);
			}
			// else
			// {
			//     Debug.LogWarning($"Enemy with tag 'Enemy' on {collision.gameObject.name} does not have an EnemyController component!", collision.gameObject);
			// }
		}
	}
}