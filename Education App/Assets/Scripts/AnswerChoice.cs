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

    private void Update()
    {

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
        Debug.Log("choice being reset");
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
        status = SizeStatus.Shrunk;
        //normalScale = transform.localScale;

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
        status = SizeStatus.Normal;
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
 