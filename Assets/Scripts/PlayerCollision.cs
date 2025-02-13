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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
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

                        hit.gameObject.GetComponent<Building>().HandleImpact(pushForce, levelController);
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
                default:
                    //Debug.Log("Player Collision Unrecognized");

                    break;
            }
        }
    }
}
