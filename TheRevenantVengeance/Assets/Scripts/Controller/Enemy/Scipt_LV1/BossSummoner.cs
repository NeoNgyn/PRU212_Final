using UnityEngine;
using System.Collections.Generic;

public class BossSummoner : EnemyController // K? th?a t? EnemyController
{
	// --- BI?N ANIMATOR VÀ TR?NG THÁI ---
	private Animator animator; // Thêm Animator
	private bool isAttacking = false; // ?? ki?m soát tr?ng thái t?n công c?a Boss (ví d?: ?ang tri?u h?i, ?ang Ground Slam)
	//protected bool isDead = false; // ?? ki?m soát tr?ng thái ch?t c?a Boss

	// --- CÀI ??T CHUNG ---
	[Header("General Boss Settings")]
	[SerializeField] private float attackAnimationDuration = 1.0f; // Th?i gian ch?y animation Attack (Summon ho?c Slam)


	// --- CÀI ??T TRI?U H?I MINION ---
	[Header("Summoner Settings")]
	[SerializeField] private GameObject minionPrefab;
	[SerializeField] private float summonCooldown = 5f;
	[SerializeField] private Transform[] summonPoints;
	[SerializeField] private int maxActiveMinions = 3;

	private float summonTimer;
	private List<GameObject> activeMinions = new List<GameObject>();

	// --- CÀI ??T C?NG ---
	[Header("Gate Settings")]
	[SerializeField] private GateTriggerBoss gateTrigger;

	// --- CÀI ??T ??A CH?N ---
	[Header("Ground Slam Settings")]
	[SerializeField] private float slamRadius = 3f;
	[SerializeField] private float slamDamage = 20f;
	[SerializeField] private float slamCooldown = 4f;
	[SerializeField] private GameObject slamEffectPrefab;

	private float currentSlamCooldown;

	// --- CÀI ??T COMBO PH?N ?NG KHI B? T?N CÔNG (B?n c?u l?a + D?ch chuy?n) ---
	[Header("Hit Reaction Combo Settings")]
	[SerializeField] private GameObject fireballPrefab;
	[SerializeField] private int radialFireballOnHitCount = 8;
	[SerializeField] private float hitReactionComboCooldown = 5f;
	private float currentHitReactionComboCooldown;

	[Header("Teleport Settings")]
	[SerializeField] private float teleportCooldown = 6f; // Cooldown riêng cho Teleport n?u mu?n tách ra
	[SerializeField] private GameObject teleportEffectPrefab;
	[SerializeField] private float minTeleportDistance = 8f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip summonSound;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioSource audioSource;
    // Override hàm Awake ?? l?y component Animator
    protected override void Awake()
	{
		base.Awake();
		animator = GetComponent<Animator>();
	}

	// Override hàm Start ?? kh?i t?o t?t c? các thành ph?n c?a boss
	protected override void Start()
	{
		base.Start(); // G?i hàm Start c?a EnemyController ?? kh?i t?o HP (currentHp = maxHp) và các th? khác.

		// Kh?i t?o các b? ??m th?i gian
		summonTimer = summonCooldown;
		currentSlamCooldown = slamCooldown;
		currentHitReactionComboCooldown = hitReactionComboCooldown;
	}

	// Override hàm Update ?? x? lý t?t c? logic c?a boss
	protected override void Update()
	{
		if (IsDead()) return; // N?u ch?t thì không làm gì n?a

		// C?p nh?t t?t c? các b? ??m th?i gian
		summonTimer -= Time.deltaTime;
		currentSlamCooldown -= Time.deltaTime;
		currentHitReactionComboCooldown -= Time.deltaTime;

		// --- Logic Tri?u h?i Minion ---
		// Tri?u h?i ch? khi không ?ang t?n công hành ??ng khác
		if (summonTimer <= 0f && !isAttacking)
		{
			CleanUpDeadMinions();
			if (activeMinions.Count < maxActiveMinions)
			{
				Attack_Summon(); // Kích ho?t animation Attack cho tri?u h?i
				summonTimer = summonCooldown;
			}
		}

		// --- Logic Ground Slam ---
		// Ground Slam ch? khi không ?ang t?n công hành ??ng khác và player ? g?n (tùy ch?n)
		// Ví d?: Slam khi player ? g?n và cooldown cho phép
		if (player != null && currentSlamCooldown <= 0f && !isAttacking)
		{
			float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
			if (distanceToPlayer <= slamRadius * 1.5f) // Kích ho?t Slam khi player ? trong ph?m vi nh?t ??nh
			{
				Attack_GroundSlam(); // Kích ho?t animation Attack cho Ground Slam
				currentSlamCooldown = slamCooldown;
			}
		}
	}

