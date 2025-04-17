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
        dialogueUI.SetActive(true);
    }

    public void DisableDialogue()
    {
        dialogueUI.SetActive(false);
    }

    public void UpdateSceneDialogue()
    {
        if (slideIndex < sceneLength)
        {
            speakerName.SetText(dialogueArrays.storyDialoguesList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].SpeakerName());
            speakerName.color = dialogueArrays.storyDialoguesList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].SpeakerColor();
            dialogueText.SetText(dialogueArrays.storyDialoguesList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].DialogueText());
            dialogueText.color = dialogueArrays.storyDialoguesList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].SpeakerColor();

            slideIndex++;
        }
        else
        {
            DisableDialogue();
            levelController.EndDialogueScene();
        }
    }

    public void UpdateBarkDialogue()
    {
        if (slideIndex < sceneLength)
        {
            speakerName.SetText(dialogueArrays.orderBarksList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].SpeakerName());
            speakerName.color = dialogueArrays.orderBarksList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].SpeakerColor();
            dialogueText.SetText(dialogueArrays.orderBarksList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].DialogueText());
            dialogueText.color = dialogueArrays.orderBarksList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].SpeakerColor();

            // slideIndex++;
        }
        else
        {
            DisableDialogue();
        }
    }

    public void SelectStoryScene(string _sceneName)
    {
        sceneIndex = dialogueArrays.FindStorySceneIndex(_sceneName);
        sceneLength = dialogueArrays.storyDialoguesList.dialogueSets[sceneIndex].dialogueSlides.Count;

        UpdateSceneDialogue();
        // Debug.Log("Scene Length: " + sceneLength.ToString());
    }

    public void SelectBark()
    {
        sceneIndex = dialogueArrays.PickOrderBark();
        sceneLength = dialogueArrays.orderBarksList.dialogueSets[sceneIndex].dialogueSlides.Count;

        UpdateBarkDialogue();
    }
}
