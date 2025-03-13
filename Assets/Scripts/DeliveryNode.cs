using UnityEngine;
using TMPro;

public class DeliveryNode : MonoBehaviour
{
    /* Managers */
    LevelController levelController;
    DeliveryManager deliveryManager;

    [Header("Timer Hookup")]
    [SerializeField] TMP_Text timerTxt;
    [SerializeField] GameObject head;

    /* Private Variables */
    // private float deliveryTime;
    private float timeLeft;
    private bool timerActive = false;
    // private float baseTip = 20f;
    private bool orderPlaced = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        deliveryManager = GameObject.Find("DeliveryManager").GetComponent<DeliveryManager>();
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
                // Debug.Log("Order Cancelled");
            }
        }
    }

    private void UpdateTimer(float currentTime)
    {
        //currentTime += 1;

        //float minutes = Mathf.FloorToInt(currentTime / 60);
        //float seconds = Mathf.FloorToInt(currentTime % 60);

        //timerTxt.text = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerTxt.text = string.Format("{0:00}s Left", currentTime);
    }

    //Deprecated
    public void OrderPlaced()
    {
        orderPlaced = true;
    }

    public void OrderPickedUp(float _timeLeft)
    {
        head.SetActive(true);

        timeLeft = _timeLeft;
        timerActive = true;
    }

    public void OrderDelivered()
    {
        orderPlaced = false;

        timeLeft = 0;
        timerActive = false;
        head.SetActive(false);

        Debug.Log("Order Delivered");
    }

    public void OrderExpired()
    {
        orderPlaced = false;

        timeLeft = 0;
        timerActive = false;
        head.SetActive(false);
    }

    public bool HasAlreadyOrdered()
    {
        return orderPlaced;
    }
}
