using UnityEngine;

public class ExplosionEnemyController : EnemyController
{
    [SerializeField] private GameObject explosionPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Die();
        }
    }

    private void CreateExplosion()
    {
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    protected override void Die()
    {
        CreateExplosion();
        base.Die();
    }
    
}
