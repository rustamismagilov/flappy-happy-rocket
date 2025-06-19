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

    Rigidbody myRigidody;
    ParticleSystem myParticleSystem;
    CinemachineCamera myCinemachineCamera;


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
        //Vector3 rotation = new Vector3(transform.rotation.x + (moveInput.x * 0.5f), transform.rotation.y, transform.rotation.x + (moveInput.y * 0.5f));
        //transform.rotation = Quaternion.Euler(rotation);

        if (moveInput.x > 0)
        {
            //Debug.Log("Right");
            transform.Rotate(Vector3.right * moveForce);
        }
        else if (moveInput.x < 0)
        {
            //Debug.Log("Left");
            transform.Rotate(Vector3.left * moveForce);
        }

        if (moveInput.y > 0)
        {
            //Debug.Log("Forward");
            transform.Rotate(Vector3.forward * moveForce);
        }
        else if (moveInput.y < 0)
        {
            //Debug.Log("Back");
            ApplyRotation(Vector3.back * moveForce);
        }
    }

    // because we call it every time and we need to know which sign it has at this exact frame
    void ApplyRotation(Vector3 rotation)
    {
        myRigidody.freezeRotation = true;  // freezing rotation so we can manually rotate
        transform.Rotate(rotation);
        myRigidody.freezeRotation = false;  // unfreezing rotation so the physics system can take over
    }

}
