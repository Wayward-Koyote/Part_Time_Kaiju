using UnityEngine;
using UnityEngine.InputSystem;

public class State
{
    /* Public Variables */
    public Character character;
    public StateMachine stateMachine;

    /* Protected Variables */
    protected Vector3 gravityVelocity;
    protected Vector3 velocity;
    protected Vector2 input;

    /* Input Actions */
    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction sprintAction;

    /* Constructor - Allows for states to be created within a MonoBehaviour class */
    public State(Character _character, StateMachine _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;

        moveAction = character.playerInput.actions["Move"];
        lookAction = character.playerInput.actions["Look"];
        sprintAction = character.playerInput.actions["Sprint"];
    }

    /* Call to Enter a State - Handles Setup for new State */
    public virtual void Enter()
    {
        Debug.Log("Player enter state: " + this.ToString());
    }

    /* Call during Update() - Gates available Inputs and what States each Input connects to */
    public virtual void HandleInput()
    {
    }

    /* Call during Update() - Handles how Update() functions in this State */
    public virtual void LogicUpdate()
    {
    }

    /* Call during FixedUpdate() - Handles physics calculations for this State */
    public virtual void PhysicsUpdate()
    {
    }

    /* Call when Exiting a State - Handles cleanup for old State */
    public virtual void Exit()
    {
    }
}
