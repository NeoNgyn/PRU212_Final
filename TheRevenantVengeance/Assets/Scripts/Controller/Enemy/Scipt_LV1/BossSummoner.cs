
using UnityEngine;
using System.Collections.Generic;

public class BossSummoner : EnemyController
{
    private Animator animator;
    private bool isAttacking = false;
    protected bool isDead = false;

    [Header("General Boss Settings")]
    [SerializeField] private float attackAnimationDuration = 1.0f;
    [SerializeField] private float shieldDuration = 3f;
    [SerializeField] private int minionPerSummon = 2;

    [Header("Summoner Settings")]
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private float summonCooldown = 5f;
    [SerializeField] private Transform[] summonPoints;
    [SerializeField] private int maxActiveMinions = 3;
    [SerializeField] private float spawnRange = 5f;

    private float summonTimer;
    private List<GameObject> activeMinions = new List<GameObject>();

    [Header("Fireball Shield Settings")]
    [SerializeField] private GameObject shieldFireballPrefab;
    [SerializeField] private int shieldFireballCount = 6;

    [Header("Radial Fireball Auto Attack Settings")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private int radialFireballCount = 8;
    [SerializeField] private float fireballAttackCooldown = 8f;
    private float currentFireballAttackCooldown = 0f;

    [Header("Teleport Settings")]
    [SerializeField] private Transform[] teleportPoints;  // Các điểm teleport cố định

    [Header("Sound Effects")]
    [SerializeField] private AudioClip summonSound;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioClip takeHitSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Drop Settings")]
    [SerializeField] private GameObject itemDropPrefab;  // Prefab món đồ rơi ra

    [SerializeField] private GateTriggerBoss gateTrigger;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        summonTimer = summonCooldown;
        currentFireballAttackCooldown = fireballAttackCooldown;
    }

    protected override void Update()
    {
        if (isDead) return;

        summonTimer -= Time.deltaTime;
        currentFireballAttackCooldown -= Time.deltaTime;

        if (summonTimer <= 0f)
        {
            CleanUpDeadMinions();
            if (activeMinions.Count < maxActiveMinions)
            {
                SummonMinion();
                summonTimer = summonCooldown;
            }
        }

        // Auto bắn fireball xung quanh theo chu kỳ
        if (currentFireballAttackCooldown <= 0f)
        {
            ShootRadialFireballs();
            currentFireballAttackCooldown = fireballAttackCooldown;
        }
    }



    void SummonMinion()
    {
        if (minionPrefab == null) return;
        animator?.SetTrigger("Attack");
        int summonCount = Mathf.Min(minionPerSummon, maxActiveMinions - activeMinions.Count);
        for (int i = 0; i < summonCount; i++)
        {
            float angle = Random.Range(0f, 360f);
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f) * Random.Range(1f, spawnRange);
            Vector3 spawnPos = transform.position + offset;

            GameObject newMinion = Instantiate(minionPrefab, spawnPos, Quaternion.identity);
            activeMinions.Add(newMinion);
            audioSource?.PlayOneShot(summonSound);
        }
    }

    void CleanUpDeadMinions()
    {
        activeMinions.RemoveAll(item => item == null);
    }

    void CreateFireballShield()
    {
        if (shieldFireballPrefab == null) return;

        for (int i = 0; i < shieldFireballCount; i++)
        {
            float angle = i * (360f / shieldFireballCount);
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f) * 2f;
            GameObject shield = Instantiate(shieldFireballPrefab, transform.position + offset, Quaternion.identity);
            shield.transform.SetParent(transform);
            shield.transform.localPosition = offset;
            shield.AddComponent<FireBallScene3>().rotationSpeed = 120f;
            Destroy(shield, shieldDuration);
        }
    }

    void ShootRadialFireballs()
    {
        if (fireballPrefab == null) return;

        for (int i = 0; i < radialFireballCount; i++)
        {
            float angle = i * (360f / radialFireballCount);
            float radian = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * 5f;
            }
        }
    }

    public override void TakeDamage(float damage, Vector2 knockback)
    {
        if (isDead) return;

        currentHp -= damage;
        UpdateHealthBar();

        animator?.SetTrigger("TakeHit");
        audioSource?.PlayOneShot(takeHitSound);
        base.ApplyKnockback(knockback);

        if (currentHp <= 0f)
        {
            Die();
        }
        else
        {
            // Chỉ tạo khiên và teleport nếu chưa chết
            CreateFireballShield();
            TeleportBoss();
        }
    }


    protected override void Die()
    {
        if (gateTrigger != null)
        {
            gateTrigger.OpenGate();
        }
        if (isDead) return;
        isDead = true;
        
        animator?.SetTrigger("Die");
        audioSource?.PlayOneShot(dieSound);

        foreach (GameObject minion in activeMinions)
        {
            if (minion != null)
                Destroy(minion);
        }
        activeMinions.Clear();

        if (itemDropPrefab != null)
        {
            Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
            Debug.Log("Item đã được rơi!");
        }
        else
        {
            Debug.LogWarning("ItemDropPrefab chưa được gán!");
        }

        // Destroy sau khi animation phát xong
        float deathAnimLength = 3f; // Hoặc animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, deathAnimLength);
    }

    void TeleportBoss()
    {
        if (teleportPoints == null || teleportPoints.Length == 0) return;

        // Teleport tới điểm ngẫu nhiên trong danh sách teleport points
        Transform targetPoint = teleportPoints[Random.Range(0, teleportPoints.Length)];
        transform.position = targetPoint.position;
        Debug.Log("Boss teleport tới điểm cố định: " + targetPoint.position);
    }

    protected override void MoveToPlayer() { }
}
