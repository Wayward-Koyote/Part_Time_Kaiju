using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Level Hookup")]
    [SerializeField] LevelController levelController;

    [Header("Player Momentum Setup")]
    [SerializeField] float playerMass = 300f;
    private Character character;
    private float currentVelocity;
    private float pushForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with Player Occured");
        if (collision.gameObject.tag != null)
        {
            switch (collision.gameObject.tag)
            {
                case "Building":
                    //Debug.Log("Player Collision with Building");

                    if (collision.gameObject.GetComponent<Building>() != null)
                    {
                        currentVelocity = character.GetCurrentVelocity().magnitude;
                        pushForce = currentVelocity * playerMass;

                        collision.gameObject.GetComponent<Building>().HandleImpact(pushForce, levelController);
                    }

                    break;

                case "BuildingPart":
                    //Debug.Log("Player Collision with BuildingPart");

                    if (collision.collider.attachedRigidbody != null)
                    {
                        Rigidbody rb = collision.collider.attachedRigidbody;

                        Vector3 forceDirection = collision.gameObject.transform.position - transform.position;
                        forceDirection.y *= 0.5f;
                        forceDirection.Normalize();

                        currentVelocity = character.GetCurrentVelocity().magnitude;
                        pushForce = currentVelocity * playerMass;

                        rb.AddForceAtPosition(forceDirection * pushForce, transform.position, ForceMode.Impulse);

                        //Debug.Log("Applied Force: " + pushForce.ToString());
                    }

                    break;

                case "DeliveryZone":
                    //Debug.Log("Player Collision with DeliveryZone");
                    if (collision.gameObject.GetComponent<DeliveryNode>() != null)
                    {
                        collision.gameObject.GetComponent<DeliveryNode>().OrderDelivered();
                    }

                    break;

                default:
                    //Debug.Log("Player Collision Unrecognized");

                    break;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("Collision with Player Occured");

        if (hit.gameObject.tag != null)
        {
            switch (hit.gameObject.tag)
            {
                case "Building":
                    //Debug.Log("Player Collision with Building");

                    if (hit.gameObject.GetComponent<Building>() != null)
                    {
                        currentVelocity = character.GetCurrentVelocity().magnitude;
                        pushForce = currentVelocity * playerMass;

                        character.TripChance(hit.gameObject.GetComponent<Building>().HandleImpact(pushForce, levelController));
                    }

                    break;

                case "BuildingPart":
                    //Debug.Log("Player Collision with BuildingPart");

                    if (hit.collider.attachedRigidbody != null)
                    {
                        Rigidbody rb = hit.collider.attachedRigidbody;

                        Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
                        forceDirection.y *= 0.5f;
                        forceDirection.Normalize();

                        currentVelocity = character.GetCurrentVelocity().magnitude;
                        pushForce = currentVelocity * playerMass;

                        rb.AddForceAtPosition(forceDirection * pushForce, transform.position, ForceMode.Impulse);

                        //Debug.Log("Applied Force: " + pushForce.ToString());
                    }

                    break;

                case "DeliveryZone":
                    //Debug.Log("Player Collision with DeliveryZone");
                    if (hit.gameObject.GetComponent<DeliveryNode>() != null)
                    {
                        hit.gameObject.GetComponent<DeliveryNode>().OrderDelivered();
                    }

                    break;

                default:
                    //Debug.Log("Player Collision Unrecognized");

                    break;
            }
        }
    }
}
