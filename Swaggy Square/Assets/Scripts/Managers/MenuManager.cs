using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    public GameObject pauseMenu;
    [SerializeField]
    private PlayerController playerController;
    public bool menuIsOpen = false;
    public bool selectorMenu = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !selectorMenu)
        {
            menuIsOpen = !menuIsOpen;

            if (menuIsOpen)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
                
            else if (!menuIsOpen)
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
            }
        }
    }

    private void Start()
    {
        
    }

    public void MenuReturn()
    {
        SceneManager.LoadScene(sceneBuildIndex: 0);

        selectorMenu = true;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void Return()
    {
        menuIsOpen = !menuIsOpen;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);

        if (playerController.isPlayerDead)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
