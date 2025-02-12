using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [Header("Controls")]
    public float playerSpeed = 5f;
    public float sprintSpeed = 7f;
    public float roatationSpeed = 5f;
    public float gravityMultiplier = 2f;

    [Header("Animation Smoothing")]
    [Range(0, 1)] public float speedDampTime = 0.1f;
    [Range(0, 1)] public float velocityLerp = 0.8f;
    [Range(0, 1)] public float rotationDampTime = 0.2f;
    [Range(0, 1)] public float airControl = 0.5f;

    /* State Machine Variables */
    
    public StateMachine movementSM;
    public StandingState standing;
    public SprintState sprinting;

    /* Public Variables */
    [HideInInspector] public float gravityValue = -9.81f;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public Transform cameraTransform;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 playerVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /* GetComponents Setup */
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        /* State Machine Setup */
        
        movementSM = new StateMachine();
        standing = new StandingState(this, movementSM);
        sprinting = new SprintState(this, movementSM);

        movementSM.Initialize(standing);

        gravityValue *= gravityMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        /* State Machine */
        movementSM.currentState.HandleInput();
        movementSM.currentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        movementSM.currentState.PhysicsUpdate();
    }
}
