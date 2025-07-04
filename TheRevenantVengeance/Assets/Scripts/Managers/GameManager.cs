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

    void Start()
    {
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
