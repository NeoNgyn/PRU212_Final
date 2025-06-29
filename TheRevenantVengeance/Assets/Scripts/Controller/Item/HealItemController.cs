using UnityEngine;

public class HealItemController : MonoBehaviour
{
    [SerializeField] private float healValue = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Heal(healValue);
                Destroy(gameObject);
            }
        }
    }
}
