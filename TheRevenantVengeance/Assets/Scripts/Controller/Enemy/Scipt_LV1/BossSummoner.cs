using UnityEngine;
using System.Collections.Generic;

public class BossSummoner : EnemyController // K? th?a t? EnemyController
{
	// --- BI?N ANIMATOR V� TR?NG TH�I ---
	private Animator animator; // Th�m Animator
	private bool isAttacking = false; // ?? ki?m so�t tr?ng th�i t?n c�ng c?a Boss (v� d?: ?ang tri?u h?i, ?ang Ground Slam)
	//protected bool isDead = false; // ?? ki?m so�t tr?ng th�i ch?t c?a Boss

	// --- C�I ??T CHUNG ---
	[Header("General Boss Settings")]
	[SerializeField] private float attackAnimationDuration = 1.0f; // Th?i gian ch?y animation Attack (Summon ho?c Slam)


	// --- C�I ??T TRI?U H?I MINION ---
	[Header("Summoner Settings")]
	[SerializeField] private GameObject minionPrefab;
	[SerializeField] private float summonCooldown = 5f;
	[SerializeField] private Transform[] summonPoints;
	[SerializeField] private int maxActiveMinions = 3;

	private float summonTimer;
	private List<GameObject> activeMinions = new List<GameObject>();

	// --- C�I ??T C?NG ---
	[Header("Gate Settings")]
	[SerializeField] private GateTriggerBoss gateTrigger;

	// --- C�I ??T ??A CH?N ---
	[Header("Ground Slam Settings")]
	[SerializeField] private float slamRadius = 3f;
	[SerializeField] private float slamDamage = 20f;
	[SerializeField] private float slamCooldown = 4f;
	[SerializeField] private GameObject slamEffectPrefab;

	private float currentSlamCooldown;

	// --- C�I ??T COMBO PH?N ?NG KHI B? T?N C�NG (B?n c?u l?a + D?ch chuy?n) ---
	[Header("Hit Reaction Combo Settings")]
	[SerializeField] private GameObject fireballPrefab;
	[SerializeField] private int radialFireballOnHitCount = 8;
	[SerializeField] private float hitReactionComboCooldown = 5f;
	private float currentHitReactionComboCooldown;

	[Header("Teleport Settings")]
	[SerializeField] private float teleportCooldown = 6f; // Cooldown ri�ng cho Teleport n?u mu?n t�ch ra
	[SerializeField] private GameObject teleportEffectPrefab;
	[SerializeField] private float minTeleportDistance = 8f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip summonSound;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioSource audioSource;
    // Override h�m Awake ?? l?y component Animator
    protected override void Awake()
	{
		base.Awake();
		animator = GetComponent<Animator>();
	}

	// Override h�m Start ?? kh?i t?o t?t c? c�c th�nh ph?n c?a boss
	protected override void Start()
	{
		base.Start(); // G?i h�m Start c?a EnemyController ?? kh?i t?o HP (currentHp = maxHp) v� c�c th? kh�c.

		// Kh?i t?o c�c b? ??m th?i gian
		summonTimer = summonCooldown;
		currentSlamCooldown = slamCooldown;
		currentHitReactionComboCooldown = hitReactionComboCooldown;
	}

	// Override h�m Update ?? x? l� t?t c? logic c?a boss
	protected override void Update()
	{
		if (IsDead()) return; // N?u ch?t th� kh�ng l�m g� n?a

		// C?p nh?t t?t c? c�c b? ??m th?i gian
		summonTimer -= Time.deltaTime;
		currentSlamCooldown -= Time.deltaTime;
		currentHitReactionComboCooldown -= Time.deltaTime;

		// --- Logic Tri?u h?i Minion ---
		// Tri?u h?i ch? khi kh�ng ?ang t?n c�ng h�nh ??ng kh�c
		if (summonTimer <= 0f && !isAttacking)
		{
			CleanUpDeadMinions();
			if (activeMinions.Count < maxActiveMinions)
			{
				Attack_Summon(); // K�ch ho?t animation Attack cho tri?u h?i
				summonTimer = summonCooldown;
			}
		}

		// --- Logic Ground Slam ---
		// Ground Slam ch? khi kh�ng ?ang t?n c�ng h�nh ??ng kh�c v� player ? g?n (t�y ch?n)
		// V� d?: Slam khi player ? g?n v� cooldown cho ph�p
		if (player != null && currentSlamCooldown <= 0f && !isAttacking)
		{
			float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
			if (distanceToPlayer <= slamRadius * 1.5f) // K�ch ho?t Slam khi player ? trong ph?m vi nh?t ??nh
			{
				Attack_GroundSlam(); // K�ch ho?t animation Attack cho Ground Slam
				currentSlamCooldown = slamCooldown;
			}
		}
	}

	// --- H�M T?N C�NG: TRI?U H?I MINION ---
	void Attack_Summon()
	{
		if (IsDead()) return;
		isAttacking = true; // ?�nh d?u boss ?ang t?n c�ng
		animator?.SetTrigger("Attack"); // K�ch ho?t trigger "Attack"

		// G?i h�m SummonMinion() sau m?t kho?ng th?i gian ng?n ?? ??ng b? v?i animation
		Invoke(nameof(SummonMinion), attackAnimationDuration * 0.5f);

		// Reset tr?ng th�i t?n c�ng sau khi animation k?t th�c
		Invoke(nameof(ResetAttackState), attackAnimationDuration);
		Debug.Log("Boss b?t ??u tri?u h?i minion!");
	}

