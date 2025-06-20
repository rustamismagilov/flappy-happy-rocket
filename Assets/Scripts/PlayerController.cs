using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveForce = 2f;
    Vector2 moveInput;

    [Header("Thrust")]
    [SerializeField] float thrustForce = 2000f;
    [SerializeField] AudioClip thrustAudioClip;
    bool thrustInput;

    [Header("Camera")]
    [SerializeField] float fieldOfViewDefault = 50f;
    [SerializeField] float fieldOfViewOnThrust = 80f;
    [SerializeField] float fieldOfViewDuration = 2f;
    [SerializeField] float cameraSensitivityX = 0.5f;
    [SerializeField] float cameraSensitivityY = 0.5f;

    bool isCameraControlActive = false;
    Vector2 mouseInitialPosition;
    Quaternion cameraInitialRotation;

    CinemachineCamera myCinemachineCamera;
    AudioSource myAudioSource;
    Rigidbody myRigidody;
    ParticleSystem myParticleSystem;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidody = GetComponent<Rigidbody>();
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        myCinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        myAudioSource = GetComponent<AudioSource>();

        myCinemachineCamera.Lens.FieldOfView = fieldOfViewDefault;
    }

    // Update is called once per time
    void FixedUpdate()
    {
        UpdateMove();
        UpdateThrust();
        UpdateCamera();
    }


    // on move
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // on jump
    void OnJump(InputValue value)
    {
        thrustInput = value.isPressed;
    }

    // on camera
    void OnCamera(InputValue value)
    {
        isCameraControlActive = value.isPressed;
        if (isCameraControlActive)
        {
            mouseInitialPosition = Mouse.current.position.ReadValue();
            cameraInitialRotation = myCinemachineCamera.transform.rotation;
        }
    }

    // checking is moving
    void UpdateMove()
    {
        /*
        if (moveInput == Vector2.zero) return;

        // Get camera's current forward and right vectors
        Vector3 camForward = myCinemachineCamera.transform.forward;
        Vector3 camRight = myCinemachineCamera.transform.right;

        // Flatten the vectors to avoid tilting
        camForward.Normalize();
        camRight.Normalize();

        // Calculate movement direction based on input and camera orientation
        Vector3 moveDir = camRight * moveInput.x + camForward * moveInput.y;

        // Rotate rocket to face direction (optional)
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f);

        // Optional: Apply forward thrust (or adjust this if you want force instead)
        myRigidody.AddForce(moveDir * moveForce);
        */



        if (moveInput == Vector2.zero) return;

        // apply rotation
        if (moveInput.x > 0) ApplyRotation(Vector3.right * moveForce);
        else if (moveInput.x < 0) ApplyRotation(Vector3.left * moveForce);
        if (moveInput.y > 0) ApplyRotation(Vector3.forward * moveForce);
        else if (moveInput.y < 0) ApplyRotation(Vector3.back * moveForce);
    }

    // because we call it every time and we need to know which sign it has at this exact frame
    void ApplyRotation(Vector3 rotation)
    {
        myRigidody.freezeRotation = true;  // freezing rotation so we can manually rotate
        transform.Rotate(rotation);
        myRigidody.freezeRotation = false;  // unfreezing rotation so the physics system can take over
    }

    // check if is thrusting
    void UpdateThrust()
    {
        if (thrustInput)
        {
            // constantly add force
            myRigidody.AddRelativeForce(Vector3.up * thrustForce);

            myCinemachineCamera.Lens.FieldOfView = Mathf.SmoothStep(myCinemachineCamera.Lens.FieldOfView, fieldOfViewOnThrust, fieldOfViewDuration);
            if (!myParticleSystem.isPlaying) myParticleSystem.Play();
            if (!myAudioSource.isPlaying) myAudioSource.PlayOneShot(thrustAudioClip);
        }
        else
        {
            // stop force
            myCinemachineCamera.Lens.FieldOfView = Mathf.SmoothStep(myCinemachineCamera.Lens.FieldOfView, fieldOfViewDefault, fieldOfViewDuration);
            myParticleSystem.Stop();
            myAudioSource.Stop();
        }
    }

    // enable camera rotation only with right click
    void UpdateCamera()
    {
        if (isCameraControlActive)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 mouseDelta = mouseInitialPosition - mousePosition;
            myCinemachineCamera.transform.rotation = Quaternion.Euler(cameraInitialRotation.eulerAngles.x + (mouseDelta.y * cameraSensitivityX), cameraInitialRotation.eulerAngles.y + (mouseDelta.x * cameraSensitivityX), cameraInitialRotation.eulerAngles.z);
        }

    }

}
