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

//	// --- Th�m bi?n GateTriggerBoss v�o ?�y ---
//	[Header("Gate Settings")]
//	[SerializeField] private GateTriggerBoss gateTrigger; // C?ng c?n m? khi boss ch?t

//	// Override h�m Start ?? thi?t l?p ri�ng cho Boss Tri?u H?i
//	protected override void Start()
//	{
//		base.Start(); // G?i h�m Start c?a EnemyController ?? kh?i t?o HP, tham chi?u ng??i ch?i, v.v.
//		summonTimer = summonCooldown; // Kh?i t?o b? ??m th?i gian tri?u h?i
//	}

//	// Override h�m Update ?? th?c hi?n logic ri�ng c?a Boss Tri?u H?i
//	protected override void Update()
//	{
//		// Boss n�y s? kh�ng di chuy?n theo logic m?c ??nh c?a EnemyController.
//		// base.Update(); // D�ng n�y ???c b? comment n?u b?n mu?n boss v?n di chuy?n v? ph�a player.

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

//		// L?u �: Ph?n h?i m�u (regen) kh�ng ???c ??a v�o ?�y nh? BossEnemyLV1.
//		// N?u b?n mu?n boss summoner c� h?i m�u, b?n s? c?n th�m bi?n regenRate v� logic ? ?�y.
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

//		// 1. Ph� h?y t?t c? c�c minion ?ang ho?t ??ng
//		foreach (GameObject minion in activeMinions)
//		{
//			if (minion != null) // ??m b?o minion kh�ng ph?i null tr??c khi ph� h?y
//			{
//				Destroy(minion);
//			}
//		}
//		activeMinions.Clear(); // X�a danh s�ch sau khi ph� h?y

//		// 2. G?i h�m OpenGate() c?a GateTriggerBoss khi boss ch?t
//		if (gateTrigger != null)
//		{
//			Debug.Log("M? c?ng!");
//			gateTrigger.OpenGate();
//		}
//		else
//		{
//			Debug.LogWarning("GateTrigger ch?a ???c g�n v�o BossSummoner!");
//		}

//		// 3. G?i h�m Die() c?a l?p c? s? (EnemyController) ?? ph� h?y GameObject c?a boss
//		base.Die();
//	}

//	// N?u b?n mu?n boss ho�n to�n ??ng y�n, b?n c� th? override MoveToPlayer() ?? kh�ng l�m g�:
//	protected override void MoveToPlayer() { /* Boss ??ng y�n, kh�ng di chuy?n theo logic EnemyController */ }
//	// N?u b?n mu?n n� di chuy?n theo logic kh�c (kh�ng ph?i MoveToPlayer m?c ??nh), b?n c� th? th�m logic ?� v�o ?�y.
//}
using UnityEngine;
using System.Collections.Generic; // C?n thi?t ?? s? d?ng List

public class BossSummoner : EnemyController // K? th?a t? EnemyController
{
	// --- C�I ??T TRI?U H?I MINION ---
	[Header("Summoner Settings")]
	[SerializeField] private GameObject minionPrefab; // Prefab c?a qu�i v?t s? ???c tri?u h?i
	[SerializeField] private float summonCooldown = 5f; // Th?i gian ch? gi?a c�c l?n tri?u h?i
	[SerializeField] private Transform[] summonPoints; // C�c ?i?m m� minion c� th? ???c tri?u h?i
	[SerializeField] private int maxActiveMinions = 3; // S? l??ng minion t?i ?a c� th? ho?t ??ng c�ng l�c

	private float summonTimer;
	private List<GameObject> activeMinions = new List<GameObject>(); // Danh s�ch theo d�i c�c minion ?ang ho?t ??ng

	// --- C�I ??T C?NG ---
	[Header("Gate Settings")]
	[SerializeField] private GateTriggerBoss gateTrigger; // C?ng c?n m? khi boss ch?t

