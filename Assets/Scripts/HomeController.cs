using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    GameObject rocket;

    // Awake is called once before the Start
    void Awake()
    {
        rocket = (GameObject.FindWithTag("Player"));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        SceneManager.LoadScene(1);
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
}
