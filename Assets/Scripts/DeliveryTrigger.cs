using UnityEngine;

public class DeliveryTrigger : MonoBehaviour
{
    private OrderInventory inventory;

    private void Start()
    {
        inventory = GameObject.Find("OrderInventory").GetComponent<OrderInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventory.OrderDelivered(this.GetComponent<DeliveryNode>());
        }
    }
}
