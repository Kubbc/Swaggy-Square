using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorMenuManager : MonoBehaviour
{
    public void Breach()
    {
        MenuManager.Instance.selectorMenu = false;
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }
}