	// --- HÀM T?N CÔNG: TRI?U H?I MINION ---
	void Attack_Summon()
	{
		if (IsDead()) return;
		isAttacking = true; // ?ánh d?u boss ?ang t?n công
		animator?.SetTrigger("Attack"); // Kích ho?t trigger "Attack"

		// G?i hàm SummonMinion() sau m?t kho?ng th?i gian ng?n ?? ??ng b? v?i animation
		Invoke(nameof(SummonMinion), attackAnimationDuration * 0.5f);

		// Reset tr?ng thái t?n công sau khi animation k?t thúc
		Invoke(nameof(ResetAttackState), attackAnimationDuration);
		Debug.Log("Boss b?t ??u tri?u h?i minion!");
	}

	// --- HÀM T?N CÔNG: ??A CH?N ---
	void Attack_GroundSlam()
	{
		if (IsDead()) return;
		isAttacking = true; // ?ánh d?u boss ?ang t?n công
		animator?.SetTrigger("Attack"); // Kích ho?t trigger "Attack" (ho?c trigger riêng n?u b?n có animation khác cho Slam)

		// G?i hàm PerformGroundSlam() sau m?t kho?ng th?i gian ng?n ?? ??ng b? v?i animation
		Invoke(nameof(PerformGroundSlam), attackAnimationDuration * 0.7f); // Slam th??ng ? cu?i animation

		// Reset tr?ng thái t?n công sau khi animation k?t thúc
		Invoke(nameof(ResetAttackState), attackAnimationDuration);
		Debug.Log("Boss b?t ??u ??a ch?n!");
	}

	// --- RESET TR?NG THÁI T?N CÔNG ---
	void ResetAttackState()
	{
		isAttacking = false;
		Debug.Log("Attack state reset.");
	}

	// --- HÀM TRI?U H?I MINION ---
	void SummonMinion()
	{
		if (minionPrefab == null)
		{
			Debug.LogWarning("Minion Prefab ch?a ???c gán cho BossSummoner!");
			return;
		}

		Vector3 spawnPos = transform.position;
		if (summonPoints != null && summonPoints.Length > 0)
		{
			spawnPos = summonPoints[Random.Range(0, summonPoints.Length)].position;
		}

		GameObject newMinion = Instantiate(minionPrefab, spawnPos, Quaternion.identity);
		activeMinions.Add(newMinion);
        audioSource?.PlayOneShot(summonSound);
        Debug.Log("Boss ?ã tri?u h?i m?t minion!");
	}

	// --- HÀM D?N D?P MINION ?Ã CH?T ---
	void CleanUpDeadMinions()
	{
		activeMinions.RemoveAll(item => item == null);
	}

