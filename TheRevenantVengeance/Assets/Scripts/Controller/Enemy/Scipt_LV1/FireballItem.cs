using UnityEngine;

public class FireballItem : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // G�y s�t th??ng cho Enemy (n?u Enemy c� script ph� h?p)
            Debug.Log("Enemy tr�ng ??n!");
            Destroy(gameObject);
        }
    }
}
