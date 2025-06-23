using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField][Min(1)] float nextSceneDelay = 1f;

    LoadingHandler loadingHandler;
    GameObject rocket;

    // Awake is called once before the Start
    void Awake()
    {
        loadingHandler = FindFirstObjectByType<LoadingHandler>();
        rocket = (GameObject.FindWithTag("Player"));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingHandler.StopLoading();
        rocket.GetComponent<CollisionHandler>().enabled = false;
        rocket.GetComponent<PlayerController>().DisableMove();
        rocket.GetComponent<PlayerController>().DisableThrust();
        rocket.GetComponent<PlayerController>().DisableCamera();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // when home button is clicked
    public void OnHomeClick()
    {
        //Debug.Log("OnHomeClick");
        loadingHandler.StartLoading();
        Invoke(nameof(LoadHomeScene), nextSceneDelay);
    }

    // load next level
    void LoadHomeScene()
    {
        SceneManager.LoadScene(0);
    }
}