	// --- HÀM B?N C?U L?A T?A TRÒN (KHI B? T?N CÔNG) ---
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
				rb.linearVelocity = direction * 4f;
			}
		}
		Debug.Log($"Boss b?n {radialFireballOnHitCount} c?u l?a t?a tròn khi b? t?n công!");
	}

	// --- HÀM TH?C HI?N ??A CH?N ---
	void PerformGroundSlam()
	{
		Debug.Log("Boss th?c hi?n Ground Slam!");

		if (slamEffectPrefab != null)
		{
			Instantiate(slamEffectPrefab, transform.position, Quaternion.identity);
		}

		Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, slamRadius);

		foreach (Collider2D obj in hitObjects)
		{
			if (obj.CompareTag("Player"))
			{
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

		if (teleportEffectPrefab != null)
		{
			GameObject effectDisappear = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
			Destroy(effectDisappear, 1f);
		}

		if (summonPoints != null && summonPoints.Length > 0)
		{
			Transform bestTeleportPoint = null;
			float maxDistanceToPlayer = -1f;

			if (player == null || player.transform == null)
			{
				Debug.LogWarning("Player không tìm th?y khi Boss c? g?ng d?ch chuy?n!");
				transform.position = summonPoints[Random.Range(0, summonPoints.Length)].position;
				if (teleportEffectPrefab != null)
				{
					GameObject effectAppear = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
					Destroy(effectAppear, 1f);
				}
				return;
			}

			foreach (Transform point in summonPoints)
			{
				float distanceToPlayerFromPoint = Vector2.Distance(point.position, player.transform.position);

				if (distanceToPlayerFromPoint > maxDistanceToPlayer && distanceToPlayerFromPoint >= minTeleportDistance)
				{
					maxDistanceToPlayer = distanceToPlayerFromPoint;
					bestTeleportPoint = point;
				}
			}

			if (bestTeleportPoint != null)
			{
				transform.position = bestTeleportPoint.position;
				Debug.Log($"Boss d?ch chuy?n ??n ?i?m xa nh?t kh?i Player: {bestTeleportPoint.name} (Cách Player: {maxDistanceToPlayer:F2})");

				if (teleportEffectPrefab != null)
				{
					GameObject effectAppear = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
					Destroy(effectAppear, 1f);
				}
			}
			else
			{
				Debug.LogWarning("Không tìm ???c ?i?m d?ch chuy?n ?? xa kh?i Player. D?ch chuy?n ng?u nhiên m?t ?i?m b?t k?.");
				transform.position = summonPoints[Random.Range(0, summonPoints.Length)].position;
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
	public override void TakeDamage(float damage, Vector2 knockback)
	{
		if (IsDead()) return; // Không nh?n sát th??ng n?u ?ã ch?t

		Debug.Log($"Boss Tri?u H?i nh?n {damage} damage, HP tr??c khi tr?: {currentHp}");

		currentHp -= damage;
		currentHp = Mathf.Max(currentHp, 0);

		UpdateHealthBar();

		Debug.Log("HP sau khi tr?: " + currentHp);

		if (currentHp <= 0)
		{
			Debug.Log("Boss Tri?u H?i ch?t, g?i Die()");
			Die();
		}
		else
		{
			animator?.SetTrigger("TakeHit"); // Kích ho?t trigger "TakeHit"
			base.ApplyKnockback(knockback);

			// --- KÍCH HO?T COMBO PH?N ?NG KHI B? T?N CÔNG (B?n c?u l?a + D?ch chuy?n) ---
			if (currentHitReactionComboCooldown <= 0f)
			{
				ShootRadialFireballsOnHit();
				PerformTeleport();
				currentHitReactionComboCooldown = hitReactionComboCooldown;
			}

			// --- KÍCH HO?T ??A CH?N (n?u cooldown cho phép và mu?n nó là ph?n ?ng ??c l?p) ---
			// ??a ch?n s? kích ho?t ??c l?p v?i combo b?n c?u l?a/d?ch chuy?n
			// L?u ý: N?u mu?n Slam là m?t Attack animation riêng bi?t, b?n c?n trigger riêng
			// ho?c có m?t logic ph?c t?p h?n ?? xác ??nh khi nào Slam s? kích ho?t Attack trigger chung.
			// Hi?n t?i, Ground Slam c?ng s? kích ho?t trigger "Attack" n?u b?n g?i Attack_GroundSlam() t? ?ây.
			// Tuy nhiên, vi?c Slam có cooldown riêng ?ã ???c ki?m soát ? Update().
		}
	}

	// --- OVERRIDE DIE ---
	protected override void Die()
	{
		if (IsDead()) return; // ??m b?o ch? ch?t m?t l?n
							  //isDead = true;
		
		animator?.SetTrigger("Die"); // Kích ho?t trigger "Die"
        audioSource?.PlayOneShot(dieSound);
        Debug.Log("Boss Tri?u H?i ?ã b? ?ánh b?i!");

		// 1. Phá h?y t?t c? các minion ?ang ho?t ??ng
		foreach (GameObject minion in activeMinions)
		{
			if (minion != null)
			{
				Destroy(minion);
			}
		}
		activeMinions.Clear();

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
		base.Die();
		// 3. H?y ??i t??ng boss sau m?t kho?ng th?i gian, cho phép animation Die ch?y h?t
		//Destroy(gameObject, 3f); // Ví d?: h?y sau 3 giây
								 // base.Die(); // Không g?i base.Die() n?u b?n ?ã Destroy(gameObject, th?i gian)
	}

	// --- OVERRIDE MOVE TO PLAYER ---
	// Boss này ??ng yên, không di chuy?n theo logic m?c ??nh c?a EnemyController.
	// Do ?ó, chúng ta không c?n trigger "Move" (bool) cho Animator ? ?ây.
	protected override void MoveToPlayer() { /* Boss ??ng yên, không di chuy?n */ }
}