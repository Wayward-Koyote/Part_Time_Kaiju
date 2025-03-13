using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [Header("Level Variables")]
    [SerializeField] float levelTime = 300f;

    [Header("UI Hookup")]
    [SerializeField] TMP_Text timerTxt;
    [SerializeField] TMP_Text damageTxt;
    [SerializeField] TMP_Text tipsTxt;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject shiftEndScreen;
    [SerializeField] TMP_Text damageTxtEnd;
    [SerializeField] TMP_Text tipsTxtEnd;

    /* Private Variables */
    private float totalDamage;
    private float totalTips;

    private float timeLeft;
    private bool timerActive = false;

    private Character player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player").GetComponent<Character>();
        }

        totalDamage = 0;
        totalTips = 0;

        timeLeft = levelTime;
        timerActive = true;

        GameManager.Instance.UpdateGameState(GameState.PlayLevel);
        AudioManager.Instance.StopMenuBGM();
        AudioManager.Instance.StartLevelBGM();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                timeLeft = 0;
                timerActive = false;

                Debug.Log("End of Shift");
                EndShift();
            }
        }
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pause Game");
            PauseGame();
        }
    }

    private void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerTxt.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void UpdateDamage(float damage)
    {
        totalDamage += damage;
        //Debug.Log("Total Damage: $" + totalDamage);
        damageTxt.text = string.Format("{0:C}", totalDamage);
    }

    public void UpdateTips(float tip)
    {
        totalTips += tip;
        tipsTxt.text = string.Format("{0:C}",  totalTips);
    }

    private void EndShift()
    {
        GameManager.Instance.UpdateGameState(GameState.EndShift);

        damageTxtEnd.text = damageTxt.text;
        tipsTxtEnd.text = tipsTxt.text;

        shiftEndScreen.SetActive(true);

        player.enabled = false;

        AudioManager.Instance.StopLevelBGM();
        AudioManager.Instance.StartMenuBGM();
    }

    public void PauseGame()
    {
        GameManager.Instance.UpdateGameState(GameState.Pause);
        pauseMenu.gameObject.SetActive(true);

        player.enabled = false;

        AudioManager.Instance.PauseLevelBGM();
    }

    public void ResumeGame()
    {
        GameManager.Instance.UpdateGameState(GameState.PlayLevel);
        pauseMenu.gameObject.SetActive(false);

        player.enabled = true;

        AudioManager.Instance.ResumeLevelBGM();
    }

    public void QuitToMainMenu()
    {
        Debug.Log("Quit To MainMenu");

        AudioManager.Instance.StopLevelBGM();

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
