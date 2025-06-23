using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    [SerializeField][Min(1)] float nextSceneDelay = 1f;

    GameObject rocket;
    LoadingController loadingController;

    // Awake is called once before the Start
    void Awake()
    {
        rocket = (GameObject.FindWithTag("Player"));
        loadingController = FindFirstObjectByType<LoadingController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingController.gameObject.SetActive(true);
        loadingController.StopLoading();
        rocket.GetComponent<CollisionHandler>().enabled = false;
        rocket.GetComponent<PlayerController>().DisableMove();
        rocket.GetComponent<PlayerController>().EnableThrust();
        rocket.GetComponent<PlayerController>().DisableCamera();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // when play next button is clicked
    public void OnPlayNextClick()
    {
        loadingController.gameObject.SetActive(true);
        loadingController.StartLoading();
        Invoke(nameof(LoadNextLevel), nextSceneDelay);
    }
    // when list of levels button is clicked
    public void OnLevelListClick()
    {
        Debug.Log("LevelListClick");
    }
    // when credits button is clicked
    public void OnCreditsClick()
    {
        //Debug.Log("OnCreditsClick");
        SceneManager.LoadScene(0);
    }

    // load next level
    void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }
}
