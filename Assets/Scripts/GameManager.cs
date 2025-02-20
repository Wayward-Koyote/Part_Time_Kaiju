using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance {  get { return _instance; } }

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    /* References */

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch(newState)
        {
            case GameState.MainMenu:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Time.timeScale = 1; //Temp pause soltion for now (setup secondary time scale later)

                break;
            case GameState.PlayLevel:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                Time.timeScale = 1; //Temp pause soltion for now (setup secondary time scale later)

                break;
            case GameState.Pause:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Time.timeScale = 0; //Temp pause soltion for now (setup secondary time scale later)

                break;
            case GameState.EndShift:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Time.timeScale = 0; //Temp pause soltion for now (setup secondary time scale later)

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }
}

public enum GameState
{
    MainMenu,
    PlayLevel,
    Pause,
    EndShift
}
