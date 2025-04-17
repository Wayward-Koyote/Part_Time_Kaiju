using UnityEngine;

public class DialogueUpdater : MonoBehaviour
{
    [SerializeField] GameObject tempPanel;

    public void EnableDialogue()
    {
        tempPanel.SetActive(true);
    }

    public void DisableDialogue()
    {
        tempPanel.SetActive(false);
    }
}
