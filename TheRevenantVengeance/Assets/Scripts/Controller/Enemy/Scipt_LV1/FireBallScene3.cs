using UnityEngine;

public class FireBallScene3: MonoBehaviour
{
	[SerializeField] private float fireballDamage = 15f; // Sát thương cầu lửa gây ra
	[SerializeField] private float playerKnockbackForce = 500f; // Lực đẩy người chơi ra xa

	// Hàm này được gọi khi collider của cầu lửa va chạm với một collider khác (Is Trigger phải true)
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Debug.Log($"Fireball va chạm với: {collision.name} (Tag: {collision.tag})");

		if (collision.CompareTag("Player")) // Kiểm tra xem có phải là người chơi không
		{
			// Debug.Log("Fireball trúng Player!");

			PlayerController playerScript = collision.GetComponent<PlayerController>();
			if (playerScript != null)
			{
				// Gây sát thương cho người chơi
				playerScript.TakeDamage(fireballDamage);

				// Tính toán hướng đẩy lùi (ngược lại với hướng va chạm hoặc từ tâm fireball đến player)
				Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
				// Nếu muốn đẩy theo hướng ngược lại với hướng bay của fireball:
				// Vector2 knockbackDirection = -GetComponent<Rigidbody2D>().linearVelocity.normalized;

				// Áp dụng lực đẩy lùi lên Rigidbody2D của người chơi
				Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
				if (playerRb != null)
				{
					playerRb.AddForce(knockbackDirection * playerKnockbackForce, ForceMode2D.Impulse);
				}
			}
			// Hủy cầu lửa sau khi va chạm (để nó không bay xuyên qua)
			Destroy(gameObject);
		}
		// Tùy chọn: Hủy cầu lửa nếu va chạm với môi trường (ví dụ: tường có tag "Ground")
		// else if (collision.CompareTag("Ground"))
		// {
		//     Destroy(gameObject);
		// }
	}
}
