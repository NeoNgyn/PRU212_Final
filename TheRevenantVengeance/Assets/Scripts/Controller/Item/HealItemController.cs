using UnityEngine;

public class HealItemController : MonoBehaviour
{
    [SerializeField] private float healValue = 20f;
    [SerializeField] private float attractRange = 3f;
    [SerializeField] private float moveSpeed = 5f;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attractRange)
        {
            // Di chuyển về phía player
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

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
