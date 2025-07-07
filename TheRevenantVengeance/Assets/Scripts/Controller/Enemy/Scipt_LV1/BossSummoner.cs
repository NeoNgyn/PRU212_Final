//using UnityEngine;
//using System.Collections.Generic; // C?n thi?t ?? s? d?ng List

//public class BossSummoner : EnemyController // K? th?a t? EnemyController
//{
//	[Header("Summoner Settings")]
//	[SerializeField] private GameObject minionPrefab; // Prefab c?a qu�i v?t s? ???c tri?u h?i
//	[SerializeField] private float summonCooldown = 5f; // Th?i gian ch? gi?a c�c l?n tri?u h?i
//	[SerializeField] private Transform[] summonPoints; // C�c ?i?m m� minion c� th? ???c tri?u h?i
//	[SerializeField] private int maxActiveMinions = 3; // S? l??ng minion t?i ?a c� th? ho?t ??ng c�ng l�c

//	private float summonTimer;
//	private List<GameObject> activeMinions = new List<GameObject>(); // Danh s�ch theo d�i c�c minion ?ang ho?t ??ng

//	// Override h�m Start ?? thi?t l?p ri�ng cho Boss Tri?u H?i
//	protected override void Start()
//	{
//		base.Start(); // G?i h�m Start c?a EnemyController ?? kh?i t?o HP, tham chi?u ng??i ch?i, v.v.
//		summonTimer = summonCooldown; // Kh?i t?o b? ??m th?i gian tri?u h?i
//	}

//	// Override h�m Update ?? th?c hi?n logic ri�ng c?a Boss Tri?u H?i
//	protected override void Update()
//	{
//		// Quan tr?ng: N?u b?n KH�NG mu?n boss di chuy?n v? ph�a ng??i ch?i (nh? EnemyController l�m),
//		// th� KH�NG g?i base.Update() ? ?�y. Ho?c b?n c� th? override MoveToPlayer() ?? n� kh�ng l�m g� c?.
//		// base.Update(); // B? comment d�ng n�y n?u b?n mu?n boss v?n di chuy?n theo logic EnemyController

//		// --- Logic Tri?u H?i ---
//		summonTimer -= Time.deltaTime;
//		if (summonTimer <= 0f)
//		{
//			CleanUpDeadMinions(); // D?n d?p c�c minion ?� ch?t kh?i danh s�ch
//			if (activeMinions.Count < maxActiveMinions) // Ch? tri?u h?i n?u ch?a ??t gi?i h?n
//			{
//				SummonMinion();
//				summonTimer = summonCooldown; // ??t l?i b? ??m th?i gian
//			}
//		}

//		// --- Logic H?i m�u (n?u boss c� regen) ---
//		// N?u BossSummoner c� regen, b?n c?n th�m logic n�y.
//		// B?n c� th? th�m bi?n regenRate ri�ng ? ?�y n?u mu?n.
//		// V� d?:
//		// if (currentHp < maxHp) // currentHp v� maxHp t? EnemyController
//		// {
//		//    currentHp += regenRate * Time.deltaTime;
//		//    currentHp = Mathf.Min(currentHp, maxHp);
//		//    UpdateHealthBar(); // C?p nh?t thanh m�u
//		// }
//	}

//	void SummonMinion()
//	{
//		if (minionPrefab == null)
//		{
//			Debug.LogWarning("Minion Prefab ch?a ???c g�n cho BossSummoner!");
//			return;
//		}

//		Vector3 spawnPos = transform.position; // M?c ??nh spawn t?i v? tr� c?a boss
//		if (summonPoints != null && summonPoints.Length > 0)
//		{
//			// Ch?n m?t ?i?m tri?u h?i ng?u nhi�n n?u c� nhi?u ?i?m ???c cung c?p
//			spawnPos = summonPoints[Random.Range(0, summonPoints.Length)].position;
//		}

//		GameObject newMinion = Instantiate(minionPrefab, spawnPos, Quaternion.identity);
//		activeMinions.Add(newMinion); // Th�m minion m?i v�o danh s�ch theo d�i
//		Debug.Log("Boss ?� tri?u h?i m?t minion!");
//	}

//	void CleanUpDeadMinions()
//	{
//		// Lo?i b? c�c ph?n t? null (c�c minion ?� b? ph� h?y) kh?i danh s�ch
//		activeMinions.RemoveAll(item => item == null);
//	}

//	// Override h�m Die() ?? l�m nh?ng ?i?u ??c bi?t khi boss ch?t
//	protected override void Die()
//	{
//		Debug.Log("Boss Tri?u H?i ?� b? ?�nh b?i!");
//		// T�y ch?n: Ph� h?y t?t c? c�c minion ?ang ho?t ??ng khi boss ch?t
//		foreach (GameObject minion in activeMinions)
//		{
//			if (minion != null) // ??m b?o minion kh�ng ph?i null tr??c khi ph� h?y
//			{
//				Destroy(minion);
//			}
//		}
//		activeMinions.Clear(); // X�a danh s�ch sau khi ph� h?y

//		// G?i h�m Die() c?a l?p c? s? (EnemyController) ?? ph� h?y GameObject c?a boss
//		base.Die();
//	}

//	// N?u b?n mu?n boss ho�n to�n ??ng y�n, b?n c� th? override MoveToPlayer() ?? kh�ng l�m g�:
//	// protected override void MoveToPlayer() { /* Boss ??ng y�n, kh�ng di chuy?n */ }
//}

