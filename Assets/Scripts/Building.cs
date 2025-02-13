using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Prefab Variables")]
    [SerializeField] float breakForceThreshold = 1f;
    [SerializeField] float value = 1000f;

    [Header("Broken Parts")]
    [SerializeField] GameObject[] parts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleImpact(float impactForce, LevelController levelController)
    {
        if (impactForce >= breakForceThreshold)
        {
            levelController.UpdateDamage(value);

            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].SetActive(true);
            }

            this.gameObject.SetActive(false);
        }
    }
}
