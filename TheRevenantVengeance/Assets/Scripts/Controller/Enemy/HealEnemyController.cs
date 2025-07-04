using UnityEngine;

public class HealEnemyController : EnemyController
{
    [SerializeField] private GameObject heartObject;
    [SerializeField] private float healValue = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(enterDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamage(stayDamage);
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

    private void HealPlayer()
    {
        if (player != null)
            player.Heal(healValue);
    }
}