	// --- H�M T?N C�NG: ??A CH?N ---
	void Attack_GroundSlam()
	{
		if (IsDead()) return;
		isAttacking = true; // ?�nh d?u boss ?ang t?n c�ng
		animator?.SetTrigger("Attack"); // K�ch ho?t trigger "Attack" (ho?c trigger ri�ng n?u b?n c� animation kh�c cho Slam)

		// G?i h�m PerformGroundSlam() sau m?t kho?ng th?i gian ng?n ?? ??ng b? v?i animation
		Invoke(nameof(PerformGroundSlam), attackAnimationDuration * 0.7f); // Slam th??ng ? cu?i animation

		// Reset tr?ng th�i t?n c�ng sau khi animation k?t th�c
		Invoke(nameof(ResetAttackState), attackAnimationDuration);
		Debug.Log("Boss b?t ??u ??a ch?n!");
	}

	// --- RESET TR?NG TH�I T?N C�NG ---
	void ResetAttackState()
	{
		isAttacking = false;
		Debug.Log("Attack state reset.");
	}

	// --- H�M TRI?U H?I MINION ---
	void SummonMinion()
	{
		if (minionPrefab == null)
		{
			Debug.LogWarning("Minion Prefab ch?a ???c g�n cho BossSummoner!");
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
        Debug.Log("Boss ?� tri?u h?i m?t minion!");
	}

	// --- H�M D?N D?P MINION ?� CH?T ---
	void CleanUpDeadMinions()
	{
		activeMinions.RemoveAll(item => item == null);
	}

	// --- H�M B?N C?U L?A T?A TR�N (KHI B? T?N C�NG) ---
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
				rb.linearVelocity = direction * 4f;
			}
		}
		Debug.Log($"Boss b?n {radialFireballOnHitCount} c?u l?a t?a tr�n khi b? t?n c�ng!");
	}

	// --- H�M TH?C HI?N ??A CH?N ---
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
					Debug.Log($"Player b? Ground Slam g�y {slamDamage} s�t th??ng!");
				}
			}
		}
	}

	// --- H�M TH?C HI?N D?CH CHUY?N ---
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
				Debug.LogWarning("Player kh�ng t�m th?y khi Boss c? g?ng d?ch chuy?n!");
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
				Debug.Log($"Boss d?ch chuy?n ??n ?i?m xa nh?t kh?i Player: {bestTeleportPoint.name} (C�ch Player: {maxDistanceToPlayer:F2})");

				if (teleportEffectPrefab != null)
				{
					GameObject effectAppear = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
					Destroy(effectAppear, 1f);
				}
			}
			else
			{
				Debug.LogWarning("Kh�ng t�m ???c ?i?m d?ch chuy?n ?? xa kh?i Player. D?ch chuy?n ng?u nhi�n m?t ?i?m b?t k?.");
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
			Debug.LogWarning("Kh�ng c� Summon Points n�o ???c g�n ?? Boss d?ch chuy?n!");
		}
	}

	// --- OVERRIDE TAKE DAMAGE ---
	public override void TakeDamage(float damage, Vector2 knockback)
	{
		if (IsDead()) return; // Kh�ng nh?n s�t th??ng n?u ?� ch?t

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
			animator?.SetTrigger("TakeHit"); // K�ch ho?t trigger "TakeHit"
			base.ApplyKnockback(knockback);

			// --- K�CH HO?T COMBO PH?N ?NG KHI B? T?N C�NG (B?n c?u l?a + D?ch chuy?n) ---
			if (currentHitReactionComboCooldown <= 0f)
			{
				ShootRadialFireballsOnHit();
				PerformTeleport();
				currentHitReactionComboCooldown = hitReactionComboCooldown;
			}

			// --- K�CH HO?T ??A CH?N (n?u cooldown cho ph�p v� mu?n n� l� ph?n ?ng ??c l?p) ---
			// ??a ch?n s? k�ch ho?t ??c l?p v?i combo b?n c?u l?a/d?ch chuy?n
			// L?u �: N?u mu?n Slam l� m?t Attack animation ri�ng bi?t, b?n c?n trigger ri�ng
			// ho?c c� m?t logic ph?c t?p h?n ?? x�c ??nh khi n�o Slam s? k�ch ho?t Attack trigger chung.
			// Hi?n t?i, Ground Slam c?ng s? k�ch ho?t trigger "Attack" n?u b?n g?i Attack_GroundSlam() t? ?�y.
			// Tuy nhi�n, vi?c Slam c� cooldown ri�ng ?� ???c ki?m so�t ? Update().
		}
	}

	// --- OVERRIDE DIE ---
	protected override void Die()
	{
		if (IsDead()) return; // ??m b?o ch? ch?t m?t l?n
							  //isDead = true;
		
		animator?.SetTrigger("Die"); // K�ch ho?t trigger "Die"
        audioSource?.PlayOneShot(dieSound);
        Debug.Log("Boss Tri?u H?i ?� b? ?�nh b?i!");

		// 1. Ph� h?y t?t c? c�c minion ?ang ho?t ??ng
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
			Debug.LogWarning("GateTrigger ch?a ???c g�n v�o BossSummoner!");
		}
		base.Die();
		// 3. H?y ??i t??ng boss sau m?t kho?ng th?i gian, cho ph�p animation Die ch?y h?t
		//Destroy(gameObject, 3f); // V� d?: h?y sau 3 gi�y
								 // base.Die(); // Kh�ng g?i base.Die() n?u b?n ?� Destroy(gameObject, th?i gian)
	}

	// --- OVERRIDE MOVE TO PLAYER ---
	// Boss n�y ??ng y�n, kh�ng di chuy?n theo logic m?c ??nh c?a EnemyController.
	// Do ?�, ch�ng ta kh�ng c?n trigger "Move" (bool) cho Animator ? ?�y.
	protected override void MoveToPlayer() { /* Boss ??ng y�n, kh�ng di chuy?n */ }
}