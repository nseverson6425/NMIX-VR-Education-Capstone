using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnswerController : MonoBehaviour
{
    public TextMeshProUGUI text; // reference to display text
    public string displayAnswer; // answer to display
    public SlideGameController gameController; // reference for slide game controller
    public Material correctMaterial; // display color when answer is right
    public Material incorrectMaterial; // display color when answer is wrong
    public Material awaitingMaterial; // display color when answer is correct and waiting for another selection
    public Material defaultMaterial; // default black material
    public MeshRenderer parentMeshRenderer; // reference to display mesh renderer
    public bool triggerAnswerSelected = false;


    private bool answerSelected = false; // true if this answer has been selected
    private bool canAcceptSelection = true; // true if not already selected

    // reference to projectile that selected this answer
    private ProjectileController targetProjectile;
    private SlideGameController.CheckAnswerStatus returnStatus; // status of the answer

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (triggerAnswerSelected)
        {
            triggerAnswerSelected = false;
            answerSelected = true;
            processReturnStatus();
        }
    }

    private void processReturnStatus()
    {
        canAcceptSelection = false;
        returnStatus = gameController.CheckAnswer(displayAnswer);

        switch (returnStatus)
        {
            case SlideGameController.CheckAnswerStatus.Correct:
                parentMeshRenderer.material = correctMaterial;

                break;
            case SlideGameController.CheckAnswerStatus.Incorrect:
                parentMeshRenderer.material = incorrectMaterial;

                break;
            case SlideGameController.CheckAnswerStatus.AwaitingNextAnswer:
                parentMeshRenderer.material = awaitingMaterial;

                break;
            default:
                Debug.LogError("AnswerController: something has gone wrong with checking answer status");
                break;
        }

    }

    public void InitiateDestruction()
    {
        answerSelected = false;
        canAcceptSelection = true;
        parentMeshRenderer.gameObject.SetActive(false);
        parentMeshRenderer.material = defaultMaterial; // reset color
    }

    public void ResetDisplay()
    {
        parentMeshRenderer.gameObject.SetActive(true);
    }

    // replace display text
    public void SetAnswer(string answer)
    {
        Debug.Log("set display answer as " + answer);
        displayAnswer = answer; 
        text.SetText(answer); // replace display text
    }

    // set reference to scene slide game controller
    public void SetSlideGameControllerReference(SlideGameController sGC)
    {
        gameController = sGC;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canAcceptSelection)
        {
            // get reference to projectile if available
            ProjectileController pc = other.GetComponent<ProjectileController>();
            if (pc != null)
            {
                answerSelected = true;
                targetProjectile = pc;
                targetProjectile.Explode();
                processReturnStatus();
            }
        }
    }
}
