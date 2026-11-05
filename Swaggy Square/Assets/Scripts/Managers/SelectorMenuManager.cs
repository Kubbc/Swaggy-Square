using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorMenuManager : MonoBehaviour
{
    public void Breach()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }
}
