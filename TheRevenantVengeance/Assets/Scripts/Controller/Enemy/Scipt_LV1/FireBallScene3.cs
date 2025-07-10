using UnityEngine;

public class FireBallScene3 : MonoBehaviour
{
    public float rotationSpeed = 120f;

    private void Update()
    {
        if (transform.parent == null) return; // Chặn lỗi nếu không có parent

        // Xoay quanh boss (parent)
        transform.RotateAround(transform.parent.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = (collision.transform.position - transform.parent.position).normalized;
                rb.AddForce(dir * 500f);
            }
        }
    }
}
