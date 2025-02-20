using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameManager gameManager;

    public void Start()
    {
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
    }

    public void PlayDemoLevel()
    {
        Debug.Log("Load Prototype Level");
        SceneManager.LoadScene("PrototypeLevel");
    }

    public void HowToPlay()
    {
        Debug.Log("Load How To Play");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
