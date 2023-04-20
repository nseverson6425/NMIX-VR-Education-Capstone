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
    public ParticleSystem explosionSparks; // reference to explosion sparks
    public AudioSource explosionSound;

    private bool canAcceptSelection = true; // true if not already selected

    // reference to projectile that selected this answer
    private ProjectileController targetProjectile;
    private SlideGameController.CheckAnswerStatus returnStatus; // status of the answer

    private GameObject controllerParent; // parent to this controller

    public bool causeExplode = false; // cause this display to explode
    public bool causeDisplayReset = false; // reset display state
    public float shrinkRate = 2f; // rate at which display shrinks during explosion

    public float minExplodeDelay = 0f; // minimum time to wait before exploding
    public float maxExplodeDelay = 0.3f; // max time to wait before exploding
    public float displayResetDelay = 0.5f; // delay before resetting displays

    private bool useExplosionEffect = true;
    private bool canResetScreen = true;

    // Start is called before the first frame update
    void Start()
    {
        controllerParent = parentMeshRenderer.gameObject;
        if (controllerParent == null)
        {
            Debug.LogError("AnswerController: unset parent mesh renderer");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerAnswerSelected)
        {
            triggerAnswerSelected = false;
            processReturnStatus();
        }
        if (causeExplode)
        {
            causeExplode = false;
            InitiateDestruction(true);
        }
        if (causeDisplayReset)
        {
            causeDisplayReset = false;
            ResetDisplay();
        }
    }

    private void processReturnStatus()
    {
        canAcceptSelection = false;
        //returnStatus = gameController.CheckAnswer(displayAnswer);

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

    public void InitiateDestruction(bool allowReset)
    {
        Debug.Log("AnswerController: display is self destructing");
        canAcceptSelection = true;
        if (!allowReset)
        {
            canResetScreen = false;
        }

        StartCoroutine(ScaleToTargetDelayCoroutine());
    }

    private IEnumerator ScaleToTargetDelayCoroutine()
    {
        Debug.Log("ScaleToTargetDelayCoroutine running");
       
        yield return new WaitForSeconds(Random.Range(minExplodeDelay, maxExplodeDelay));
        ScaleToTarget(new Vector3(0.1f, 0.1f, 0.1f), shrinkRate); // reduce scale to 0.1 so that it shrinks into explosion

        yield return new WaitForSeconds(displayResetDelay);
        if (canResetScreen)
        {
            ResetDisplay();
        }
    }

    private void ScaleToTarget(Vector3 targetScale, float duration)
    {
        StartCoroutine(ScaleToTargetCoroutine(targetScale, duration));
    }

    private IEnumerator ScaleToTargetCoroutine(Vector3 targetScale, float duration)
    {
        Vector3 startScale = controllerParent.transform.localScale;
        float timer = 0.0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            //smoother step algorithm
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            controllerParent.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        //parentMeshRenderer.gameObject.SetActive(false);
        parentMeshRenderer.material = defaultMaterial; // reset color
        if (useExplosionEffect)
        {
            //explosionSound.Play();
            explosionSparks.Play();
        } 
        else
        {
            useExplosionEffect = true;
        }

        yield return null;
    }

    public void ResetDisplay()
    {
        //Debug.Log("Display is being reset");
        controllerParent.SetActive(true);
        useExplosionEffect = false;
        ScaleToTarget(new Vector3(0.1f, 2, 4), shrinkRate); // reset size
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
                targetProjectile = pc;
                targetProjectile.Explode();
                processReturnStatus();
            }
        }
    }
}
