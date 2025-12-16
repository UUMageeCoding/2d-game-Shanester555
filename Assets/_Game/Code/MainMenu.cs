using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // loads game when referenced
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
}
