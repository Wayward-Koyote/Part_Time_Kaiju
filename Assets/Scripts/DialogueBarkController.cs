using UnityEngine;

public class DialogueBarkController : MonoBehaviour
{
    [SerializeField] DialogueUpdater updater;
    [SerializeField] float advanceTime = 2.5f;

    private float timeLeft;
    private bool barkActive;

    void Update()
    {
        if (barkActive)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                timeLeft = 0;
                barkActive = false;

                updater.DisableDialogue();
            }
        }
    }
    
    public void OrderBark()
    {
        updater.DisableDialogue();
        updater.SelectBark();
        updater.EnableBarkDialogue();
        updater.UpdateBarkDialogue();
        timeLeft = advanceTime;
        barkActive = true;
    }
}
