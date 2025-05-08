using TMPro;
using UnityEngine;

public class DialogueUpdater : MonoBehaviour
{
    [SerializeField] LevelController levelController;
    [SerializeField] DialogueArrays dialogueArrays;
    
    [SerializeField] GameObject sceneDialogueUI;
    [SerializeField] TMP_Text sceneDialogueText;
    [SerializeField] TMP_Text sceneSpeakerName;

    [SerializeField] GameObject barkDialogueUI;
    [SerializeField] TMP_Text barkDialogueText;
    [SerializeField] TMP_Text barkSpeakerName;

    private int sceneIndex = 0;
    private int sceneLength = 0;
    private int slideIndex = 0;

    public void EnableSceneDialogue()
    {
        slideIndex = 0;
        sceneDialogueUI.SetActive(true);
    }
    public void EnableBarkDialogue()
    {
        slideIndex = 0;
        barkDialogueUI.SetActive(true);
    }

    // Disable all dialogue panels
    public void DisableDialogue()
    {
        sceneDialogueUI.SetActive(false);
        barkDialogueUI.SetActive(false);
    }

    public void UpdateSceneDialogue()
    {
        if (slideIndex < sceneLength)
        {
            sceneSpeakerName.SetText(dialogueArrays.storyDialoguesList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].SpeakerName());
            sceneSpeakerName.color = dialogueArrays.storyDialoguesList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].SpeakerColor();
            sceneDialogueText.SetText(dialogueArrays.storyDialoguesList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].DialogueText());
            sceneDialogueText.color = dialogueArrays.storyDialoguesList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].SpeakerColor();

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
            barkSpeakerName.SetText(dialogueArrays.orderBarksList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].SpeakerName());
            barkSpeakerName.color = dialogueArrays.orderBarksList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].SpeakerColor();
            barkDialogueText.SetText(dialogueArrays.orderBarksList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].DialogueText());
            barkDialogueText.color = dialogueArrays.orderBarksList.dialogueSets[sceneIndex].dialogueSlides[slideIndex].SpeakerColor();

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
