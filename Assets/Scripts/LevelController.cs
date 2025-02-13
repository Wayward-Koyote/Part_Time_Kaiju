using UnityEngine;
using TMPro;

public class LevelController : MonoBehaviour
{
    [Header("Level Variables")]
    [SerializeField] float levelTime = 300f;

    [Header("UI Hookup")]
    [SerializeField] TMP_Text timerTxt;
    [SerializeField] TMP_Text damageTxt;
    [SerializeField] TMP_Text tipsTxt;

    /* Private Variables */
    private float totalDamage;
    private float totalTips;

    private float timeLeft;
    private bool timerActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalDamage = 0;
        totalTips = 0;

        timeLeft = levelTime;
        timerActive = true;
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
            }
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
}
