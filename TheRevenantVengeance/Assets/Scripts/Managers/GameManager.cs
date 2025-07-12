using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float countdownTime = 180f;
    private bool bossSpawned = false;

    private PlayerController player;
    public GameObject bossEnemy;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private GameObject introduction;
    [SerializeField] private GameObject playerController;
    [SerializeField] private GameObject uiComponent;



    void Start()
    {
        // Mở introduction trước khi chơi
        introduction.SetActive(true);
        playerController.SetActive(false);
        uiComponent.SetActive(false);

        UpdateTimerUI();
        bossEnemy.SetActive(false);
    }

    void Update()
    {
        if (!bossSpawned)
        {
            countdownTime -= Time.deltaTime;

            if (countdownTime <= 0)
            {
                countdownTime = 0;
                SpawnBoss();
            }

            UpdateTimerUI();
        }
    }

    public void PlayGame()
    {
        introduction.SetActive(false);
        playerController.SetActive(true);
        uiComponent.SetActive(true);

    }

    public void UpdateHealthBarUI(float currentHp, float maxHp)
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHp / maxHp;
            healthText.text = $"{currentHp} / {maxHp}";
        }
    }

    public void UpdateEnergyBarUI(float currentEnergy, float maxEnergy)
    {
        if (energyBar != null)
        {
            energyBar.fillAmount = currentEnergy / maxEnergy;
        }
    }

    public void UpdateExpBarUI(float currentExp, float maxExp)
    {
        if (expBar != null)
        {
            expBar.fillAmount = currentExp / maxExp;
        }
    }

    public void UpdateLevelUI(int level)
    {
        levelText.text = $"{level}";
    }

    void UpdateTimerUI()
    {
        if (!bossSpawned)
        {
            int minutes = Mathf.FloorToInt(countdownTime / 60f);
            int seconds = Mathf.FloorToInt(countdownTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void SpawnBoss()
    {
        bossSpawned = true;
        bossEnemy.SetActive(true);
        timerText.text = "The Overlord Descends";
        timerText.fontSize = 72;
        timerText.color = Color.red;
    }


}
