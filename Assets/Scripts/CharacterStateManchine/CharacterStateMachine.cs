
public class CharacterStateMachine
{
    public CharacterState currentState;

    public void Initialize(CharacterState startingState)
    {
        currentState = startingState;
        startingState.Enter();
    }

    public void ChangeState(CharacterState newState)
    {
        currentState.Exit();

        currentState = newState;
        newState.Enter();
    }
    
}
