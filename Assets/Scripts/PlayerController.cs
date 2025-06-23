using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveForce = 2f;
    [HideInInspector] public Vector2? moveForced;
    Vector2 moveInput;
    bool moveEnabled = false;

    [Header("Thrust")]
    [SerializeField] float thrustPower = 2000f;
    [HideInInspector] public float? thrustPowerForced;
    [SerializeField] AudioClip thrustAudioClip;
    [HideInInspector] public bool? thrustForced;
    bool thrustInput;
    bool thrustEnabled = false;

    [Header("Camera")]
    [SerializeField] float fieldOfViewDefault = 50f;
    [SerializeField] float fieldOfViewOnThrust = 80f;
    [SerializeField] float fieldOfViewDuration = 2f;
    [SerializeField] float cameraSensitivityX = 0.2f;
    [SerializeField] float cameraSensitivityY = 0.2f;
    [SerializeField] bool controlYMovement = true;
    [HideInInspector] public Quaternion? cameraForced;
    bool cameraEnabled = false;

    bool isCameraControlActive = false;
    Vector2 mouseInitialPosition;
    Quaternion cameraInitialRotation;

    CinemachineCamera myCinemachineCamera;
    Rigidbody myRigidody;
    AudioSource myAudioSource;
    ParticleSystem myParticleSystem;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        myRigidody = GetComponent<Rigidbody>();
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        myAudioSource = GetComponent<AudioSource>();

        if (myCinemachineCamera) myCinemachineCamera.Lens.FieldOfView = fieldOfViewDefault;
    }

    // Update is called once per time
    void FixedUpdate()
    {
        UpdateMove();
        UpdateThrust();
        UpdateCamera();
    }


    // on camera
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
        if (!myCinemachineCamera) return;

            isCameraControlActive = value.isPressed;
        if (isCameraControlActive)
        {
            mouseInitialPosition = Mouse.current.position.ReadValue();
            cameraInitialRotation = myCinemachineCamera.transform.rotation;
        }
    }

    // enable camera
    public void EnableMove()
    {
        moveEnabled = true;
    }
    // disable camera
    public void DisableMove()
    {
        moveEnabled = false;
    }

    // enable thrust
    public void EnableThrust()
    {
        thrustEnabled = true;
    }
    // disable thrust
    public void DisableThrust()
    {
        thrustEnabled = false;
    }

    // enable camera
    public void EnableCamera()
    {
        cameraEnabled = true;
    }
    // disable camera
    public void DisableCamera()
    {
        cameraEnabled = false;
    }

    // checking is moving
    void UpdateMove()
    {
        Vector2 move;
        if (moveForced != null) move = (Vector2)moveForced;
        else if (!moveEnabled) move = new Vector2(0, 0);
        else move = moveInput;

        if (move == Vector2.zero) return;

        // apply rotation (considering the camera view)
        if (move.x > 0) ApplyRotation(Vector3.back * moveForce);
        else if (move.x < 0) ApplyRotation(Vector3.forward * moveForce);
        if (move.y > 0) ApplyRotation(Vector3.right * moveForce);
        else if (move.y < 0) ApplyRotation(Vector3.left * moveForce);
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
        bool thrust;
        if (thrustForced != null) thrust = (bool)thrustForced;
        else if (!thrustEnabled) thrust = false;
        else thrust = thrustInput;

        if (thrust)
        {
            // constantly add force
            float force = thrustPowerForced != null ? (float)thrustPowerForced : thrustPower;
            myRigidody.AddRelativeForce(Vector3.up * (force));

            if (myCinemachineCamera) myCinemachineCamera.Lens.FieldOfView = Mathf.SmoothStep(myCinemachineCamera.Lens.FieldOfView, fieldOfViewOnThrust, fieldOfViewDuration);
            if (!myParticleSystem.isPlaying) myParticleSystem.Play();
            if (!myAudioSource.isPlaying) myAudioSource.PlayOneShot(thrustAudioClip);
        }
        else
        {
            // stop force
            if (myCinemachineCamera) myCinemachineCamera.Lens.FieldOfView = Mathf.SmoothStep(myCinemachineCamera.Lens.FieldOfView, fieldOfViewDefault, fieldOfViewDuration);
            myParticleSystem.Stop();
            //myAudioSource.Stop();
        }
    }

    // enable camera rotation only with right click
    void UpdateCamera()
    {
        if (!myCinemachineCamera) return;

        bool cameraControlActive = cameraEnabled ? isCameraControlActive : false;

        // move camera
        if (cameraForced != null)
        {
            myCinemachineCamera.transform.rotation = (Quaternion)cameraForced;
        }
        else if (cameraControlActive)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 mouseDelta = mouseInitialPosition - mousePosition;

            float xAngle = NormalizeAngle(cameraInitialRotation.eulerAngles.x);
            float yAngle = NormalizeAngle(cameraInitialRotation.eulerAngles.y);
            float zAngle = NormalizeAngle(cameraInitialRotation.eulerAngles.z);
            float xRotation = Mathf.Clamp(xAngle + (mouseDelta.y * cameraSensitivityY), -70f, 70f);
            float yRotation = Mathf.Clamp(yAngle - (mouseDelta.x * cameraSensitivityX), -180f, 180f);
            float zRotation = zAngle;
            myCinemachineCamera.transform.rotation = Quaternion.Euler(
                xRotation,
                yRotation,
                zRotation
            );
        }

        // move the rocket with the camera (if enabled)
        if (controlYMovement)
        {
            Vector3 currentRotation = transform.eulerAngles;
            Vector3 cameraRotation = myCinemachineCamera.transform.eulerAngles;
            transform.rotation = Quaternion.Euler(
                currentRotation.x,      // keep current X
                cameraRotation.y,       // match Y to camera
                currentRotation.z       // keep current Z
            );
        }
    }

    // normalize angle from -180 to 180 instead from 0 to -360
    float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle > 180f) angle -= 360f;
        if (angle < -180f) angle += 360f;
        return angle;
    }
}
