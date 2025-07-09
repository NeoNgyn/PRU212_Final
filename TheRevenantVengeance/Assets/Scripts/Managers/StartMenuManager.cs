using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject introduction;


    void Start()
    {
        startMenu.SetActive(true);
        introduction.SetActive(false);
    }

    private void Update()
    {
        
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        introduction.SetActive(true);
    }

    public void SkipIntro()
    {
        SceneManager.LoadScene("SceneLv1.1");
    }
}
