//using UnityEngine;
//using System.Collections.Generic; // C?n thi?t ?? s? d?ng List

//public class BossSummoner : EnemyController // K? th?a t? EnemyController
//{
//	[Header("Summoner Settings")]
//	[SerializeField] private GameObject minionPrefab; // Prefab c?a quái v?t s? ???c tri?u h?i
//	[SerializeField] private float summonCooldown = 5f; // Th?i gian ch? gi?a các l?n tri?u h?i
//	[SerializeField] private Transform[] summonPoints; // Các ?i?m mà minion có th? ???c tri?u h?i
//	[SerializeField] private int maxActiveMinions = 3; // S? l??ng minion t?i ?a có th? ho?t ??ng cùng lúc

//	private float summonTimer;
//	private List<GameObject> activeMinions = new List<GameObject>(); // Danh sách theo dõi các minion ?ang ho?t ??ng

//	// Override hàm Start ?? thi?t l?p riêng cho Boss Tri?u H?i
//	protected override void Start()
//	{
//		base.Start(); // G?i hàm Start c?a EnemyController ?? kh?i t?o HP, tham chi?u ng??i ch?i, v.v.
//		summonTimer = summonCooldown; // Kh?i t?o b? ??m th?i gian tri?u h?i
//	}

//	// Override hàm Update ?? th?c hi?n logic riêng c?a Boss Tri?u H?i
//	protected override void Update()
//	{
//		// Quan tr?ng: N?u b?n KHÔNG mu?n boss di chuy?n v? phía ng??i ch?i (nh? EnemyController làm),
//		// thì KHÔNG g?i base.Update() ? ?ây. Ho?c b?n có th? override MoveToPlayer() ?? nó không làm gì c?.
//		// base.Update(); // B? comment dòng này n?u b?n mu?n boss v?n di chuy?n theo logic EnemyController

//		// --- Logic Tri?u H?i ---
//		summonTimer -= Time.deltaTime;
//		if (summonTimer <= 0f)
//		{
//			CleanUpDeadMinions(); // D?n d?p các minion ?ã ch?t kh?i danh sách
//			if (activeMinions.Count < maxActiveMinions) // Ch? tri?u h?i n?u ch?a ??t gi?i h?n
//			{
//				SummonMinion();
//				summonTimer = summonCooldown; // ??t l?i b? ??m th?i gian
//			}
//		}

//		// --- Logic H?i máu (n?u boss có regen) ---
//		// N?u BossSummoner có regen, b?n c?n thêm logic này.
//		// B?n có th? thêm bi?n regenRate riêng ? ?ây n?u mu?n.
//		// Ví d?:
//		// if (currentHp < maxHp) // currentHp và maxHp t? EnemyController
//		// {
//		//    currentHp += regenRate * Time.deltaTime;
//		//    currentHp = Mathf.Min(currentHp, maxHp);
//		//    UpdateHealthBar(); // C?p nh?t thanh máu
//		// }
//	}

//	void SummonMinion()
//	{
//		if (minionPrefab == null)
//		{
//			Debug.LogWarning("Minion Prefab ch?a ???c gán cho BossSummoner!");
//			return;
//		}

//		Vector3 spawnPos = transform.position; // M?c ??nh spawn t?i v? trí c?a boss
//		if (summonPoints != null && summonPoints.Length > 0)
//		{
//			// Ch?n m?t ?i?m tri?u h?i ng?u nhiên n?u có nhi?u ?i?m ???c cung c?p
//			spawnPos = summonPoints[Random.Range(0, summonPoints.Length)].position;
//		}

//		GameObject newMinion = Instantiate(minionPrefab, spawnPos, Quaternion.identity);
//		activeMinions.Add(newMinion); // Thêm minion m?i vào danh sách theo dõi
//		Debug.Log("Boss ?ã tri?u h?i m?t minion!");
//	}

//	void CleanUpDeadMinions()
//	{
//		// Lo?i b? các ph?n t? null (các minion ?ã b? phá h?y) kh?i danh sách
//		activeMinions.RemoveAll(item => item == null);
//	}

//	// Override hàm Die() ?? làm nh?ng ?i?u ??c bi?t khi boss ch?t
//	protected override void Die()
//	{
//		Debug.Log("Boss Tri?u H?i ?ã b? ?ánh b?i!");
//		// Tùy ch?n: Phá h?y t?t c? các minion ?ang ho?t ??ng khi boss ch?t
//		foreach (GameObject minion in activeMinions)
//		{
//			if (minion != null) // ??m b?o minion không ph?i null tr??c khi phá h?y
//			{
//				Destroy(minion);
//			}
//		}
//		activeMinions.Clear(); // Xóa danh sách sau khi phá h?y

//		// G?i hàm Die() c?a l?p c? s? (EnemyController) ?? phá h?y GameObject c?a boss
//		base.Die();
//	}

//	// N?u b?n mu?n boss hoàn toàn ??ng yên, b?n có th? override MoveToPlayer() ?? không làm gì:
//	// protected override void MoveToPlayer() { /* Boss ??ng yên, không di chuy?n */ }
//}