using UnityEngine;
using System.Collections.Generic; // C?n thi?t ?? s? d?ng List

public class BossSummoner : EnemyController // K? th?a t? EnemyController
{
	[Header("Summoner Settings")]
	[SerializeField] private GameObject minionPrefab; // Prefab c?a qu�i v?t s? ???c tri?u h?i
	[SerializeField] private float summonCooldown = 5f; // Th?i gian ch? gi?a c�c l?n tri?u h?i
	[SerializeField] private Transform[] summonPoints; // C�c ?i?m m� minion c� th? ???c tri?u h?i
	[SerializeField] private int maxActiveMinions = 3; // S? l??ng minion t?i ?a c� th? ho?t ??ng c�ng l�c

	private float summonTimer;
	private List<GameObject> activeMinions = new List<GameObject>(); // Danh s�ch theo d�i c�c minion ?ang ho?t ??ng

	// --- Th�m bi?n GateTriggerBoss v�o ?�y ---
	[Header("Gate Settings")]
	[SerializeField] private GateTriggerBoss gateTrigger; // C?ng c?n m? khi boss ch?t

	// Override h�m Start ?? thi?t l?p ri�ng cho Boss Tri?u H?i
	protected override void Start()
	{
		base.Start(); // G?i h�m Start c?a EnemyController ?? kh?i t?o HP, tham chi?u ng??i ch?i, v.v.
		summonTimer = summonCooldown; // Kh?i t?o b? ??m th?i gian tri?u h?i
	}

	// Override h�m Update ?? th?c hi?n logic ri�ng c?a Boss Tri?u H?i
	protected override void Update()
	{
		// Boss n�y s? kh�ng di chuy?n theo logic m?c ??nh c?a EnemyController.
		// base.Update(); // D�ng n�y ???c b? comment n?u b?n mu?n boss v?n di chuy?n v? ph�a player.

		// --- Logic Tri?u H?i ---
		summonTimer -= Time.deltaTime;
		if (summonTimer <= 0f)
		{
			CleanUpDeadMinions(); // D?n d?p c�c minion ?� ch?t kh?i danh s�ch
			if (activeMinions.Count < maxActiveMinions) // Ch? tri?u h?i n?u ch?a ??t gi?i h?n
			{
				SummonMinion();
				summonTimer = summonCooldown; // ??t l?i b? ??m th?i gian
			}
		}

		// L?u �: Ph?n h?i m�u (regen) kh�ng ???c ??a v�o ?�y nh? BossEnemyLV1.
		// N?u b?n mu?n boss summoner c� h?i m�u, b?n s? c?n th�m bi?n regenRate v� logic ? ?�y.
	}

	void SummonMinion()
	{
		if (minionPrefab == null)
		{
			Debug.LogWarning("Minion Prefab ch?a ???c g�n cho BossSummoner!");
			return;
		}

		Vector3 spawnPos = transform.position; // M?c ??nh spawn t?i v? tr� c?a boss
		if (summonPoints != null && summonPoints.Length > 0)
		{
			// Ch?n m?t ?i?m tri?u h?i ng?u nhi�n n?u c� nhi?u ?i?m ???c cung c?p
			spawnPos = summonPoints[Random.Range(0, summonPoints.Length)].position;
		}

		GameObject newMinion = Instantiate(minionPrefab, spawnPos, Quaternion.identity);
		activeMinions.Add(newMinion); // Th�m minion m?i v�o danh s�ch theo d�i
		Debug.Log("Boss ?� tri?u h?i m?t minion!");
	}

	void CleanUpDeadMinions()
	{
		// Lo?i b? c�c ph?n t? null (c�c minion ?� b? ph� h?y) kh?i danh s�ch
		activeMinions.RemoveAll(item => item == null);
	}

	// Override h�m Die() ?? l�m nh?ng ?i?u ??c bi?t khi boss ch?t
	protected override void Die()
	{
		Debug.Log("Boss Tri?u H?i ?� b? ?�nh b?i!");

		// 1. Ph� h?y t?t c? c�c minion ?ang ho?t ??ng
		foreach (GameObject minion in activeMinions)
		{
			if (minion != null) // ??m b?o minion kh�ng ph?i null tr??c khi ph� h?y
			{
				Destroy(minion);
			}
		}
		activeMinions.Clear(); // X�a danh s�ch sau khi ph� h?y

		// 2. G?i h�m OpenGate() c?a GateTriggerBoss khi boss ch?t
		if (gateTrigger != null)
		{
			Debug.Log("M? c?ng!");
			gateTrigger.OpenGate();
		}
		else
		{
			Debug.LogWarning("GateTrigger ch?a ???c g�n v�o BossSummoner!");
		}

		// 3. G?i h�m Die() c?a l?p c? s? (EnemyController) ?? ph� h?y GameObject c?a boss
		base.Die();
	}

	// N?u b?n mu?n boss ho�n to�n ??ng y�n, b?n c� th? override MoveToPlayer() ?? kh�ng l�m g�:
	protected override void MoveToPlayer() { /* Boss ??ng y�n, kh�ng di chuy?n theo logic EnemyController */ }
	// N?u b?n mu?n n� di chuy?n theo logic kh�c (kh�ng ph?i MoveToPlayer m?c ??nh), b?n c� th? th�m logic ?� v�o ?�y.
}