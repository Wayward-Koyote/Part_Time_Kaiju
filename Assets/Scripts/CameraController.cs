using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Hookup")]
    [SerializeField] Character player;
    [SerializeField] float followSpeed;
    [SerializeField] float spinSpeed;

    /* Private variables */
    Vector3 offset;
    Vector2 input;

    InputAction lookAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = this.transform.position + player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        this.transform.position = player.transform.position + offset;
    }
}
