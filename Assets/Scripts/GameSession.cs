using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    void Awake()
    {
        int numGameSession = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // subscribe to scene loaded event
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // unsubscribe from scene loaded event
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Home") return; // dont play fade in Home scene

        LoadingController loading = FindFirstObjectByType<LoadingController>();
        if (loading != null)
        {
            loading.DarkToTransparent();
        }
        else
        {
            Debug.LogWarning("LoadingController not found in scene: " + scene.name);
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
