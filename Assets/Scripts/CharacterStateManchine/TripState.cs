using UnityEngine;

public class TripState : CharacterState
{
    /* State Vairables */
    bool trip;
    private float timeLeft;
    private bool timerActive = false;

    public TripState(Character _character, CharacterStateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        trip = true;

        timeLeft = Mathf.Max(character.tripDuration * 0.3f, character.tripDuration * (character.tripChance/100));
        timerActive = true;

        character.rb.isKinematic = false;
        character.rb.detectCollisions = true;

        character.tripChance = 0f;

        character.animator.SetBool("trip", true);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        //velocity = new Vector3(velocity.x * (timeLeft / character.tripDuration), 0, velocity.z * (timeLeft / character.tripDuration));
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!trip)
        {
            stateMachine.ChangeState(character.standing);
        }
        else
        {
            if (timerActive)
            {
                if (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime;
                    
                    //Debug.Log("Trip Time Left: " + timeLeft.ToString());
                }
                else
                {
                    timeLeft = 0;
                    timerActive = false;

                    trip = false;

                    //Debug.Log("TripEnd");
                }
            }
        }

        //character.MoveCharacter(velocity);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();

        //CharacterController weirdness...
        character.controller.enabled = false;
        //character.controller.Move(character.gameObject.transform.position);
        character.controller.enabled = true;

        character.SetCurrentVelocity(new Vector3(0, 0, 0));

        character.rb.isKinematic = true;
        character.rb.detectCollisions = false;

        character.trip = false;

        character.animator.SetBool("trip", false);
    }
}
