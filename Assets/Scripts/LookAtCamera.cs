using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCam;

    private void OnEnable()
    {
        mainCam = Camera.main.transform;

        // Debug.Log("Main Cam: " + mainCam.name);
    }

    private void LateUpdate()
    {
        transform.LookAt(mainCam);
        transform.RotateAround(transform.position, transform.up, 180f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
