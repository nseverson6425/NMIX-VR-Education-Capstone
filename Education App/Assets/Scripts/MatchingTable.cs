using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MatchingTable : MonoBehaviour
{
    public TMP_Text helpMessage;
    public Button checkButton;

    private List<MatchBlock> matchBlocks;
    private bool readyToMatch = true;

    // Start is called before the first frame update
    void Start()
    {
        matchBlocks = new List<MatchBlock>();
        helpMessage.text = "Place blocks here to match!";
        checkButton.gameObject.SetActive(false);
        checkButton.onClick.AddListener(CheckMatch);
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
        }
        else
        {
            // they don't match
            helpMessage.text = "That doesn't look quite right";
            readyToMatch = false;
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
