using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections.Generic;

public class LevelController : MonoBehaviour
{
    [Header("Level Variables")]
    [SerializeField] float levelTime = 300f;
    [SerializeField] List<Overtime> overtimeList;

    [Header("UI Hookup")]
    [SerializeField] TMP_Text timerTxt;
    [SerializeField] TMP_Text damageTxt;
    [SerializeField] TMP_Text tipsTxt;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject shiftEndScreen;
    [SerializeField] TMP_Text damageTxtEnd;
    [SerializeField] TMP_Text tipsTxtEnd;

    [Header("Lazy Hookups")]
    [SerializeField] DialogueUpdater dialogueUpdater;
    [SerializeField] DialogueSceneController dialogueSceneController;
    [SerializeField] DialogueBarkController barkController;
    [SerializeField] string startSceneDialogue;
    [SerializeField] string endSceneDialogue;
    [SerializeField] Animator anim;

    /* Private Variables */
    private float totalDamage;
    private float totalTips;
    private float totalDeliveries;

    private float timeLeft;
    private bool timerActive = false;

    private int overtime = 0;
    private float payMult = 1f;

    private Character player;

    private GameState pauseReturn = GameState.PlayLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player").GetComponent<Character>();
        }

        totalDamage = 0;
        totalTips = 0;
        totalDeliveries = 0;

        timeLeft = levelTime;

        overtime = 0;
        payMult = 1.0f;

        StartDialogueScene(startSceneDialogue);
        //StartShift(); *depricated*
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
                OvertimeCheck();
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

    private void OvertimeCheck()
    {
        if (overtime < overtimeList.Count)
        {
            if (totalDeliveries >= overtimeList[overtime].OvertimePar())
            {
                timeLeft += overtimeList[overtime].TimeExtention();
                payMult = overtimeList[overtime].PayMult();
                overtime++;

                Debug.Log("Overtime " + overtime.ToString());
            }
            else
            {
                timeLeft = 0;
                timerActive = false;

                Debug.Log("End of Shift");
                StartDialogueScene(endSceneDialogue);
            }
        }
        else
        {
            timeLeft = 0;
            timerActive = false;

            Debug.Log("End of Shift");
            StartDialogueScene(endSceneDialogue);
        }
    }

    public void UpdateDamage(float damage)
    {
        totalDamage += damage;
        //Debug.Log("Total Damage: $" + totalDamage);
        damageTxt.text = string.Format("{0:C}", totalDamage);
    }

    public void UpdateTips(float tip)
    {
        totalTips += tip * payMult;
        tipsTxt.text = string.Format("{0:C}",  totalTips);

        totalDeliveries++;
    }

    private void StartShift()
    {
        timerActive = true;

        AudioManager.Instance.StopMenuBGM();
        AudioManager.Instance.StartLevelBGM();

        GameManager.Instance.UpdateGameState(GameState.PlayLevel);
    }

    private void EndShift()
    {
        GameManager.Instance.UpdateGameState(GameState.EndShift);

        damageTxtEnd.text = damageTxt.text;
        tipsTxtEnd.text = tipsTxt.text;

        shiftEndScreen.SetActive(true);

        player.enabled = false;
    }

    public void PauseGame()
    {
        pauseReturn = GameManager.Instance.State;

        GameManager.Instance.UpdateGameState(GameState.Pause);
        pauseMenu.gameObject.SetActive(true);

        player.enabled = false;

        AudioManager.Instance.PauseLevelBGM();
    }

    public void ResumeGame()
    {
        GameManager.Instance.UpdateGameState(pauseReturn);
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

    public void StartDialogueScene(string _scene)
    {
        GameManager.Instance.UpdateGameState(GameState.DialogueScene);

        dialogueUpdater.SelectStoryScene(_scene);
        dialogueUpdater.DisableDialogue();
        dialogueUpdater.EnableSceneDialogue();

        AudioManager.Instance.StopLevelBGM();
        AudioManager.Instance.StartMenuBGM();
    }

    public void EndDialogueScene()
    {
        if(timeLeft > 0)
        {
            StartShift();
        }
        else
        {
            EndShift();
        }
    }

    /* Overtime Serializable Class */
    [System.Serializable]
    public class Overtime
    {
        [SerializeField] int overtimePar = 20;
        [SerializeField] float timeExtension = 30f;
        [SerializeField] float payMult = 1.5f;

        public int OvertimePar()
        {
            return overtimePar;
        }
        public float TimeExtention()
        {
            return timeExtension;
        }
        public float PayMult()
        {
            return payMult;
        }
    }
}
