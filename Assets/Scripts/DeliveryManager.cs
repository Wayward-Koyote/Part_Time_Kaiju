using NUnit.Framework;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [Header("Level Hookup")]
    [SerializeField] OrderInventory orderInventory;

    [Header("Order Variables")]
    [SerializeField] int maxOrders = 5;
    [SerializeField] float firstOrderDelay = 2f;
    [SerializeField] float timeBetweenOrdersMin = 10f;
    [SerializeField] float timeBetweenOrdersMax = 15f;
    [SerializeField] float timeToCompleteOrders = 30f;
    [SerializeField] float baseTips = 20f;

    [Header("Delivery Nodes")]
    [SerializeField] GameObject[] nodes;
    private DeliveryNode[] deliveryNodes;

    /* Private Variables */
    private float timeLeft;
    private bool timerActive = false;
    private int activeOrders;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deliveryNodes = new DeliveryNode[nodes.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            deliveryNodes[i] = nodes[i].GetComponentInChildren<DeliveryNode>(true);
        }

        activeOrders = 0;

        timeLeft = firstOrderDelay;
        timerActive = true;

        Debug.Log("Time Until First Order: " + timeLeft.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                // Debug.Log("Time Until Next Order: " +  timeLeft.ToString());
            }
            else
            {
                if (activeOrders < maxOrders)
                {
                    Debug.Log("Placing Order");
                    PlaceOrder();
                }

                timeLeft = Random.Range(timeBetweenOrdersMin, timeBetweenOrdersMax);
                Debug.Log("Time Until Next Order: " + timeLeft.ToString());
            }
        }
    }

    private void PlaceOrder()
    {
        int counter = 0;
        int index;
        while (counter < maxOrders)
        {
            // Debug.Log("Attempt to Order");
            index = Random.Range(0, deliveryNodes.Length - 1);

            if (!deliveryNodes[index].HasAlreadyOrdered())
            {
                // Debug.Log("Order Up");

                orderInventory.OrderPlaced(timeToCompleteOrders, baseTips, deliveryNodes[index], true);                

                activeOrders++;
                counter = maxOrders;
            }
            else
            {
                counter++;
            }
        }
    }

    public void RemoveOrder()
    {
        activeOrders--;
    }
}
