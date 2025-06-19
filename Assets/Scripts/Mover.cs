using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveForce = 1f;
    Vector2 moveInput;

    [Header("Thrust")]
    [SerializeField] float thrustForce = 1500f;
    bool thrustInput;

    [Header("Camera")]
    [SerializeField] float fieldOfViewDefault = 50f;
    [SerializeField] float fieldOfViewOnThrust = 100f;
    [SerializeField] float fieldOfViewDuration = 2f;
    [SerializeField] CinemachineCamera myCinemachineCamera;
    [SerializeField] Transform cameraTransform;

    Rigidbody myRigidody;
    ParticleSystem myParticleSystem;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidody = GetComponent<Rigidbody>();
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        myCinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        cameraTransform = myCinemachineCamera.transform;

        myCinemachineCamera.Lens.FieldOfView = fieldOfViewDefault;
    }

    // Update is called once per time
    void FixedUpdate()
    {
        UpdateMove();
        UpdateThrust();
    }

    // on move
    void OnMove(InputValue value)
    {
        // input
        moveInput = value.Get<Vector2>();

        //Debug.Log(moveInput.x + " " + moveInput.y);
        //Debug.Log(transform.rotation.x + " " + transform.rotation.y + " " + transform.rotation.y);

        // set velocity
        //Vector3 velocity = new Vector3(moveInput.x * currentSpeed, myRigidody.linearVelocity.y, moveInput.y * currentSpeed);
        //myRigidody.linearVelocity = velocity;

        // set rotation
        //Vector3 rotation = new Vector3(transform.rotation.x + (moveInput.x * 0.1f), transform.rotation.y, transform.rotation.x + (moveInput.y * 0.1f));
        //transform.rotation = Quaternion.Euler(rotation);

        //hasHorizontalSpeed = Mathf.Abs(myRigidody.linearVelocity.x) > Mathf.Epsilon; 
        //animator.SetBool("isWalking", hasHorizontalSpeed);
    }

    // on jump
    void OnJump(InputValue value)
    {
        thrustInput = value.isPressed;
    }

    // update thrust
    void UpdateThrust()
    {
        if (thrustInput)
        {
            //Debug.Log("thrusting");
            myRigidody.AddRelativeForce(Vector3.up * thrustForce);

            myParticleSystem.Play();
            myCinemachineCamera.Lens.FieldOfView = Mathf.SmoothStep(myCinemachineCamera.Lens.FieldOfView, fieldOfViewOnThrust, fieldOfViewDuration);
        }
        else
        {
            myParticleSystem.Stop();
            myCinemachineCamera.Lens.FieldOfView = Mathf.SmoothStep(myCinemachineCamera.Lens.FieldOfView, fieldOfViewDefault, fieldOfViewDuration);
        }
    }

    // update move
    void UpdateMove()
    {
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
    }


    // because we call it every time and we need to know which sign it has at this exact frame
    void ApplyRotation(Vector3 rotation)
    {
        myRigidody.freezeRotation = true;  // freezing rotation so we can manually rotate
        transform.Rotate(rotation);
        myRigidody.freezeRotation = false;  // unfreezing rotation so the physics system can take over
    }

}
