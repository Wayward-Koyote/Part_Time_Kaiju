using UnityEngine;

public class SprintState : State
{
    /* State Vairables */
    float gravityValue;
    Vector3 currentVelocity;
    bool sprint;
    float playerSpeed;
    bool grounded;
    // bool jump;       //TODO

    Vector3 cVelocity;

    public SprintState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        // jump = false;    //TODO
        sprint = true;
        input = Vector2.zero;
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0f;

        playerSpeed = character.sprintSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        //Get Input
        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        // Convert movement input to be relative to camera direction
        velocity = (velocity.x * character.cameraTransform.right.normalized) + (velocity.z * character.cameraTransform.forward.normalized);
        velocity.y = 0f;

        //Check If Sprint Key is Released OR If there is no movement Input
        if (sprintAction.IsPressed() && input.sqrMagnitude != 0f)
        {
            sprint = true;
        }
        else
        {
            sprint = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // If still sprinting then apply Sprint Logic, Else change back to Standing State
        if (sprint)
        {
            // Update movement animation based off of a flat speed increase
            character.animator.SetFloat("speed", input.magnitude + 0.5f, character.speedDampTime, Time.deltaTime);
        }
        else
        {
            stateMachine.ChangeState(character.standing);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // Apply Gravity to the Player
        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        // If on the ground then don't send the Player through it
        if (grounded && gravityVelocity.y < 0f)
        {
            gravityVelocity.y = 0f;
        }

        // Apply velocity to the player in the direction determined by Input relative to the camera
        currentVelocity = Vector3.MoveTowards(currentVelocity, velocity, character.velocityLerp * Time.deltaTime);
        //Vector3 finalVelocity = character.transform.TransformVector(currentVelocity);
        character.controller.Move((currentVelocity * Time.deltaTime * playerSpeed) + (gravityVelocity * Time.deltaTime));

        //If the player is not facing the direction they are moving, apply roation to them.
        if (velocity.sqrMagnitude > 0f)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
        }
    }

    public override void Exit()
    {
        base.Exit();


        //Cancle out velocities being applied to be handled by next State
        gravityVelocity.y = 0f;
        //character.playerVelocity = new Vector3(input.x, 0, input.y);

        // If the player is in process of turning then set them to face the direction they were turning towards
        if (velocity.sqrMagnitude > 0f)
        {
            character.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}