using UnityEngine;
using System.Collections.Generic; // C?n thi?t ?? s? d?ng List

public class BossSummoner : EnemyController // K? th?a t? EnemyController
{
	[Header("Summoner Settings")]
	[SerializeField] private GameObject minionPrefab; // Prefab c?a quái v?t s? ???c tri?u h?i
	[SerializeField] private float summonCooldown = 5f; // Th?i gian ch? gi?a các l?n tri?u h?i
	[SerializeField] private Transform[] summonPoints; // Các ?i?m mà minion có th? ???c tri?u h?i
	[SerializeField] private int maxActiveMinions = 3; // S? l??ng minion t?i ?a có th? ho?t ??ng cùng lúc

	private float summonTimer;
	private List<GameObject> activeMinions = new List<GameObject>(); // Danh sách theo dõi các minion ?ang ho?t ??ng

	// --- Thêm bi?n GateTriggerBoss vào ?ây ---
	[Header("Gate Settings")]
	[SerializeField] private GateTriggerBoss gateTrigger; // C?ng c?n m? khi boss ch?t

	// Override hàm Start ?? thi?t l?p riêng cho Boss Tri?u H?i
	protected override void Start()
	{
		base.Start(); // G?i hàm Start c?a EnemyController ?? kh?i t?o HP, tham chi?u ng??i ch?i, v.v.
		summonTimer = summonCooldown; // Kh?i t?o b? ??m th?i gian tri?u h?i
	}

	// Override hàm Update ?? th?c hi?n logic riêng c?a Boss Tri?u H?i
	protected override void Update()
	{
		// Boss này s? không di chuy?n theo logic m?c ??nh c?a EnemyController.
		// base.Update(); // Dòng này ???c b? comment n?u b?n mu?n boss v?n di chuy?n v? phía player.

		// --- Logic Tri?u H?i ---
		summonTimer -= Time.deltaTime;
		if (summonTimer <= 0f)
		{
			CleanUpDeadMinions(); // D?n d?p các minion ?ã ch?t kh?i danh sách
			if (activeMinions.Count < maxActiveMinions) // Ch? tri?u h?i n?u ch?a ??t gi?i h?n
			{
				SummonMinion();
				summonTimer = summonCooldown; // ??t l?i b? ??m th?i gian
			}
		}

		// L?u ý: Ph?n h?i máu (regen) không ???c ??a vào ?ây nh? BossEnemyLV1.
		// N?u b?n mu?n boss summoner có h?i máu, b?n s? c?n thêm bi?n regenRate và logic ? ?ây.
	}

	void SummonMinion()
	{
		if (minionPrefab == null)
		{
			Debug.LogWarning("Minion Prefab ch?a ???c gán cho BossSummoner!");
			return;
		}

		Vector3 spawnPos = transform.position; // M?c ??nh spawn t?i v? trí c?a boss
		if (summonPoints != null && summonPoints.Length > 0)
		{
			// Ch?n m?t ?i?m tri?u h?i ng?u nhiên n?u có nhi?u ?i?m ???c cung c?p
			spawnPos = summonPoints[Random.Range(0, summonPoints.Length)].position;
		}

		GameObject newMinion = Instantiate(minionPrefab, spawnPos, Quaternion.identity);
		activeMinions.Add(newMinion); // Thêm minion m?i vào danh sách theo dõi
		Debug.Log("Boss ?ã tri?u h?i m?t minion!");
	}

	void CleanUpDeadMinions()
	{
		// Lo?i b? các ph?n t? null (các minion ?ã b? phá h?y) kh?i danh sách
		activeMinions.RemoveAll(item => item == null);
	}

	// Override hàm Die() ?? làm nh?ng ?i?u ??c bi?t khi boss ch?t
	protected override void Die()
	{
		Debug.Log("Boss Tri?u H?i ?ã b? ?ánh b?i!");

		// 1. Phá h?y t?t c? các minion ?ang ho?t ??ng
		foreach (GameObject minion in activeMinions)
		{
			if (minion != null) // ??m b?o minion không ph?i null tr??c khi phá h?y
			{
				Destroy(minion);
			}
		}
		activeMinions.Clear(); // Xóa danh sách sau khi phá h?y

		// 2. G?i hàm OpenGate() c?a GateTriggerBoss khi boss ch?t
		if (gateTrigger != null)
		{
			Debug.Log("M? c?ng!");
			gateTrigger.OpenGate();
		}
		else
		{
			Debug.LogWarning("GateTrigger ch?a ???c gán vào BossSummoner!");
		}

		// 3. G?i hàm Die() c?a l?p c? s? (EnemyController) ?? phá h?y GameObject c?a boss
		base.Die();
	}

	// N?u b?n mu?n boss hoàn toàn ??ng yên, b?n có th? override MoveToPlayer() ?? không làm gì:
	protected override void MoveToPlayer() { /* Boss ??ng yên, không di chuy?n theo logic EnemyController */ }
	// N?u b?n mu?n nó di chuy?n theo logic khác (không ph?i MoveToPlayer m?c ??nh), b?n có th? thêm logic ?ó vào ?ây.
}