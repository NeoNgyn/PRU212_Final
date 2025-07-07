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

//	// --- Thêm bi?n GateTriggerBoss vào ?ây ---
//	[Header("Gate Settings")]
//	[SerializeField] private GateTriggerBoss gateTrigger; // C?ng c?n m? khi boss ch?t

//	// Override hàm Start ?? thi?t l?p riêng cho Boss Tri?u H?i
//	protected override void Start()
//	{
//		base.Start(); // G?i hàm Start c?a EnemyController ?? kh?i t?o HP, tham chi?u ng??i ch?i, v.v.
//		summonTimer = summonCooldown; // Kh?i t?o b? ??m th?i gian tri?u h?i
//	}

//	// Override hàm Update ?? th?c hi?n logic riêng c?a Boss Tri?u H?i
//	protected override void Update()
//	{
//		// Boss này s? không di chuy?n theo logic m?c ??nh c?a EnemyController.
//		// base.Update(); // Dòng này ???c b? comment n?u b?n mu?n boss v?n di chuy?n v? phía player.

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

//		// L?u ý: Ph?n h?i máu (regen) không ???c ??a vào ?ây nh? BossEnemyLV1.
//		// N?u b?n mu?n boss summoner có h?i máu, b?n s? c?n thêm bi?n regenRate và logic ? ?ây.
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

//		// 1. Phá h?y t?t c? các minion ?ang ho?t ??ng
//		foreach (GameObject minion in activeMinions)
//		{
//			if (minion != null) // ??m b?o minion không ph?i null tr??c khi phá h?y
//			{
//				Destroy(minion);
//			}
//		}
//		activeMinions.Clear(); // Xóa danh sách sau khi phá h?y

//		// 2. G?i hàm OpenGate() c?a GateTriggerBoss khi boss ch?t
//		if (gateTrigger != null)
//		{
//			Debug.Log("M? c?ng!");
//			gateTrigger.OpenGate();
//		}
//		else
//		{
//			Debug.LogWarning("GateTrigger ch?a ???c gán vào BossSummoner!");
//		}

//		// 3. G?i hàm Die() c?a l?p c? s? (EnemyController) ?? phá h?y GameObject c?a boss
//		base.Die();
//	}

//	// N?u b?n mu?n boss hoàn toàn ??ng yên, b?n có th? override MoveToPlayer() ?? không làm gì:
//	protected override void MoveToPlayer() { /* Boss ??ng yên, không di chuy?n theo logic EnemyController */ }
//	// N?u b?n mu?n nó di chuy?n theo logic khác (không ph?i MoveToPlayer m?c ??nh), b?n có th? thêm logic ?ó vào ?ây.
//}
using UnityEngine;
using System.Collections.Generic; // C?n thi?t ?? s? d?ng List

public class BossSummoner : EnemyController // K? th?a t? EnemyController
{
	// --- CÀI ??T TRI?U H?I MINION ---
	[Header("Summoner Settings")]
	[SerializeField] private GameObject minionPrefab; // Prefab c?a quái v?t s? ???c tri?u h?i
	[SerializeField] private float summonCooldown = 5f; // Th?i gian ch? gi?a các l?n tri?u h?i
	[SerializeField] private Transform[] summonPoints; // Các ?i?m mà minion có th? ???c tri?u h?i
	[SerializeField] private int maxActiveMinions = 3; // S? l??ng minion t?i ?a có th? ho?t ??ng cùng lúc

	private float summonTimer;
	private List<GameObject> activeMinions = new List<GameObject>(); // Danh sách theo dõi các minion ?ang ho?t ??ng

	// --- CÀI ??T C?NG ---
	[Header("Gate Settings")]
	[SerializeField] private GateTriggerBoss gateTrigger; // C?ng c?n m? khi boss ch?t

