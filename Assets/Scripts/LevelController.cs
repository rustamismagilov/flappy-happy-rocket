using System.Collections;
using System.Net.Sockets;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Header("Start")]
    [SerializeField][Min(3)] float timeBeforeStart = 3f;

    [Header("On success")]
    [SerializeField] float loadNextLevelDelay = 2f;
    [SerializeField] AudioClip successAudioClip;

    [Header("On faild")]
    [SerializeField] float reloadCurrentLevelDelay = 2f;
    [SerializeField] AudioClip faildAudioClip;

    LoadingController loadingController;
    CinemachineCamera myCinemachineCamera;
    GameObject rocket;
    GameObject startPlatform;
    GameObject finishPlatform;
    AudioSource finishAudioSource;

    bool isFinished = false;
    bool isCrashed = false;

    // Awake is called once before the Start
    void Awake()
    {
        loadingController = FindFirstObjectByType<LoadingController>();
        myCinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        rocket = (GameObject.FindWithTag("Player"));
        startPlatform = (GameObject.FindWithTag("Start"));
        finishPlatform = (GameObject.FindWithTag("Finish"));
        finishAudioSource = finishPlatform.GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingController.DarkToTransparent(); // Show loading with animation

        /* IMPORTANT!!! 
        * AS SOON AS YOU WILL IMPLEMENT SEQUENCE WHICH WILL SHOW THE LOADING SCREEN OR GAME OVER SCREEN (E.G. WITH FINAL SCORE OR TIME) 
        * FOR THIS PARTICULAR LEVEL, YOU SHOULD UNCOMMENT AND PUT THIS LINE IN THERE
        loadingController.TransparentToDark();
        */

        StartCoroutine(StartLevel());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // bring the rocket in the start platform
    IEnumerator StartLevel()
    {
        // disable rocket commands
        rocket.GetComponent<CollisionHandler>().enabled = false;
        rocket.GetComponent<PlayerController>().enabled = true;
        rocket.GetComponent<PlayerController>().DisableMove();
        rocket.GetComponent<PlayerController>().DisableThrust();
        rocket.GetComponent<PlayerController>().DisableCamera();
        rocket.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, 0);
        rocket.GetComponent<Rigidbody>().freezeRotation = true;

        // reset camera and rocket rotation to the right position for moves
        myCinemachineCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        rocket.transform.rotation = Quaternion.Euler(0, 0, 0);

        // bring the rocket to the platform
        rocket.transform.position = new Vector3(startPlatform.transform.position.x, startPlatform.transform.position.y + 7, startPlatform.transform.position.z);
        rocket.GetComponent<PlayerController>().thrustForced = true;
        rocket.GetComponent<PlayerController>().thrustPowerForced = 800;

        // wait the time passed
        yield return new WaitForSeconds(timeBeforeStart);

        // disable forced thrust
        rocket.GetComponent<PlayerController>().thrustForced = null;
        rocket.GetComponent<PlayerController>().thrustPowerForced = null;

        // enable player input again
        rocket.GetComponent<CollisionHandler>().enabled = true;
        rocket.GetComponent<PlayerController>().EnableMove();
        rocket.GetComponent<PlayerController>().EnableThrust();
        rocket.GetComponent<PlayerController>().EnableCamera();
        rocket.GetComponent<Rigidbody>().freezeRotation = false;
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
    public void StartSuccessSequence(Collision collision)
    {
        if (isFinished || isCrashed) return;

        isFinished = true;
        collision.gameObject.GetComponent<MeshRenderer>().material.color = UnityEngine.Color.blue;
        finishAudioSource.PlayOneShot(successAudioClip);
        rocket.GetComponent<PlayerController>().enabled = false;
        Invoke(nameof(LoadNextLevel), loadNextLevelDelay);

        loadingController.DarkToTransparent();
    }

    // when the lavel is failed
    public void StartFailedSequence()
    {
        if (isCrashed || isFinished) return;

        isCrashed = true;
        rocket.GetComponent<PlayerController>().enabled = false;
        Invoke(nameof(ReloadCurrentLevel), reloadCurrentLevelDelay);

        loadingController.gameObject.SetActive(true);
        loadingController.DarkToTransparent();
    }
}
