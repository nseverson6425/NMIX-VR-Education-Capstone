using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MatchingTable : MonoBehaviour
{
    public TMP_Text helpMessage;
    public Button checkButton;
    public Button spawnButton;
    public TMP_Text scoreCount;
    public GameObject blockPrefab;
    public Transform spawnPoint;

    private int score;
    private List<MatchBlock> matchBlocks;
    private bool readyToMatch = true;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        matchBlocks = new List<MatchBlock>();
        helpMessage.text = "Place blocks here to match!";
        checkButton.gameObject.SetActive(false);
        checkButton.onClick.AddListener(CheckMatch);
        spawnButton.onClick.AddListener(SpawnBlock);
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToMatch)
        {
            int length = matchBlocks.Count;
            switch (length)
            {
                case 0:
                    // need blocks match
                    helpMessage.text = "Place blocks here to match!";
                    break;
                case 1:
                    // need one more block
                    helpMessage.text = "Find its pair";
                    break;
                case 2:
                    // allow check
                    helpMessage.text = "Press button to check";
                    checkButton.gameObject.SetActive(true);
                    //CheckMatch();
                    break;
                default:
                    // too many blocks
                    helpMessage.text = "Only place two blocks on this table!";
                    break;
            }
        }
    }

    private void SpawnBlock()
    {
        Instantiate(blockPrefab, spawnPoint);
    }

    IEnumerator WaitAndPrint(MatchBlock m1, MatchBlock m2)
    {
        // suspend execution for 5 seconds
        yield return new WaitForSeconds(2);
        Destroy(m1);
        Destroy(m2);
        readyToMatch = true;
    }

    private void CheckMatch()
    {
        MatchBlock m1 = matchBlocks[0];
        MatchBlock m2 = matchBlocks[1];
        
        // check if they match each other
        if (m1.CheckMatch(m2.content))
        {
            // they match
            helpMessage.text = "It's a match!";
            readyToMatch = false;
            score += 1;
            scoreCount.text = score.ToString();
            coroutine = WaitAndPrint(m1, m2);
            StartCoroutine(coroutine);
        }
        else
        {
            // they don't match
            helpMessage.text = "That doesn't look quite right";
            readyToMatch = false;
            coroutine = WaitAndPrint(m1, m2);
            StartCoroutine(coroutine);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MatchBlock mb = other.GetComponent<MatchBlock>();
        if (mb != null)
        {
            matchBlocks.Add(mb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MatchBlock mb = other.GetComponent<MatchBlock>();
        if (mb != null)
        {
            matchBlocks.Remove(mb);
        }
    }
}