	// --- C�I ??T ??A CH?N --- (V?n l� m?t ph?n ?ng ??c l?p)
	[Header("Ground Slam Settings")]
	[SerializeField] private float slamRadius = 3f; // B�n k�nh v�ng ??a ch?n
	[SerializeField] private float slamDamage = 20f; // S�t th??ng g�y ra b?i ??a ch?n
	[SerializeField] private float slamCooldown = 4f; // Th?i gian ch? gi?a c�c l?n ??a ch?n (sau khi ???c k�ch ho?t)
	[SerializeField] private GameObject slamEffectPrefab; // Prefab hi?u ?ng khi boss ??a ch?n (t�y ch?n)

	private float currentSlamCooldown; // Bi?n ??m th?i gian cho ??a ch?n

	// --- C�I ??T COMBO PH?N ?NG KHI B? T?N C�NG (B?n c?u l?a + D?ch chuy?n) ---
	[Header("Hit Reaction Combo Settings")]
	[SerializeField] private GameObject fireballPrefab; // Prefab c?a qu? c?u l?a (c�ng lo?i v?i c�i b?n ?� g?n FireballController)
	[SerializeField] private int radialFireballOnHitCount = 8; // S? l??ng c?u l?a t?a ra khi b? t?n c�ng
	//[SerializeField] private GameObject teleportEffectPrefab; // Hi?u ?ng d?ch chuy?n khi boss bi?n m?t/xu?t hi?n
	[SerializeField] private float hitReactionComboCooldown = 5f; // Th?i gian ch? gi?a c�c l?n combo n�y ???c k�ch ho?t


	[Header("Teleport Settings")]
	[SerializeField] private float teleportCooldown = 6f;
	[SerializeField] private GameObject teleportEffectPrefab; // Hi?u ?ng d?ch chuy?n khi boss bi?n m?t/xu?t hi?n
	[SerializeField] private float minTeleportDistance = 8f;
	private float currentHitReactionComboCooldown; // Bi?n ??m cooldown cho combo

	// Player ?� ???c ??nh ngh?a l� 'player' trong EnemyController (protected PlayerController player;)
	// currentHp v� maxHp c?ng t? EnemyController

	// Override h�m Start ?? kh?i t?o t?t c? c�c th�nh ph?n c?a boss
	protected override void Start()
	{
		base.Start(); // G?i h�m Start c?a EnemyController ?? kh?i t?o HP (currentHp = maxHp) v� c�c th? kh�c.

		// Kh?i t?o c�c b? ??m th?i gian
		summonTimer = summonCooldown;
		currentSlamCooldown = slamCooldown;
		currentHitReactionComboCooldown = hitReactionComboCooldown; // Kh?i t?o cooldown cho combo
	}

	// Override h�m Update ?? x? l� t?t c? logic c?a boss
	protected override void Update()
	{
		// C?p nh?t t?t c? c�c b? ??m th?i gian
		summonTimer -= Time.deltaTime;
		currentSlamCooldown -= Time.deltaTime;
		currentHitReactionComboCooldown -= Time.deltaTime; // C?p nh?t cooldown combo

		// --- Logic Tri?u h?i Minion ---
		if (summonTimer <= 0f)
		{
			CleanUpDeadMinions(); // D?n d?p c�c minion ?� ch?t kh?i danh s�ch
			if (activeMinions.Count < maxActiveMinions) // Ch? tri?u h?i n?u ch?a ??t gi?i h?n
			{
				SummonMinion();
				summonTimer = summonCooldown; // ??t l?i b? ??m th?i gian
			}
		}

		// Boss n�y ??ng y�n, kh�ng c� logic di chuy?n m?c ??nh trong Update (v� MoveToPlayer ?� override)
	}

	// --- H�M TRI?U H?I MINION ---
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

	// --- H�M D?N D?P MINION ?� CH?T ---
	void CleanUpDeadMinions()
	{
		// Lo?i b? c�c ph?n t? null (c�c minion ?� b? ph� h?y) kh?i danh s�ch
		activeMinions.RemoveAll(item => item == null);
	}

