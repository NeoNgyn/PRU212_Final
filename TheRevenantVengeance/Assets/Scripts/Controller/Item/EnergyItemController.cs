using UnityEngine;

public class EnergyItemController : MonoBehaviour
{
    [SerializeField] private float energyValue = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.GetEnergy(energyValue);
                Destroy(gameObject);
            }
        }
    }
}
