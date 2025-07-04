using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBulletController : MonoBehaviour
{
	//[SerializeField] private float moveSpeed = 25f;
	//[SerializeField] private float destroyTime = 0.5f;
	//[SerializeField] private float damage = 10f;
	//[SerializeField] GameObject bloodPrefab;

	//void Start()
	//{
	//    Destroy(gameObject, destroyTime);
	//}


	//void Update()
	//{
	//    BulletMovement();
	//}

	//private void OnTriggerEnter2D(Collider2D collision)
	//{
	//    if (collision.CompareTag("Enemy"))
	//    {
	//        EnemyController enemy = collision.GetComponent<EnemyController>();
	//        if (enemy != null)
	//        {
	//            enemy.TakeDamage(damage);
	//            GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
	//            Destroy(blood, 1f);
	//        }
	//        Destroy(gameObject);
	//    }
	//}

	//void BulletMovement()
	//{
	//    transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
	//}

	[SerializeField] private float moveSpeed = 25f;
	[SerializeField] private float destroyTime = 0.5f;
	[SerializeField] private float damage = 10f;
	[SerializeField] GameObject bloodPrefab;

	void Start()
	{
		Destroy(gameObject, destroyTime);
	}

	void Update()
	{
		BulletMovement();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			EnemyController enemy = collision.GetComponent<EnemyController>();
			if (enemy != null)
			{
				// --- THAY ??I D�NG N�Y ?? TH�M H??NG ??Y L�I ---
				// T�nh to�n h??ng ??y l�i t? vi�n ??n ??n Enemy
				Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;

				// G?i TakeDamage v?i s�t th??ng v� h??ng ??y l�i
				enemy.TakeDamage(damage, knockbackDirection);
				// EnemyController.TakeDamage ?� nh?n Vector2 v� t? nh�n v?i knockbackForce ri�ng c?a enemy.

				// T?o hi?u ?ng m�u
				GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
				Destroy(blood, 1f);
			}
			Destroy(gameObject); // H?y vi�n ??n sau khi va ch?m
		}
	}

	void BulletMovement()
	{
		transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
	}
}
