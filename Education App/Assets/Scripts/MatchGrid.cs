using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchGrid : MonoBehaviour
{
    public Transform gridTopLeft;
    public Transform gridBottomRight;
    public int numOfRows = 4;
    public int numOfColumns = 4;

    private MatchPair[] studySet;
    private MatchPair[] pendingSet;
    private int gridSize;

    // Start is called before the first frame update
    void Start()
    {
        gridSize = numOfRows * numOfColumns;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetStudySet(MatchPair[] set)
    {
        studySet = set;
    }

}
