//using UnityEngine;
//using UnityEngine.Tilemaps;
//using UnityEngine.SceneManagement;

//public class GateTrigger : MonoBehaviour
//{
//	[SerializeField] private Tilemap gateTilemap;  // G�N Tilemap Gate ? ?�y
//	[SerializeField] private int requiredEnergy = 10;

//	private bool gateOpened = false;

//	private void OnTriggerEnter2D(Collider2D collision)
//	{
//		if (collision.CompareTag("Player"))
//		{
//			PlayerController player = collision.GetComponent<PlayerController>();
//			if (player != null && player.GetCurrentEnergy() >= requiredEnergy)
//			{
//				OpenGate();
//			}
//		}
//	}

//	private void OpenGate()
//	{
//		if (gateTilemap != null && !gateOpened)
//		{
//			gateTilemap.gameObject.SetActive(false);  // T?t c?ng
//			gateOpened = true;
//		}
//	}

//	private void OnTriggerStay2D(Collider2D collision)
//	{
//		if (gateOpened && collision.CompareTag("Player"))
//		{
//			if (Input.GetKeyDown(KeyCode.W))  // Nh?n W ?? v�o map m?i
//			{
//				SceneManager.LoadScene("NextSceneName");  // ??i t�n Scene t?i ?�y
//			}
//		}
//	}
//}
//using UnityEngine;
//using UnityEngine.Tilemaps;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class GateTrigger : MonoBehaviour
//{
//	[SerializeField] private Tilemap gateTilemap;
//	[SerializeField] private int requiredEnergy = 10;
//	[SerializeField] private Text gateMessageText;  // G�n UI Text th�ng b�o
//	[SerializeField] private AudioSource gateOpenSound;  // G�n AudioSource ch?a �m m? c?ng

//	private bool gateOpened = false;

//	private void OnTriggerEnter2D(Collider2D collision)
//	{
//		if (collision.CompareTag("Player"))
//		{
//			PlayerController player = collision.GetComponent<PlayerController>();
//			if (player != null && player.GetCurrentEnergy() >= requiredEnergy)
//			{
//				OpenGate();
//			}
//		}
//	}

//	private void OpenGate()
//	{
//		if (!gateOpened)
//		{
//			gateTilemap.gameObject.SetActive(false);
//			gateOpened = true;

//			// Ph�t �m thanh m? c?ng
//			if (gateOpenSound != null)
//			{
//				gateOpenSound.Play();
//			}

//			// Hi?n th? th�ng b�o c?ng m?
//			if (gateMessageText != null)
//			{
//				gateMessageText.text = "The Gate is Open!";
//				Invoke(nameof(HideMessage), 2f);  // ?n sau 2 gi�y (t�y ch?nh)
//			}
//		}
//	}

//	private void HideMessage()
//	{
//		if (gateMessageText != null)
//		{
//			gateMessageText.text = "";
//		}
//	}

//	private void OnTriggerStay2D(Collider2D collision)
//	{
//		if (gateOpened && collision.CompareTag("Player"))
//		{
//			if (Input.GetKeyDown(KeyCode.W))
//			{
//				SceneManager.LoadScene("SceneLv1.2");
//			}
//		}
//	}
//}
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SceneManagement;

public class GateTrigger : MonoBehaviour
{
	[SerializeField] private Tilemap gateTilemap;
	[SerializeField] private int requiredEnergy = 10;
	[SerializeField] private TMP_Text gateMessageText;  // D�NG TMP_Text
	[SerializeField] private AudioSource gateOpenSound;
	[SerializeField] private PlayerController player;
	[SerializeField] private Collider2D gateCollider;  // TH�M: K�o Collider c?a c?ng v�o ?�y (Composite Collider ho?c Tilemap Collider 2D)

	private bool gateOpened = false;

	private void Update()
	{
		if (!gateOpened && player != null && player.GetCurrentEnergy() >= requiredEnergy)
		{
			OpenGate();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerController player = collision.GetComponent<PlayerController>();
			if (player != null && player.GetCurrentEnergy() >= requiredEnergy)
			{
				OpenGate();
			}
		}
	}

	private void OpenGate()
	{
		if (!gateOpened)
		{
			gateOpened = true;

			// ?n Tilemap (?n h�nh ?nh c?ng)
			if (gateTilemap != null)
			{
				gateTilemap.gameObject.SetActive(false);
			}

			// T?t collider ?? Player ?i qua
			if (gateCollider != null)
			{
				gateCollider.enabled = false;
			}

			// Ph�t �m thanh m? c?ng
			if (gateOpenSound != null)
			{
				gateOpenSound.Play();
			}

			// Hi?n th�ng b�o
			if (gateMessageText != null)
			{
				gateMessageText.text = "The Gate is Open!";
				Invoke(nameof(HideMessage), 2f);
			}
		}
	}

	private void HideMessage()
	{
		if (gateMessageText != null)
		{
			gateMessageText.text = "";
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (gateOpened && collision.CompareTag("Player"))
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				SceneManager.LoadScene("SceneLv1.2");
			}
		}
	}
}

