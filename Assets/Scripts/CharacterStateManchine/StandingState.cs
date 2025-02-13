using UnityEngine;

public class StandingState : CharacterState
{
    /* State Vairables */
    bool sprint;
    bool careful;
    // bool jump;       //TODO

    public StandingState(Character _character, CharacterStateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        // jump = false;    //TODO
        sprint = false;
        careful = false;
        input = Vector2.zero;
        velocity = Vector3.zero;
        //gravityVelocity.y = 0f;

        character.currentSpeed = character.playerSpeed;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        // Get Input
        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        // Convert movement input to be relative to camera direction
        velocity = (velocity.x * character.cameraTransform.right.normalized) + (velocity.z * character.cameraTransform.forward.normalized);
        velocity.y = 0f;

        // Check If Sprint Key is pressed AND that there is movement Input
        if (sprintAction.IsPressed() && input.sqrMagnitude != 0f)
        {
            sprint = true;
        }
        else if (carefulAction.IsPressed())
        {
            careful = true;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Update movement animation
        character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);

        // If Input for switching States occured during HandleInput() then switch to new State
        if (sprint)
        {
            stateMachine.ChangeState(character.sprinting);
        }
        else if (careful)
        {
            stateMachine.ChangeState(character.careful);
        }

        character.MoveCharacter(velocity);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        
    }

    public override void Exit()
    {
        base.Exit();

        /*
        // Cancle out velocities being applied to be handled by next State
        gravityVelocity.y = 0f;
        character.playerVelocity = new Vector3(input.x, 0, input.y);

        // If the player is in process of turning then set them to face the direction they were turning towards
        if (velocity.sqrMagnitude > 0f)
        {
            character.transform.rotation = Quaternion.LookRotation(velocity);
        }
        */
    }
}