	// --- CÀI ??T ??A CH?N --- (V?n là m?t ph?n ?ng ??c l?p)
	[Header("Ground Slam Settings")]
	[SerializeField] private float slamRadius = 3f; // Bán kính vùng ??a ch?n
	[SerializeField] private float slamDamage = 20f; // Sát th??ng gây ra b?i ??a ch?n
	[SerializeField] private float slamCooldown = 4f; // Th?i gian ch? gi?a các l?n ??a ch?n (sau khi ???c kích ho?t)
	[SerializeField] private GameObject slamEffectPrefab; // Prefab hi?u ?ng khi boss ??a ch?n (tùy ch?n)

	private float currentSlamCooldown; // Bi?n ??m th?i gian cho ??a ch?n

	// --- CÀI ??T COMBO PH?N ?NG KHI B? T?N CÔNG (B?n c?u l?a + D?ch chuy?n) ---
	[Header("Hit Reaction Combo Settings")]
	[SerializeField] private GameObject fireballPrefab; // Prefab c?a qu? c?u l?a (cùng lo?i v?i cái b?n ?ã g?n FireballController)
	[SerializeField] private int radialFireballOnHitCount = 8; // S? l??ng c?u l?a t?a ra khi b? t?n công
	//[SerializeField] private GameObject teleportEffectPrefab; // Hi?u ?ng d?ch chuy?n khi boss bi?n m?t/xu?t hi?n
	[SerializeField] private float hitReactionComboCooldown = 5f; // Th?i gian ch? gi?a các l?n combo này ???c kích ho?t


	[Header("Teleport Settings")]
	[SerializeField] private float teleportCooldown = 6f;
	[SerializeField] private GameObject teleportEffectPrefab; // Hi?u ?ng d?ch chuy?n khi boss bi?n m?t/xu?t hi?n
	[SerializeField] private float minTeleportDistance = 8f;
	private float currentHitReactionComboCooldown; // Bi?n ??m cooldown cho combo

	// Player ?ã ???c ??nh ngh?a là 'player' trong EnemyController (protected PlayerController player;)
	// currentHp và maxHp c?ng t? EnemyController

	// Override hàm Start ?? kh?i t?o t?t c? các thành ph?n c?a boss
	protected override void Start()
	{
		base.Start(); // G?i hàm Start c?a EnemyController ?? kh?i t?o HP (currentHp = maxHp) và các th? khác.

		// Kh?i t?o các b? ??m th?i gian
		summonTimer = summonCooldown;
		currentSlamCooldown = slamCooldown;
		currentHitReactionComboCooldown = hitReactionComboCooldown; // Kh?i t?o cooldown cho combo
	}

	// Override hàm Update ?? x? lý t?t c? logic c?a boss
	protected override void Update()
	{
		// C?p nh?t t?t c? các b? ??m th?i gian
		summonTimer -= Time.deltaTime;
		currentSlamCooldown -= Time.deltaTime;
		currentHitReactionComboCooldown -= Time.deltaTime; // C?p nh?t cooldown combo

		// --- Logic Tri?u h?i Minion ---
		if (summonTimer <= 0f)
		{
			CleanUpDeadMinions(); // D?n d?p các minion ?ã ch?t kh?i danh sách
			if (activeMinions.Count < maxActiveMinions) // Ch? tri?u h?i n?u ch?a ??t gi?i h?n
			{
				SummonMinion();
				summonTimer = summonCooldown; // ??t l?i b? ??m th?i gian
			}
		}

		// Boss này ??ng yên, không có logic di chuy?n m?c ??nh trong Update (vì MoveToPlayer ?ã override)
	}

	// --- HÀM TRI?U H?I MINION ---
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

	// --- HÀM D?N D?P MINION ?Ã CH?T ---
	void CleanUpDeadMinions()
	{
		// Lo?i b? các ph?n t? null (các minion ?ã b? phá h?y) kh?i danh sách
		activeMinions.RemoveAll(item => item == null);
	}

