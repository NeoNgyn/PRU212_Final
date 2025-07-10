//using UnityEngine;

//public class ItemPickupLV1 : MonoBehaviour
//{
//    public GameObject fireballPrefab;

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Player"))
//        {
//            PlayerController player = collision.GetComponent<PlayerController>();
//            if (player != null)
//            {
//                player.SetFireballPrefab(fireballPrefab);
//            }
//            Destroy(gameObject);
//        }
//    }

//}
// ItemPickupLV1.cs
using UnityEngine;

public class ItemPickupLV1 : MonoBehaviour
{
    public GameObject fireballPrefab;
    [SerializeField] private GameObject auraPrefab;  // K�o prefab h�o quang v�o ?�y trong Inspector

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                // G�n kh? n?ng b?n qu? c?u
                player.SetFireballPrefab(fireballPrefab);

                // K�ch ho?t h�o quang lu�n
                player.ActivateAura(auraPrefab);
            }

            Destroy(gameObject);  // X�a item sau khi nh?t
        }
    }
}
