using Assets.Scripts.Controller;
using Assets.Scripts.Controller.Enemy.EnemyLv2;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float maxHp = 100f;
    public float currentHp;
    [SerializeField] private Image healthBar;

    [SerializeField] private int maxEnergy = 10;
    private float currentEnergy;
    [SerializeField] private Image energyBar;

    [SerializeField] private int maxExp = 10;
    private float currentExp;
    //[SerializeField] private Image expBar;
    [SerializeField] private int level = 1;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private AttackDetector attackDetector;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Biến để kiểm tra trạng thái tấn công
    private bool isAttacking = false;

    // --- THÊM DÒNG NÀY ---
    [SerializeField] private GameObject attackHitbox; // Kéo GameObject AttackHitbox của bạn vào đây từ Inspector

    private float lastHitTime = -999f;
    [SerializeField] private float hitCooldown = 0.5f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip takeHitClip;

    [SerializeField] private GameObject swordSpinPrefab;
    [SerializeField] private Transform spinCenter;

    [SerializeField] private GameObject circleEffectPrefab;
    private GameObject currentCircleEffect;

    private bool hasSwordSpin = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        animator = rb.GetComponent<Animator>();
    }

    void Start()
    {
        currentHp = maxHp;
        UpdateHealthBar();

        currentEnergy = 0;
        UpdateEnergyBar();

        currentExp = 0;
        UpdateExpBar();
        gameManager.UpdateLevelUI(level);

        // --- TẮT HITBOX KHI BẮT ĐẦU GAME (Nếu bạn chưa tắt nó trong Editor) ---
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }

        if (PlayerState.acquiredSwordSpin)
        {
            ShowCircleEffect();
            ActivateSwordSpin();
            //PlayerState.acquiredSwordSpin = false;
        }
    }

    void Update()
    {
        Movement();

        // Kiểm tra input tấn công
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            TakeDamage(10f);
        }
    }

    void Movement()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Chỉ di chuyển khi không tấn công
        if (!isAttacking)
        {
            rb.linearVelocity = playerInput * moveSpeed;

            // Xoay mặt khi move
            if (playerInput.x < 0)
                spriteRenderer.flipX = true;
            else if (playerInput.x > 0)
                spriteRenderer.flipX = false;

            //Set animation khi move
            if (playerInput != Vector2.zero)
                animator.SetBool("isMove", true); // Điều khiển biến isMove trong Animator
            else
                animator.SetBool("isMove", false); // Điều khiển biến isMove trong Animator
        }
        else // Khi đang tấn công, dừng di chuyển
        {
            rb.linearVelocity = Vector2.zero;
            // Có thể giữ animation idle hoặc animation đang chạy nếu bạn muốn nhân vật vẫn hiển thị tư thế đó khi chém
            // Tuy nhiên, thường thì khi chém, animation chém sẽ ưu tiên.
        }
    }

    void Attack()
    {
        isAttacking = true; // Đặt cờ đang tấn công là true
        animator.SetTrigger("Attack"); // Kích hoạt Trigger "AttackTrigger" trong Animator
                                       // Animator sẽ tự động chọn HeroAtt1 hoặc HeroAtt2 dựa vào điều kiện isMove
        if (audioSource != null && attackClip != null)
        {
            audioSource.PlayOneShot(attackClip);
        }
    }

    // Hàm này sẽ được gọi từ Animation Event của cả HeroAtt1 và HeroAtt2
    public void EndAttack()
    {
        isAttacking = false;
        // Sau khi attack kết thúc, nếu người chơi vẫn giữ phím di chuyển,
        // hàm Movement() trong Update sẽ tự động đặt lại isMove = true
        // và nhân vật sẽ quay về HeroRun.
        // Bạn KHÔNG nên gọi DisableHitbox() ở đây nếu bạn đã đặt nó làm một Animation Event riêng.
        // Gọi ở đây có thể tắt hitbox quá muộn hoặc không đồng bộ với animation.
    }

    // --- DÁN ĐOẠN CODE NÀY VÀO ĐÂY (ngoài các hàm Start, Update, v.v., nhưng trong class PlayerController) ---
    public void EnableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true); // Bật GameObject hitbox
                                          // Nếu cần, điều chỉnh vị trí và kích thước hitbox tại đây
                                          // Ví dụ: attackHitbox.transform.localPosition = new Vector3(0.5f, 0, 0); // Di chuyển hitbox tương đối với Player
                                          // attackHitbox.GetComponent<BoxCollider2D>().size = new Vector2(0.8f, 0.8f); // Thay đổi kích thước collider
            Debug.Log("Attack Hitbox: ENABLED!");
        }
        else
        {
            Debug.LogError("Attack Hitbox is not assigned in PlayerController!");
        }
    }

    public void DisableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false); // Tắt GameObject hitbox
            Debug.Log("Attack Hitbox: DISABLED!");
        }
        else
        {
            Debug.LogError("Attack Hitbox is not assigned in PlayerController!");
        }
    }
    // --- KẾT THÚC DÁN ---

    public void TakeDamage(float damage)
    {
        if (Time.time - lastHitTime < hitCooldown) return; // Nếu chưa đủ thời gian thì bỏ qua

        lastHitTime = Time.time;
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHealthBar();

        if (currentHp <= 0)
        {
            animator.ResetTrigger("TakeHit");
            animator.SetTrigger("Die");
            animator.SetBool("isDead", true);
            Die();
        }
        else
        {
            animator.SetTrigger("TakeHit");
            if (audioSource != null && takeHitClip != null)
            {
                audioSource.PlayOneShot(takeHitClip);
            }
        }
    }

    public void Heal(float healValue)
    {
        if (currentHp < maxHp)
        {
            currentHp += healValue;
            currentHp = Mathf.Min(currentHp, maxHp);
            UpdateHealthBar();
        }
    }

    public void GetEnergy(float energy)
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energy;
            currentEnergy = Mathf.Min(currentEnergy, maxEnergy);
            UpdateEnergyBar();
        }
    }

    public void GetExp(float exp)
    {
        if (currentExp < maxExp)
        {
            currentExp += exp;
            currentExp = Mathf.Min(currentExp, maxExp);
            UpdateExpBar();
        }
        else
        {
            LevelUp();
        }
    }

    protected void LevelUp()
    {
        level += 1;
        moveSpeed += 1;
        maxHp += 10;
        currentHp = maxHp;
        maxExp += 10;
        currentExp = 0;
        attackDetector.attackDamage += 5;

        UpdateHealthBar();
        UpdateExpBar();
        gameManager.UpdateHealthBarUI(currentHp, maxHp);
        gameManager.UpdateExpBarUI(currentExp, maxExp);
        gameManager.UpdateLevelUI(level);
    }

    protected void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHp / maxHp;
            gameManager.UpdateHealthBarUI(currentHp, maxHp);
        }
    }

    protected void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            energyBar.fillAmount = currentEnergy / maxEnergy;
            gameManager.UpdateEnergyBarUI(currentEnergy, maxEnergy);
        }
    }

    protected void UpdateExpBar()
    {
        gameManager.UpdateExpBarUI(currentExp, maxExp);
    }

    private void Die()
    {
        // Kích hoạt animation chết
        animator.SetTrigger("Die");
        animator.SetBool("isDead", true);

        // Ngăn chặn các hành động khác của người chơi sau khi chết
        // Vô hiệu hóa input hoặc script này
        enabled = false; // Tắt script này
        rb.linearVelocity = Vector2.zero; // Dừng mọi chuyển động

        // Vô hiệu hóa collider để không còn va chạm nữa
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        // Tắt hitbox tấn công nếu nó đang bật
        if (attackHitbox != null && attackHitbox.activeSelf)
        {
            attackHitbox.SetActive(false);
        }

        // Tùy chọn: Hủy đối tượng sau một khoảng thời gian để animation chết có thể phát hết
        // float deathAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length; // Lấy độ dài của animation hiện tại trên layer 0
        // Destroy(gameObject, deathAnimationLength); // Hủy sau khi animation chết phát hết
        // Hoặc:
        Destroy(gameObject, 3f); // Hủy sau 3 giây (đảm bảo animation có thời gian để phát)
    }

    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public void ActivateSwordSpin()
    {
        if (hasSwordSpin) return; // Không cho kích hoạt lại nếu đã có

        hasSwordSpin = true;
        int swordCount = 3;

        for (int i = 0; i < swordCount; i++)
        {
            GameObject sword = Instantiate(swordSpinPrefab, spinCenter.position, Quaternion.identity);
            sword.transform.SetParent(spinCenter); // quay quanh player
            SwordSpin spin = sword.GetComponent<SwordSpin>();
            spin.ownerTag = "Player";
            spin.bossCenter = spinCenter;
            spin.angleOffset = i * 360f / swordCount;
            spin.InitPosition();
        }
    }

    public void ShowCircleEffect()
    {
        if (circleEffectPrefab == null) return;

        if (currentCircleEffect == null)
        {
            currentCircleEffect = Instantiate(circleEffectPrefab, transform.position, Quaternion.identity);
            currentCircleEffect.transform.SetParent(transform); // Gắn vòng tròn vào Player
            currentCircleEffect.transform.localPosition = new Vector3(0, -0.7f, 0); // điều chỉnh xuống chân
        }
    }
}
