////using UnityEngine;
////using UnityEngine.Tilemaps;
////using TMPro;
////using UnityEngine.SceneManagement;

////public class GateTriggerBoss : MonoBehaviour
////{
////	[SerializeField] private Tilemap gateTilemap;
////	[SerializeField] private TMP_Text gateMessageText;
////	[SerializeField] private AudioSource gateOpenSound;
////	[SerializeField] private Collider2D gateCollider;

////	private bool gateOpened = false;

////	public void OpenGate()
////	{
////		Debug.Log("?� g?i OpenGate()");

////		if (!gateOpened)
////		{
////			Debug.Log("?ang m? c?ng...");

////			gateOpened = true;

////			if (gateTilemap != null)
////			{
////				Debug.Log("?n Tilemap th�nh c�ng.");
////				gateTilemap.gameObject.SetActive(false);
////			}
////			else
////			{
////				Debug.LogWarning("gateTilemap NULL.");
////			}

////			if (gateCollider != null)
////			{
////				Debug.Log("T?t Collider th�nh c�ng.");
////				gateCollider.enabled = false;
////			}
////			else
////			{
////				Debug.LogWarning("gateCollider NULL.");
////			}

////			if (gateOpenSound != null)
////			{
////				gateOpenSound.Play();
////			}

////			if (gateMessageText != null)
////			{
////				gateMessageText.text = "The Gate is Open!";
////				Invoke(nameof(HideMessage), 2f);
////			}
////		}
////		else
////		{
////			Debug.Log("C?ng ?� m? t? tr??c, kh�ng m? l?i.");
////		}
////	}

////	private void HideMessage()
////	{
////		if (gateMessageText != null)
////		{
////			gateMessageText.text = "";
////		}
////	}

////	private void OnTriggerStay2D(Collider2D collision)
////	{
////		if (gateOpened && collision.CompareTag("Player"))
////		{
////			if (Input.GetKeyDown(KeyCode.W))
////			{
////				SceneManager.LoadScene("SceneLv1.23");
////			}
////		}
////	}
////}
//using UnityEngine;
//using UnityEngine.Tilemaps;
//using TMPro;
//using UnityEngine.SceneManagement;

//public class GateTriggerBoss : MonoBehaviour
//{
//	[SerializeField] private Tilemap gateTilemap;
//	[SerializeField] private TMP_Text gateMessageText;
//	[SerializeField] private AudioSource gateOpenSound;
//	[SerializeField] private Collider2D gateCollider; // ??m b?o b?n ?� g�n Collider2D c?a c?ng v�o ?�y trong Inspector

//	private bool gateOpened = false;

//	public void OpenGate()
//	{
//		Debug.Log("?� g?i OpenGate()"); // ?� s?a l?i font ti?ng Vi?t

//		if (!gateOpened)
//		{
//			Debug.Log("?ang m? c?ng..."); // ?� s?a l?i font ti?ng Vi?t

//			gateOpened = true;

//			if (gateTilemap != null)
//			{
//				Debug.Log("?n Tilemap th�nh c�ng."); // ?� s?a l?i font ti?ng Vi?t
//				gateTilemap.gameObject.SetActive(false); // ?n h�nh ?nh c?ng
//			}
//			else
//			{
//				Debug.LogWarning("gateTilemap NULL.");
//			}

//			// *** D�NG N�Y ?� ???C X�A HO?C COMMENT L?I ***
//			// *** KH�NG T?T gateCollider N?A ***
//			// N?u d�ng d??i ?�y v?n c�n, b?n s? kh�ng th? chuy?n scene.
//			// if (gateCollider != null)
//			// {
//			//     Debug.Log("T?t Collider th�nh c�ng.");
//			//     gateCollider.enabled = false; // <<< D�NG C� V?N ?? N�Y ?� B? X�A/COMMENT!
//			// }
//			// else
//			// {
//			//     Debug.LogWarning("gateCollider NULL.");
//			// }

//			if (gateOpenSound != null)
//			{
//				gateOpenSound.Play();
//			}

//			if (gateMessageText != null)
//			{
//				gateMessageText.text = "The Gate is Open!";
//				Invoke(nameof(HideMessage), 2f);
//			}
//		}
//		else
//		{
//			Debug.Log("C?ng ?� m? t? tr??c, kh�ng m? l?i."); // ?� s?a l?i font ti?ng Vi?t
//		}
//	}

