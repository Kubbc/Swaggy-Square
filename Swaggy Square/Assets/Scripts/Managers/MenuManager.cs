using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    private bool menuIsOpen = false;
    public static MenuManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Breach()
    {
        SceneManager.LoadScene(sceneBuildIndex:0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuIsOpen = !menuIsOpen;

            if (menuIsOpen)
            {
                Time.timeScale = 0;
                menu.SetActive(true);
            }
                
            else if (!menuIsOpen)
            {
                Time.timeScale = 1;
                menu.SetActive(false);
            }
        }
    }
}
