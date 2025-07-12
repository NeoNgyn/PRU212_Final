using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    public GameObject pauseMenuUI;
    private bool isPaused = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }


    public void ResumeGame()
    {
        //Debug.LogWarning("Resume Clicked.");
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        MusicManager.Instance?.ResumeMusic();
    }

    public void PauseGame()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogWarning("Pause menu UI is still null.");
            return;
        }

        Debug.Log("Pause menu found. Activating...");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        MusicManager.Instance?.PauseMusic();
    }
    public void RestartGame()
    {
        Debug.LogWarning("Restart Clicked.");
        Time.timeScale = 1f;
        if (GameTimerManager.Instance != null)
        {
            GameTimerManager.Instance.ResetTimer();
            GameTimerManager.Instance.StartTimer();
        }

        if (MusicManager.Instance != null) MusicManager.Instance.RestartMusic();

        SceneManager.LoadScene(1);
    }
    public void QuitToStart()
    {
        Debug.LogWarning("Quit to Start Clicked.");
        Time.timeScale = 1f;
        if (GameTimerManager.Instance != null)
        {
            GameTimerManager.Instance.ResetTimer();
            GameTimerManager.Instance.StartTimer();
        }
        if (MusicManager.Instance != null) MusicManager.Instance.RestartMusic();
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Debug.LogWarning("Quit Clicked.");
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
