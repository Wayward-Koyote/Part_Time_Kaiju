using UnityEngine;

public class Push : MonoBehaviour
{
    [Header("Player Momentum Setup")]
    [SerializeField] float playerMass = 300f;
    private Character character;
    private float currentVelocity;

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
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb != null )
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y *= 0.5f;
            forceDirection.Normalize();

            currentVelocity = character.GetCurrentVelocity().magnitude;

            float pushForce = currentVelocity * playerMass;
            rb.AddForceAtPosition(forceDirection * pushForce, transform.position, ForceMode.Impulse);

            Debug.Log("Applied Force: " + pushForce.ToString());
        }
    }
}
