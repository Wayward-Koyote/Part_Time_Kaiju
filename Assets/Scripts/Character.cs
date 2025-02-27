using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [Header("Movement")]
    public float playerSpeed = 50f;
    public float sprintSpeed = 75f;
    public float carefulSpeed = 25f;
    public float roatationSpeed = 5f;
    public float gravityMultiplier = 2f;
    public float velocityLerp = 0.8f;
    public float stopMod = 2f;

    [Header("Trip")]
    public float tripDuration = 2.5f;
    public float tripDecayRate = 5f;
    public float tripAccrueRate = 1f;
    [Range(0, 100)] public float tripStartingBuffer = 0f;


    [Header("Animation Smoothing")]
    [Range(0, 1)] public float speedDampTime = 0.1f;
    [Range(0, 1)] public float rotationDampTime = 0.2f;
    //[Range(0, 1)] public float airControl = 0.5f;

    /* State Machine Variables */
    
    public CharacterStateMachine movementSM;
    public StandingState standing;
    public SprintState sprinting;
    public CarefulState careful;
    public TripState tripped;

    /* Public Variables */
    [HideInInspector] public float gravityValue = -9.81f;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public Transform cameraTransform;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 playerVelocity;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float tripChance;
    [HideInInspector] public bool trip;

    /* Private Variables */
    Vector3 currentVelocity;
    bool grounded;
    Vector3 gravityVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /* GetComponents Setup */
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        /* Component Start States */
        rb.detectCollisions = false;

        /* State Machine Setup */
        movementSM = new CharacterStateMachine();
        standing = new StandingState(this, movementSM);
        sprinting = new SprintState(this, movementSM);
        careful = new CarefulState(this, movementSM);
        tripped = new TripState(this, movementSM);

        movementSM.Initialize(standing);

        /* Initial Variable Values */
        gravityValue *= gravityMultiplier;
        gravityVelocity.y = 0f;
        currentVelocity = Vector3.zero;
        currentSpeed = playerSpeed;
        grounded = controller.isGrounded;
        tripChance = 0f - tripStartingBuffer;
        trip = false;
    }

    // Update is called once per frame
    void Update()
    {
        /* State Machine */
        movementSM.currentState.HandleInput();
        movementSM.currentState.LogicUpdate();

        TripDecay();
    }

    void FixedUpdate()
    {
        movementSM.currentState.PhysicsUpdate();
    }

    /* Private Functions/Methods */

    private void TripDecay()
    {
        if (tripChance > -100)
        {
            tripChance -= tripDecayRate * Time.deltaTime;
        }
        else
        {
            tripChance = -100f;
        }
    }

    /* Public Functions */ 

    public void MoveCharacter(Vector3 _velocity)
    {
        // Apply Gravity to the Player
        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = controller.isGrounded;

        // If on the ground then don't send the Player through it
        if (grounded && gravityVelocity.y < 0f)
        {
            gravityVelocity.y = 0f;
        }

        // Apply velocity to the player in the direction determined by Input relative to the camera
        if (_velocity.magnitude > 0f)
            currentVelocity = Vector3.MoveTowards(currentVelocity, _velocity, velocityLerp * Time.deltaTime);
        else
            currentVelocity = Vector3.MoveTowards(currentVelocity, _velocity, velocityLerp * stopMod * Time.deltaTime);
        //Vector3 finalVelocity = character.transform.TransformVector(currentVelocity);
        controller.Move((currentSpeed * Time.deltaTime * currentVelocity) + (gravityVelocity * Time.deltaTime));

        //If the player is not facing the direction they are moving, apply roation to them.
        if (_velocity.sqrMagnitude > 0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_velocity), rotationDampTime);
        }
    }

    public void TripChance(float _tripAmount)
    {
        if (!trip && (_tripAmount > 0))
        {
            tripChance += _tripAmount;

            if ((tripChance + Random.Range(0, 100)) >= 100)
            {
                trip = true;
            }

            Debug.Log("TripChance: " + tripChance.ToString());
        }
    }

    /* Getters & Setters */

    public Vector3 GetCurrentVelocity()
    {
        return currentVelocity;
    }

    public void SetCurrentVelocity(Vector3 _velocity)
    {
        currentVelocity = _velocity;
    }
}
