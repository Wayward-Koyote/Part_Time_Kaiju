using UnityEngine;
using TMPro;

public class DialogueSceneController : MonoBehaviour
{
    [SerializeField] DialogueUpdater _updater;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.State == GameState.DialogueScene)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                _updater.UpdateSceneDialogue();
            }
        }
    }
}
