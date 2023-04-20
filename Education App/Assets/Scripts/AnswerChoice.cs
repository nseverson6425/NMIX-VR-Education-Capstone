using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnswerChoice : MonoBehaviour
{
    [SerializeField] QuestionManager manager;
    [SerializeField] GameObject explosionPrefab;

    public TextMeshProUGUI choice;
    private bool canAlert;
    private SizeStatus status;

    // scaling variables
    private Vector3 normalScale;
    [SerializeField] float duration = 1f;

    private enum SizeStatus
    {
        Normal,
        Shrunk
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
        if (canAlert)
        {
            //manager.CheckAnswer(choice.text);
            canAlert = false;

            // shrink and blow up
            ReduceSize();

            // Instantiate explosion prefab
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    public void ResetChoice()
    {
        choice.text = "";
        canAlert = true;

        if (status == SizeStatus.Shrunk) // need to return to normal size
        {
            ReturnToSize();
        }
    }

    private void ReduceSize()
    {
        StartCoroutine(Shrink());
    }

    private void ReturnToSize()
    {
        StartCoroutine(Expand());
    }

    public IEnumerator Shrink()
    {
        normalScale = transform.localScale;

        float elapsedTime = 0f;
        while (transform.localScale.magnitude > 0.1f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(normalScale, Vector3.one * 0.01f, t);
            yield return null;
        }
        transform.localScale = Vector3.one * 0.01f;
    }

    public IEnumerator Expand()
    {
        float elapsedTime = 0f;
        while (transform.localScale != normalScale)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(Vector3.one * 0.01f, normalScale, t);
            yield return null;
        }
        transform.localScale = normalScale;
    }
}
 