	// --- HÀM B?N C?U L?A T?A TRÒN (KHI B? T?N CÔNG) ---
	// Hàm này s? ???c g?i khi combo ???c kích ho?t
	void ShootRadialFireballsOnHit()
	{
		if (fireballPrefab == null)
		{
			Debug.LogWarning("Fireball Prefab ch?a ???c gán cho BossSummoner ?? b?n khi b? t?n công!");
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
				rb.linearVelocity = direction * 4f; // T?c ?? c?u l?a t?a tròn
			}
		}
		Debug.Log($"Boss b?n {radialFireballOnHitCount} c?u l?a t?a tròn khi b? t?n công!");
	}

	// --- HÀM TH?C HI?N ??A CH?N ---
	void PerformGroundSlam()
	{
		Debug.Log("Boss th?c hi?n Ground Slam!");

		// Optional: T?o hi?u ?ng hình ?nh (b?i, sóng ??t, v.v.)
		if (slamEffectPrefab != null)
		{
			Instantiate(slamEffectPrefab, transform.position, Quaternion.identity);
		}

		// Phát hi?n ng??i ch?i trong vùng ?nh h??ng và gây sát th??ng
		// Gi? s? ng??i ch?i có Collider2D và tag là "Player"
		Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, slamRadius);

		foreach (Collider2D obj in hitObjects)
		{
			if (obj.CompareTag("Player"))
			{
				// Gi? s? PlayerController có hàm TakeDamage(float damage)
				PlayerController playerScript = obj.GetComponent<PlayerController>();
				if (playerScript != null)
				{
					playerScript.TakeDamage(slamDamage);
					Debug.Log($"Player b? Ground Slam gây {slamDamage} sát th??ng!");
				}
			}
		}
	}

	// --- HÀM TH?C HI?N D?CH CHUY?N ---
	void PerformTeleport()
	{
		Debug.Log("Boss th?c hi?n Teleport!");

		// T?o hi?u ?ng bi?n m?t t?i v? trí hi?n t?i
		if (teleportEffectPrefab != null)
		{
			GameObject effectDisappear = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
			Destroy(effectDisappear, 1f); // ?i?u ch?nh th?i gian h?y cho phù h?p
		}

		// Th?c hi?n d?ch chuy?n ??n m?t ?i?m ng?u nhiên
		if (summonPoints != null && summonPoints.Length > 0)
		{
			// Tìm ?i?m d?ch chuy?n t?t nh?t (xa nh?t kh?i ng??i ch?i và ?? kho?ng cách t?i thi?u)
			Transform bestTeleportPoint = null;
			float maxDistanceToPlayer = -1f; // Kh?i t?o v?i giá tr? nh? ?? tìm giá tr? l?n h?n

			// ??m b?o player ?ã ???c gán t? EnemyController và không ph?i null
			if (player == null || player.transform == null)
			{
				Debug.LogWarning("Player không tìm th?y khi Boss c? g?ng d?ch chuy?n!");
				// N?u không tìm th?y player, d?ch chuy?n ??n m?t ?i?m ng?u nhiên b?t k?
				transform.position = summonPoints[Random.Range(0, summonPoints.Length)].position;
				if (teleportEffectPrefab != null)
				{
					GameObject effectAppear = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
					Destroy(effectAppear, 1f);
				}
				return; // Thoát hàm
			}


			foreach (Transform point in summonPoints)
			{
				float distanceToPlayerFromPoint = Vector2.Distance(point.position, player.transform.position);

				// Ki?m tra n?u ?i?m này xa h?n ?i?m t?t nh?t hi?n t?i VÀ ?áp ?ng kho?ng cách t?i thi?u
				if (distanceToPlayerFromPoint > maxDistanceToPlayer && distanceToPlayerFromPoint >= minTeleportDistance)
				{
					maxDistanceToPlayer = distanceToPlayerFromPoint;
					bestTeleportPoint = point;
				}
			}

			// N?u tìm th?y m?t ?i?m t?t nh?t
			if (bestTeleportPoint != null)
			{
				transform.position = bestTeleportPoint.position; // D?ch chuy?n ??n ?i?m ?ã ch?n
				Debug.Log($"Boss d?ch chuy?n ??n ?i?m xa nh?t kh?i Player: {bestTeleportPoint.name} (Cách Player: {maxDistanceToPlayer:F2})");

				// T?o hi?u ?ng xu?t hi?n t?i v? trí m?i
				if (teleportEffectPrefab != null)
				{
					GameObject effectAppear = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
					Destroy(effectAppear, 1f);
				}
			}
			else // N?u không tìm ???c ?i?m nào ?? xa, thì d?ch chuy?n ng?u nhiên ho?c không d?ch chuy?n
			{
				Debug.LogWarning("Không tìm ???c ?i?m d?ch chuy?n ?? xa kh?i Player. D?ch chuy?n ng?u nhiên m?t ?i?m b?t k?.");
				transform.position = summonPoints[Random.Range(0, summonPoints.Length)].position; // D?ch chuy?n ??n ?i?m ng?u nhiên
				if (teleportEffectPrefab != null)
				{
					GameObject effectAppear = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
					Destroy(effectAppear, 1f);
				}
			}
		}
		else
		{
			Debug.LogWarning("Không có Summon Points nào ???c gán ?? Boss d?ch chuy?n!");
		}
	}


	// --- OVERRIDE TAKE DAMAGE ---
	// Hàm này s? kích ho?t các ph?n ?ng khi boss b? t?n công
	public override void TakeDamage(float damage, Vector2 knockback)
	{
		Debug.Log($"Boss Tri?u H?i nh?n {damage} damage, HP tr??c khi tr?: {currentHp}");

		currentHp -= damage; // Tr? máu
		currentHp = Mathf.Max(currentHp, 0); // ??m b?o HP không âm

		UpdateHealthBar(); // C?p nh?t thanh máu (hàm t? EnemyController)

		Debug.Log("HP sau khi tr?: " + currentHp);

		if (currentHp <= 0)
		{
			Debug.Log("Boss Tri?u H?i ch?t, g?i Die()");
			Die(); // G?i hàm Die c?a BossSummoner
		}
		else
		{
			// Áp d?ng ??y lùi cho boss (t? EnemyController)
			base.ApplyKnockback(knockback);

			// --- KÍCH HO?T COMBO PH?N ?NG KHI B? T?N CÔNG (B?n c?u l?a + D?ch chuy?n) ---
			// Combo này s? ???c kích ho?t n?u cooldown cho phép
			if (currentHitReactionComboCooldown <= 0f)
			{
				ShootRadialFireballsOnHit(); // B?n c?u l?a tr??c
				PerformTeleport();           // Sau ?ó d?ch chuy?n
				currentHitReactionComboCooldown = hitReactionComboCooldown; // ??t l?i cooldown cho combo
			}

			// --- KÍCH HO?T ??A CH?N (n?u cooldown cho phép và mu?n nó là ph?n ?ng ??c l?p) ---
			// ??a ch?n s? kích ho?t ??c l?p v?i combo b?n c?u l?a/d?ch chuy?n
			if (currentSlamCooldown <= 0f)
			{
				PerformGroundSlam();
				currentSlamCooldown = slamCooldown;
			}
		}
	}

	// --- OVERRIDE DIE ---
	// X? lý khi boss ch?t: d?n d?p minion và m? c?ng
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

		// 2. M? c?ng
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

	// --- OVERRIDE MOVE TO PLAYER ---
	// Boss này ??ng yên, không di chuy?n theo logic m?c ??nh c?a EnemyController.
	protected override void MoveToPlayer() { /* Boss ??ng yên, không di chuy?n */ }
}