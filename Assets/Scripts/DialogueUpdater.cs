using TMPro;
using UnityEngine;

public class DialogueUpdater : MonoBehaviour
{
    [SerializeField] LevelController levelController;
    [SerializeField] DialogueArrays dialogueArrays;
    
    [SerializeField] GameObject dialogueUI;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text speakerName;

    private int sceneIndex = 0;
    private int sceneLength = 0;
    private int slideIndex = 0;

    public void EnableDialogue()
    {
        slideIndex = 0;
        UpdateDialogue();
        dialogueUI.SetActive(true);
    }

    public void DisableDialogue()
    {
        dialogueUI.SetActive(false);
    }

    public void UpdateDialogue()
    {
        if (slideIndex < sceneLength)
        {
            speakerName.SetText(dialogueArrays.GetSpeakerName(sceneIndex, slideIndex));
            speakerName.color = dialogueArrays.GetSpeakerColor(sceneIndex, slideIndex);
            dialogueText.SetText(dialogueArrays.GetDialogueText(sceneIndex, slideIndex));
            dialogueText.color = dialogueArrays.GetSpeakerColor(sceneIndex, slideIndex);

            slideIndex++;
        }
        else
        {
            DisableDialogue();
            levelController.EndDialogueScene();
        }
    }
    
    public void SelectStoryScene(string _sceneName)
    {
        sceneIndex = dialogueArrays.FindStorySceneIndex(_sceneName);
        sceneLength = dialogueArrays.storyDialoguesList.dialogueSets[sceneIndex].dialogueSlides.Count;
        Debug.Log("Scene Length: " + sceneLength.ToString());
    }
}
