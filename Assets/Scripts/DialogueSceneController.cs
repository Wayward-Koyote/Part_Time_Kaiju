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
            if (Input.anyKeyDown && !(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
            {
                _updater.UpdateDialogue();
            }
        }
    }
}
