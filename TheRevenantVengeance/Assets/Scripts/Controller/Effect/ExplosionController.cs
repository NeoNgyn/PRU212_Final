using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private float damage = 30f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        EnemyController enemy = collision.GetComponent<EnemyController>();
        if (collision.CompareTag("Player"))
            player.TakeDamage(damage);

        if (collision.CompareTag("Enemy"))
            enemy.TakeDamage(damage);
    }

    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
