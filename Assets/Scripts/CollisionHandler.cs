using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Header("On success")]
    [SerializeField] float reloadCurrentLevelDelay = 2f;
    [SerializeField] AudioClip successAudioClip;

    [Header("On faild")]
    [SerializeField] float loadNextLevelDelay = 2f;

    [Header("On crash")]
    [SerializeField] AudioClip crashAudioClip;

    AudioSource myAudioSource;

    bool isFinished = false;
    bool isCrashed = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    // check if is colliding
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Start":
                //Debug.Log("This is Start point");
                break;
            case "Finish":
                //Debug.Log("This is Finish point");
                StartSuccessSequence(collision);
                break;
            case "Fuel":
                //Debug.Log("You picked up fuel");
                break;
            default:
                //Debug.Log("You hit the obstacle/ground");
                StartCrashSequence();
                break;
        }
    }


    // reload level
    void ReloadCurrentLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    // load next level
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;

        SceneManager.LoadScene(nextSceneIndex);
    }

    // when the level is succeded
    void StartSuccessSequence(Collision collision)
    {
        if (isFinished || isCrashed) return;

        isFinished = true;
        collision.gameObject.GetComponent<MeshRenderer>().material.color = UnityEngine.Color.blue;
        myAudioSource.PlayOneShot(successAudioClip);
        GetComponent<PlayerController>().enabled = false;
        Invoke(nameof(LoadNextLevel), loadNextLevelDelay);
    }

    // when the lavel is failed
    void StartCrashSequence()
    {
        myAudioSource.PlayOneShot(crashAudioClip);

        if (isCrashed || isFinished) return;

        isCrashed = true;
        GetComponent<PlayerController>().enabled = false;
        Invoke(nameof(ReloadCurrentLevel), reloadCurrentLevelDelay);
    }

}
