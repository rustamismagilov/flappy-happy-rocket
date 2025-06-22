using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    // Awake is called once before the Start
    void Awake()
    {
        // get the number of session
        int numGameSession = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        // if there is another game session destroy this one
        if (numGameSession > 1)
        {
            Destroy(gameObject);
        }
        // if there is no already a game session keep it alive
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // reset game if you lose
    void ResetGameSession()
    {
        FindFirstObjectByType<SceneSession>().DestroySceneSession();
        SceneManager.LoadScene(0);

        Destroy(gameObject);
    }
}
