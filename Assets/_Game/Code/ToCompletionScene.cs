using UnityEngine;
using UnityEngine.SceneManagement;

public class ToCompletionScene : MonoBehaviour
{
    public void CompletionScene()
    {
        SceneManager.LoadScene("ResultsScreen");
    }
}
