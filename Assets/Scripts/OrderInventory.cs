using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderInventory : MonoBehaviour
{
    /* Managers */
    LevelController levelController;
    DeliveryManager deliveryManager;

    [Header("Timer Hookup")]
    [SerializeField] Slider[] timerBars;

    [Header("Order PickUp Zone")]
    [SerializeField] GameObject pickUpZone;

    /* Private Variables */

    /* Inventory */
    private Order[] orders;
    private int orderInventorySize = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        deliveryManager = GameObject.Find("DeliveryManager").GetComponent<DeliveryManager>();

        /* Assign timer sliders to each Inventory Slot */
        orders = new Order[orderInventorySize];
        for (int i = 0; i < orders.Length; i++)
        {
            orders[i] = new Order(0, 0, timerBars[i], timerBars[i].GetComponentInChildren<TMP_Text>(true), null, 20, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < orders.Length; i++)
        {
            if (orders[i].active)
            {
                if (orders[i].timeLeft > 0)
                {
                    orders[i].timeLeft -= Time.deltaTime;
                    orders[i].UpdateTimer(orders[i].timeLeft);
                }
                else
                {
                    //Update deliveryManager
                    deliveryManager.RemoveOrder();

                    orders[i].OrderExpired();

                    Debug.Log("Order Cancelled");
                }
            }
        }
    }

    public void OrderPlaced(float _deliveryTime, float _baseTip, DeliveryNode _deliveryLocation, bool _pickedUp)
    {
        for (int i = 0; i < orders.Length; i++)
        {
            if (!orders[i].active)
            {
                orders[i].active = true;
                orders[i].deliveryTime = _deliveryTime;
                orders[i].timeLeft = _deliveryTime;
                orders[i].deliveryLocation = _deliveryLocation;
                orders[i].pickedUp = _pickedUp;

                orders[i].deliveryLocation.OrderPlaced();

                /* Timer Bar Setup */
                orders[i].timerBar.gameObject.SetActive(true);
                orders[i].timerBar.maxValue = orders[i].deliveryTime;
                orders[i].timerBar.value = orders[i].timeLeft;

                if (!orders[i].pickedUp)
                    pickUpZone.SetActive(true);
                else
                    orders[i].deliveryLocation.OrderPickedUp(orders[i].timeLeft);

                break;
            }
        }
    }

    public void PickUpOrders()
    {
        for(int i = 0; i < orders.Length; i++)
        {
            if (orders[i].active)
            {
                orders[i].pickedUp = true;
                orders[i].UpdateTimer(orders[i].timeLeft);
            }
        }
    }

    public void OrderDelivered(DeliveryNode _deliveryLocation)
    {
        for (int i = 0; i < orders.Length; i++)
        {
            if (orders[i].deliveryLocation == _deliveryLocation)
            {
                float earnedTip = (orders[i].timeLeft / orders[i].deliveryTime) * orders[i].baseTip;
                levelController.UpdateTips(earnedTip);

                orders[i].deliveryLocation.OrderDelivered();

                deliveryManager.RemoveOrder();

                orders[i].timerBar.gameObject.SetActive(false);

                orders[i].active = false;

                break;
            }
        }

        Debug.Log("Order Delivered");
    }

    /* Order Struct */

    struct Order
    {
        public bool active;

        public float deliveryTime;
        public float timeLeft;

        public Slider timerBar;
        public TMP_Text timerText;

        public DeliveryNode deliveryLocation;

        public float baseTip;

        public bool pickedUp;

        public Order(float _deliveryTime, float _timeLeft, Slider _timerBar, TMP_Text _timerText, DeliveryNode _deliveryLocation, float _baseTip, bool _pickedUp)
        {
            active = false;

            deliveryTime = _deliveryTime;
            timeLeft = _deliveryTime;

            timerBar = _timerBar;
            timerText = _timerText;

            deliveryLocation = _deliveryLocation;

            baseTip = _baseTip;

            pickedUp = _pickedUp;
        }

        public void UpdateTimer(float currentTime)
        {
            // currentTime += 1;

            timerBar.value = currentTime;

            //float minutes = Mathf.FloorToInt(currentTime / 60);
            //float seconds = Mathf.FloorToInt(currentTime % 60);

            //timerTxt.text = string.Format("{0:0}:{1:00}", minutes, seconds);

            if (pickedUp)
                timerText.text = "Deliver: " + string.Format("{0:00}s Left", currentTime);
            else
                timerText.text = "Pick Up: " + string.Format("{0:00}s Left", currentTime);
        }

        public void OrderExpired()
        {
            deliveryLocation.OrderExpired();

            timeLeft = 0;

            timerBar.gameObject.SetActive(false);

            active = false;
        }
    }
}
