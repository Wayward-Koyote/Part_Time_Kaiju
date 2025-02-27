using UnityEngine;

public class SprintState : CharacterState
{
    /* State Vairables */
    bool sprint;
    bool trip;
    // bool jump;       //TODO

    public SprintState(Character _character, CharacterStateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        // jump = false;    //TODO
        sprint = false;
        trip = false;
        input = Vector2.zero;
        velocity = Vector3.zero;
        //gravityVelocity.y = 0f;

        character.currentSpeed = character.sprintSpeed;
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

        // Check for State Change Triggers
        if (character.trip)
        {
            trip = true;
        }
        else if (sprintAction.IsPressed() && input.sqrMagnitude != 0f)
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

        // If Trigger for switching States occured during HandleInput() then switch to new State
        if (trip)
        {
            stateMachine.ChangeState(character.tripped);
        }
        else if (sprint) //If still sprinting
        {
            // Update movement animation based off of a flat speed increase
            character.animator.SetFloat("speed", input.magnitude + 0.5f, character.speedDampTime, Time.deltaTime);
        }
        else
        {
            stateMachine.ChangeState(character.standing);
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
        //Cancle out velocities being applied to be handled by next State
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
