using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float lifetime = 5f;

    private Transform target;
    void Start()
    {
        
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        Destroy(gameObject, lifetime); 
    }

    void Update()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;
        transform.right = direction;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if (!other.isTrigger) 
        {
            Destroy(gameObject);
        }
    }
}
