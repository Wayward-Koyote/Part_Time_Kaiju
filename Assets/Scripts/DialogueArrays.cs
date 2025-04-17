using UnityEngine;
using System.Collections.Generic;

public class DialogueArrays : MonoBehaviour
{
    /* Scene Arrays */
    [SerializeField] public DialoguesList storyDialoguesList;

    /* Bark Arrays */
    //TBD

    public string GetSpeakerName(int _set, int _slide)
    {
        return storyDialoguesList.dialogueSets[_set].dialogueSlides[_slide].SpeakerName();
    }

    public Color GetSpeakerColor(int _set, int _slide)
    {
        return storyDialoguesList.dialogueSets[_set].dialogueSlides[_slide].SpeakerColor();
    }

    public string GetDialogueText(int _set, int _slide)
    {
        return storyDialoguesList.dialogueSets[_set].dialogueSlides[_slide].DialogueText();
    }

    public int FindStorySceneIndex(string _sceneName)
    {
        for (int i = 0; i < storyDialoguesList.dialogueSets.Count; i++)
        {
            if (storyDialoguesList.dialogueSets[i].CompareName(_sceneName))
            {
                return i;
            }
        }
        return -1;
    }

    /* Dialogue List Classes */
    [System.Serializable]
    public class DialogueSlide
    {
        [SerializeField] string speakerName;
        [SerializeField] Color speakerColor;
        [SerializeField] string dialogueText;
        
        public string SpeakerName()
        {
            return speakerName;
        }
        public Color SpeakerColor()
        {
            return speakerColor;
        }
        public string DialogueText()
        {
            return dialogueText;
        }
    }
    [System.Serializable]
    public class DialogueSet
    {
        [SerializeField] string dialogueSceneName;

        public List<DialogueSlide> dialogueSlides;

        public bool CompareName(string _name)
        {
            if (dialogueSceneName == _name)
                return true;
            else
                return false;
        }
    }
    [System.Serializable]
    public class DialoguesList
    {
        public List<DialogueSet> dialogueSets;
    }
}


