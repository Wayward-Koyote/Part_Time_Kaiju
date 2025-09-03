using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [Header("Level Variables")]
    [SerializeField] float levelTime = 300f;
    [SerializeField] List<Overtime> overtimeList;
    [SerializeField] float parDamagePenaltyMod = 0.0001f;

    [Header("UI Hookup")]
    [SerializeField] TMP_Text timerTxt;
    [SerializeField] Image clockRadialBar;
    [SerializeField] Color timerStart;
    [SerializeField] Color timerEnd;
    [SerializeField] Image overtimeMeter;
    [SerializeField] Image overtimeMeterBG;
    //Deprecated   [SerializeField] TMP_Text damageTxt;
    //Deprecated   [SerializeField] TMP_Text tipsTxt;
    //Deprecated   [SerializeField] TMP_Text parTxt;
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
    [SerializeField] GameObject ShiftEndTempFix; //temp fix for animation bug

    /* Private Variables */
    private float totalDamage;
    private float totalTips;
    private float totalDeliveries;  //Unused, keeping for eventual stats

    private float timeLeft;
    private bool timerActive = false;
    private float timerBarOffset;

    private float parScore = 0f;
    private int bankedOvertime = 0;
    private float currentPar = 0;
    private int currentOvertime = 0;
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
        timerBarOffset = 1 / levelTime;

        bankedOvertime = 0;
        currentOvertime = 0;
        payMult = 1.0f;

        parScore = 0f;
        currentPar = overtimeList[currentOvertime].OvertimePar();
        overtimeMeter.fillAmount = 0f;
        overtimeMeter.color = overtimeList[currentOvertime].OvertimeBarColor();

        //UpdatePar(); *deprecated*

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

        float timerPercent = currentTime * timerBarOffset;
        // Debug.Log("Clock Percent: " + timerPercent.ToString());
        clockRadialBar.fillAmount = timerPercent;
        clockRadialBar.color = Color.Lerp(timerEnd, timerStart, timerPercent);
    }

    private void OvertimeCheck()
    {
        if (currentOvertime < overtimeList.Count)
        {
            if (bankedOvertime > currentOvertime)
            {
                timeLeft += overtimeList[currentOvertime].TimeExtention();
                timerBarOffset = 1 / timeLeft;

                payMult = overtimeList[currentOvertime].PayMult();
                currentOvertime++;

                // currentPar += overtimeList[currentOvertime].OvertimePar(); *deprecated*

                // UpdatePar(); *deprecated*

                anim.Play("Overtime" + currentOvertime.ToString());

                Debug.Log("Overtime " + currentOvertime.ToString());
            }
            else
            {
                timeLeft = 0;
                timerActive = false;

                // anim.Play("ShiftEnd");       Animation bug with Time.DeltaTime
                ShiftEndTempFix.SetActive(true); //temp fix for animation bug, need to setup token Times

                Debug.Log("End of Shift");
                StartDialogueScene(endSceneDialogue);
            }
        }
        else
        {
            timeLeft = 0;
            timerActive = false;

            // anim.Play("ShiftEnd");       Aniomation bug with Time.DeltaTime
            ShiftEndTempFix.SetActive(true); //temp fix for animation bug

            Debug.Log("End of Shift");
            StartDialogueScene(endSceneDialogue);
        }
    }

    public void UpdateDamage(float damage)
    {
        totalDamage += damage;
        //Debug.Log("Total Damage: $" + totalDamage);
        //Deprecated   damageTxt.text = string.Format("{0:C}", totalDamage);

        UpdateOvertimeMeter(damage * -parDamagePenaltyMod);
    }

    public void UpdateTips(float tip)
    {
        totalTips += tip * payMult;
        // Deprecated   tipsTxt.text = string.Format("{0:C}",  totalTips);

        totalDeliveries++;

        UpdateOvertimeMeter(tip);
    }

    private void UpdateOvertimeMeter(float update)
    {
        parScore += update;
        // Debug.Log("Par Score: " + parScore.ToString());

        if (bankedOvertime < overtimeList.Count)
        {
            if (parScore < currentPar)
            {
                overtimeMeter.fillAmount = Mathf.Clamp((parScore / currentPar), 0, 1);
                // Debug.Log("Overtime Meter: " +  overtimeMeter.fillAmount.ToString());
            }
            else
            {
                parScore -= currentPar;

                bankedOvertime++;
                currentPar = overtimeList[bankedOvertime].OvertimePar();

                overtimeMeterBG.color = overtimeMeter.color;
                overtimeMeter.color = overtimeList[bankedOvertime].OvertimeBarColor();

                overtimeMeter.fillAmount = Mathf.Clamp((parScore / currentPar), 0, 1);
            }
        }

        //Deprecated   parTxt.text = (totalDeliveries.ToString() + "/" + currentPar.ToString());
    }

    private void StartShift()
    {
        timerActive = true;

        AudioManager.Instance.StopMenuBGM();
        AudioManager.Instance.StartLevelBGM();

        anim.Play("ShiftStart");

        GameManager.Instance.UpdateGameState(GameState.PlayLevel);
    }

    private void EndShift()
    {
        GameManager.Instance.UpdateGameState(GameState.EndShift);

        damageTxtEnd.text = string.Format("{0:C}", totalDamage);
        tipsTxtEnd.text = string.Format("{0:C}", totalTips);

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
        [SerializeField] Color overtimeBarColor = Color.blue;

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
        public Color OvertimeBarColor()
        {
            return overtimeBarColor;
        }
    }
}