	// --- H�M B?N C?U L?A T?A TR�N (KHI B? T?N C�NG) ---
	// H�m n�y s? ???c g?i khi combo ???c k�ch ho?t
	void ShootRadialFireballsOnHit()
	{
		if (fireballPrefab == null)
		{
			Debug.LogWarning("Fireball Prefab ch?a ???c g�n cho BossSummoner ?? b?n khi b? t?n c�ng!");
			return;
		}

		for (int i = 0; i < radialFireballOnHitCount; i++)
		{
			float angle = i * (360f / radialFireballOnHitCount);
			float radian = angle * Mathf.Deg2Rad;
			Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

			GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
			Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
			if (rb != null)
			{
				rb.linearVelocity = direction * 4f; // T?c ?? c?u l?a t?a tr�n
			}
		}
		Debug.Log($"Boss b?n {radialFireballOnHitCount} c?u l?a t?a tr�n khi b? t?n c�ng!");
	}

	// --- H�M TH?C HI?N ??A CH?N ---
	void PerformGroundSlam()
	{
		Debug.Log("Boss th?c hi?n Ground Slam!");

		// Optional: T?o hi?u ?ng h�nh ?nh (b?i, s�ng ??t, v.v.)
		if (slamEffectPrefab != null)
		{
			Instantiate(slamEffectPrefab, transform.position, Quaternion.identity);
		}

		// Ph�t hi?n ng??i ch?i trong v�ng ?nh h??ng v� g�y s�t th??ng
		// Gi? s? ng??i ch?i c� Collider2D v� tag l� "Player"
		Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, slamRadius);

