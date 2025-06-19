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
    bool thrustInput;

    [Header("Camera")]
    [SerializeField] float fieldOfViewDefault = 50f;
    [SerializeField] float fieldOfViewOnThrust = 100f;
    [SerializeField] float fieldOfViewDuration = 0.1f;
    bool isCameraControlActive = false;

    CinemachineCamera myCinemachineCamera;
    Rigidbody myRigidody;
    ParticleSystem myParticleSystem;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidody = GetComponent<Rigidbody>();
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        myCinemachineCamera = FindFirstObjectByType<CinemachineCamera>();

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
            myParticleSystem.Play();
            myCinemachineCamera.Lens.FieldOfView = Mathf.SmoothStep(myCinemachineCamera.Lens.FieldOfView, fieldOfViewOnThrust, fieldOfViewDuration);
        }
        else
        {
            // stop force
            myParticleSystem.Stop();
            myCinemachineCamera.Lens.FieldOfView = Mathf.SmoothStep(myCinemachineCamera.Lens.FieldOfView, fieldOfViewDefault, fieldOfViewDuration);
        }
    }


    // enable camera rotation only with right click
    void UpdateCamera()
    {
        if (isCameraControlActive) myCinemachineCamera.GetComponent<CinemachinePanTilt>().enabled = true;
        else myCinemachineCamera.GetComponent<CinemachinePanTilt>().enabled = false;

    }

}
