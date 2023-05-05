using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnswerChoice : MonoBehaviour
{
    [SerializeField] QuestionManager manager;
    [SerializeField] GameObject explosionPrefab;
    public bool triggerSelection = false;

    public TextMeshProUGUI choice;
    private bool canAlert;
    private SizeStatus status;

    // scaling variables
    private Vector3 normalScale;
    [SerializeField] float duration = 1f;
    private bool isShrinking = false;
    private bool isExpanding = false;
    private bool needShrink = false;
    private bool needExpand = false;

    private enum SizeStatus
    {
        Normal,
        Shrunk
    }

    private void Update()
    {
        if (triggerSelection)
        {
            triggerSelection = false;
            Debug.LogWarning("Inspector triggered answer choice; " + choice.text + " ; " + canAlert);
            AlertChoice();
        }
    }

    private void Start()
    {
        canAlert = true;
        status = SizeStatus.Normal;
        normalScale = transform.localScale;
    }

    // alert QuestionManager that this choice was selected for the question
    public void AlertChoice()
    {
        if (canAlert || status == SizeStatus.Normal)
        {
            // shrink and blow up
            ReduceSize();

            manager.CheckAnswer(choice.text);
            canAlert = false;


            // Instantiate explosion prefab
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    public void ResetChoice()
    {
        //Debug.Log("choice being reset");
        choice.text = "";
        canAlert = true;

        ReturnToSize();
    }

    private void ReduceSize()
    {
        //transform.localScale = Vector3.one * 0.01f;

        StartCoroutine(Shrink());
    }

    private void ReturnToSize()
    {
        //transform.localScale = normalScale;

        StartCoroutine(Expand());
    }

    public IEnumerator Shrink()
    {
        isShrinking = true;

        if (isExpanding) // check if shrinking
        {
            needShrink = true; // indicate needs to shrink
            yield break; // stop coroutine
        }

        float elapsedTime = 0f;
        while (transform.localScale.magnitude > 0.1f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(normalScale, Vector3.one * 0.01f, t);
            yield return null;
        }
        transform.localScale = Vector3.one * 0.01f;
        
        status = SizeStatus.Shrunk;
        isShrinking = false;
        if (needExpand) // reset size
        {
            needExpand = false;
            ReturnToSize();
        }
    }

    public IEnumerator Expand()
    {
        isExpanding = true;

        if (isShrinking)
        {
            needExpand = true; // indicate need to expand
            yield break; // stop coroutine
        }

        float elapsedTime = 0f;
        while (transform.localScale != normalScale)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(Vector3.one * 0.01f, normalScale, t);
            yield return null;
        }
        transform.localScale = normalScale;

        status = SizeStatus.Normal;
        isExpanding = false;
        if (needShrink)
        {
            needShrink = false;
            ReduceSize();
        }
    }
}
 