//	private void HideMessage()
//	{
//		if (gateMessageText != null)
//			gateMessageText.text = "";
//	}

//	private void OnTriggerStay2D(Collider2D collision)
//	{
//		// Th�m Debug.Log ?? ki?m tra xem h�m n�y c� ???c g?i kh�ng
//		Debug.Log("OnTriggerStay2D ?ang ???c ki?m tra.");

//		if (gateOpened && collision.CompareTag("Player"))
//		{
//			// Th�m Debug.Log ?? ki?m tra ?i?u ki?n n�y
//			Debug.Log("Ng??i ch?i trong v�ng trigger V� c?ng ?� m?.");
//			Debug.Log("Input.GetKeyDown(KeyCode.W) hi?n t?i: " + Input.GetKeyDown(KeyCode.W)); // TH�M D�NG N�Y
//			if (Input.GetKeyDown(KeyCode.W)) // D�ng n�y ?� t?n t?i
//			{
//				// Th�m Debug.Log ?? x�c nh?n n�t W ???c nh?n
//				Debug.Log("Ng??i ch?i b?m W! ?ang t?i scene SceneLv1.3...");
//				SceneManager.LoadScene("SceneLv1.3");
//			}
//		}
//	}
//}
// (Ph?n code c? ?� comment, kh�ng quan t�m)

using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SceneManagement;

public class GateTriggerBoss : MonoBehaviour
{
	[SerializeField] private Tilemap gateTilemap;
	[SerializeField] private TMP_Text gateMessageText;
	[SerializeField] private AudioSource gateOpenSound;
	[SerializeField] private Collider2D gateCollider;


	// --- TH�M D�NG N�Y V�O ?�Y ---
	[Header("Scene Transition Settings")] // Ti�u ?? trong Inspector
	[SerializeField] private string targetSceneName; // Bi?n ?? l?u t�n c?nh ?�ch

	private bool gateOpened = false;

	public void OpenGate()
	{
		Debug.Log("?� g?i OpenGate()"); // ?� s?a l?i font ti?ng Vi?t

		if (!gateOpened)
		{
			Debug.Log("?ang m? c?ng..."); // ?� s?a l?i font ti?ng Vi?t

			gateOpened = true;

			if (gateTilemap != null)
			{
				Debug.Log("?n Tilemap th�nh c�ng."); // ?� s?a l?i font ti?ng Vi?t
				gateTilemap.gameObject.SetActive(false); // ?n h�nh ?nh c?ng
			}
			else
			{
				Debug.LogWarning("gateTilemap NULL.");
			}

			// *** KH�NG C� D�NG gateCollider.enabled = false; ? ?�Y N?A ***
			// Vui l�ng ??m b?o b?n ?� x�a ho?c comment d�ng n�y nh? ?� h??ng d?n.

			if (gateOpenSound != null)
			{
				gateOpenSound.Play();
			}

			if (gateMessageText != null)
			{
				gateMessageText.text = "The Gate is Open!";
				Invoke(nameof(HideMessage), 2f);
			}
		}
		else
		{
			Debug.Log("C?ng ?� m? t? tr??c, kh�ng m? l?i."); // ?� s?a l?i font ti?ng Vi?t
		}
	}

	private void HideMessage()
	{
		if (gateMessageText != null)
			gateMessageText.text = "";
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		Debug.Log("OnTriggerStay2D ?ang ???c ki?m tra.");

		if (gateOpened && collision.CompareTag("Player"))
		{
			Debug.Log("Ng??i ch?i trong v�ng trigger V� c?ng ?� m?.");
			Debug.Log("Input.GetKeyDown(KeyCode.W) hi?n t?i: " + Input.GetKeyDown(KeyCode.W));
			//if (Input.GetKeyDown(KeyCode.W))
			//{
				// --- THAY ??I D�NG N�Y ?? S? D?NG BI?N targetSceneName ---
				if (!string.IsNullOrEmpty(targetSceneName)) // ??m b?o t�n c?nh kh�ng r?ng
				{
					Debug.Log("Ng??i ch?i b?m W! ?ang t?i scene: " + targetSceneName + "..."); // Log t�n c?nh ?�ch
					SceneManager.LoadScene(targetSceneName);
				}
				else
				{
					Debug.LogWarning("Target Scene Name ch?a ???c ??t trong Inspector c?a GateTriggerBoss!");
				}
			//}
		}
	}
}