		foreach (Collider2D obj in hitObjects)
		{
			if (obj.CompareTag("Player"))
			{
				// Gi? s? PlayerController c� h�m TakeDamage(float damage)
				PlayerController playerScript = obj.GetComponent<PlayerController>();
				if (playerScript != null)
				{
					playerScript.TakeDamage(slamDamage);
					Debug.Log($"Player b? Ground Slam g�y {slamDamage} s�t th??ng!");
				}
			}
		}
	}

	// --- H�M TH?C HI?N D?CH CHUY?N ---
	void PerformTeleport()
	{
		Debug.Log("Boss th?c hi?n Teleport!");

		// T?o hi?u ?ng bi?n m?t t?i v? tr� hi?n t?i
		if (teleportEffectPrefab != null)
		{
			GameObject effectDisappear = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
			Destroy(effectDisappear, 1f); // ?i?u ch?nh th?i gian h?y cho ph� h?p
		}

		// Th?c hi?n d?ch chuy?n ??n m?t ?i?m ng?u nhi�n
		if (summonPoints != null && summonPoints.Length > 0)
		{
			// T�m ?i?m d?ch chuy?n t?t nh?t (xa nh?t kh?i ng??i ch?i v� ?? kho?ng c�ch t?i thi?u)
			Transform bestTeleportPoint = null;
			float maxDistanceToPlayer = -1f; // Kh?i t?o v?i gi� tr? nh? ?? t�m gi� tr? l?n h?n

			// ??m b?o player ?� ???c g�n t? EnemyController v� kh�ng ph?i null
			if (player == null || player.transform == null)
			{
				Debug.LogWarning("Player kh�ng t�m th?y khi Boss c? g?ng d?ch chuy?n!");
				// N?u kh�ng t�m th?y player, d?ch chuy?n ??n m?t ?i?m ng?u nhi�n b?t k?
				transform.position = summonPoints[Random.Range(0, summonPoints.Length)].position;
				if (teleportEffectPrefab != null)
				{
					GameObject effectAppear = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
					Destroy(effectAppear, 1f);
				}
				return; // Tho�t h�m
			}


			foreach (Transform point in summonPoints)
			{
				float distanceToPlayerFromPoint = Vector2.Distance(point.position, player.transform.position);

				// Ki?m tra n?u ?i?m n�y xa h?n ?i?m t?t nh?t hi?n t?i V� ?�p ?ng kho?ng c�ch t?i thi?u
				if (distanceToPlayerFromPoint > maxDistanceToPlayer && distanceToPlayerFromPoint >= minTeleportDistance)
				{
					maxDistanceToPlayer = distanceToPlayerFromPoint;
					bestTeleportPoint = point;
				}
			}

			// N?u t�m th?y m?t ?i?m t?t nh?t
			if (bestTeleportPoint != null)
			{
				transform.position = bestTeleportPoint.position; // D?ch chuy?n ??n ?i?m ?� ch?n
				Debug.Log($"Boss d?ch chuy?n ??n ?i?m xa nh?t kh?i Player: {bestTeleportPoint.name} (C�ch Player: {maxDistanceToPlayer:F2})");

				// T?o hi?u ?ng xu?t hi?n t?i v? tr� m?i
				if (teleportEffectPrefab != null)
				{
					GameObject effectAppear = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
					Destroy(effectAppear, 1f);
				}
			}
			else // N?u kh�ng t�m ???c ?i?m n�o ?? xa, th� d?ch chuy?n ng?u nhi�n ho?c kh�ng d?ch chuy?n
			{
				Debug.LogWarning("Kh�ng t�m ???c ?i?m d?ch chuy?n ?? xa kh?i Player. D?ch chuy?n ng?u nhi�n m?t ?i?m b?t k?.");
				transform.position = summonPoints[Random.Range(0, summonPoints.Length)].position; // D?ch chuy?n ??n ?i?m ng?u nhi�n
				if (teleportEffectPrefab != null)
				{
					GameObject effectAppear = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
					Destroy(effectAppear, 1f);
				}
			}
		}
		else
		{
			Debug.LogWarning("Kh�ng c� Summon Points n�o ???c g�n ?? Boss d?ch chuy?n!");
		}
	}


	// --- OVERRIDE TAKE DAMAGE ---
	// H�m n�y s? k�ch ho?t c�c ph?n ?ng khi boss b? t?n c�ng
	public override void TakeDamage(float damage, Vector2 knockback)
	{
		Debug.Log($"Boss Tri?u H?i nh?n {damage} damage, HP tr??c khi tr?: {currentHp}");

		currentHp -= damage; // Tr? m�u
		currentHp = Mathf.Max(currentHp, 0); // ??m b?o HP kh�ng �m

		UpdateHealthBar(); // C?p nh?t thanh m�u (h�m t? EnemyController)

		Debug.Log("HP sau khi tr?: " + currentHp);

		if (currentHp <= 0)
		{
			Debug.Log("Boss Tri?u H?i ch?t, g?i Die()");
			Die(); // G?i h�m Die c?a BossSummoner
		}
		else
		{
			// �p d?ng ??y l�i cho boss (t? EnemyController)
			base.ApplyKnockback(knockback);

			// --- K�CH HO?T COMBO PH?N ?NG KHI B? T?N C�NG (B?n c?u l?a + D?ch chuy?n) ---
			// Combo n�y s? ???c k�ch ho?t n?u cooldown cho ph�p
			if (currentHitReactionComboCooldown <= 0f)
			{
				ShootRadialFireballsOnHit(); // B?n c?u l?a tr??c
				PerformTeleport();           // Sau ?� d?ch chuy?n
				currentHitReactionComboCooldown = hitReactionComboCooldown; // ??t l?i cooldown cho combo
			}

			// --- K�CH HO?T ??A CH?N (n?u cooldown cho ph�p v� mu?n n� l� ph?n ?ng ??c l?p) ---
			// ??a ch?n s? k�ch ho?t ??c l?p v?i combo b?n c?u l?a/d?ch chuy?n
			if (currentSlamCooldown <= 0f)
			{
				PerformGroundSlam();
				currentSlamCooldown = slamCooldown;
			}
		}
	}

	// --- OVERRIDE DIE ---
	// X? l� khi boss ch?t: d?n d?p minion v� m? c?ng
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

		// 2. M? c?ng
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

	// --- OVERRIDE MOVE TO PLAYER ---
	// Boss n�y ??ng y�n, kh�ng di chuy?n theo logic m?c ??nh c?a EnemyController.
	protected override void MoveToPlayer() { /* Boss ??ng y�n, kh�ng di chuy?n */ }
}