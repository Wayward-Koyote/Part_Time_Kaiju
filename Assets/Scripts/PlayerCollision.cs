using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Building":
                Debug.Log("Player Collision with Building");

                currentVelocity = character.GetCurrentVelocity().magnitude;
                pushForce = currentVelocity * playerMass;

                hit.gameObject.GetComponent<Building>().HandleImpact(pushForce);

                break;
            case "BuildingPart":
                Debug.Log("Player Collision with BuildingPart");

                Rigidbody rb = hit.collider.attachedRigidbody;

                if (rb != null)
                {
                    Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
                    forceDirection.y *= 0.5f;
                    forceDirection.Normalize();

                    currentVelocity = character.GetCurrentVelocity().magnitude;
                    pushForce = currentVelocity * playerMass;

                    rb.AddForceAtPosition(forceDirection * pushForce, transform.position, ForceMode.Impulse);

                    //Debug.Log("Applied Force: " + pushForce.ToString());
                }

                break;
            default:
                Debug.Log("Player Collision Unrecognized");

                break;
        }
    }
}
