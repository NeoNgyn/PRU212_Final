using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float maxHp = 100f;
    private float currentHp;
    [SerializeField] private Image healthBar;

    [SerializeField] private int maxEnergy = 10;
    private float currentEnergy;
    [SerializeField] private Image energyBar;

    [SerializeField] private GameManager gameManager;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
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
    }

    void Update()
    {
        Movement();
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

        rb.linearVelocity = playerInput * moveSpeed;

        // Xoay mặt khi move
        if (playerInput.x < 0)
            spriteRenderer.flipX = true;
        else if (playerInput.x > 0)
            spriteRenderer.flipX = false;

        //Set animation khi move
        if (playerInput != Vector2.zero)
            animator.SetBool("isMove", true);
        else
            animator.SetBool("isMove", false);

    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHealthBar();

        if (currentHp <= 0)
            Die();
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

    private void Die()
    {
        Destroy(gameObject);
    }
}
