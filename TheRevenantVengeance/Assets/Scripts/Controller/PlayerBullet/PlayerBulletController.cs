using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBulletController : MonoBehaviour
{
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
                enemy.TakeDamage(damage);
                GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
                Destroy(blood, 1f);
            }
            Destroy(gameObject);
        }
    }

    void BulletMovement()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }


}
