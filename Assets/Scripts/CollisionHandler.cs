using System.Collections;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [Header("On collision")]
    [SerializeField] AudioClip collisionAudioClip;

    AudioSource myAudioSource;

    // Awake is called once before the Start
    void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // check if is colliding
    void OnCollisionEnter(Collision collision)
    {
        myAudioSource.PlayOneShot(collisionAudioClip);

        LevelController myLevelController = FindFirstObjectByType<LevelController>();

        switch (collision.gameObject.tag)
        {
            case "Start":
                //Debug.Log("This is Start point");
                break;
            case "Finish":
                //Debug.Log("This is Finish point");
                myLevelController.StartSuccessSequence(collision);
                break;
            case "Fuel":
                //Debug.Log("You picked up fuel");
                break;
            default:
                //Debug.Log("You hit the obstacle/ground");
                myLevelController.StartFailedSequence();
                break;
        }
    }

}
