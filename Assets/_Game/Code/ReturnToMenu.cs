using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    // sends player back to the main menu when referenced
    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
