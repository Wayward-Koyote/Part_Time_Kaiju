using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;

public class CarefulState : CharacterState
{
    /* State Vairables */
    bool sprint;
    bool careful;
    // bool jump;       //TODO

    public CarefulState(Character _character, CharacterStateMachine _stateMachine) : base(_character, _stateMachine)
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

        character.currentSpeed = character.carefulSpeed;
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

        if (sprintAction.IsPressed() && input.sqrMagnitude != 0f)
        {
            sprint = true;
        }
        else if (carefulAction.IsPressed())
        {
            careful = true;
        }
        else
        {
            careful = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (sprint)
        {
            stateMachine.ChangeState(character.sprinting);
        }
        else if (careful)
        {
            // Update movement animation based off of a flat speed decrease
            character.animator.SetFloat("speed", input.magnitude - 0.5f, character.speedDampTime, Time.deltaTime);
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

        character.trip = false; //Cannot trip in Careful State so remove flag

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
