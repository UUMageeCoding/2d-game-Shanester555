using UnityEngine;
using UnityEngine.SceneManagement;

public class ToCompletionScene : MonoBehaviour
{
    // sends player to results screen when referenced
    public void CompletionScene()
    {
        SceneManager.LoadScene("ResultsScreen");
    }
}
