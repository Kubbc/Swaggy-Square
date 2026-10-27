using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    GameObject menu;
    public bool menuIsOpen = false;
    public bool selectorMenu = false;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !selectorMenu)
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

    private void Start()
    {
        menu = GetComponent<Canvas>().transform.Find("Menu")?.gameObject;
    }

    public void MenuReturn()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);

        selectorMenu = true;
        Time.timeScale = 1;
        menu.SetActive(false);
    }
    public void Return()
    {
        menuIsOpen = !menuIsOpen;
        Time.timeScale = 1;
        menu.SetActive(false);
    }
}
