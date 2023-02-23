using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BlockType
{
    Undefined,
    Definition,
    Word
}
public class MatchBlock : MonoBehaviour
{
    public BlockType type = BlockType.Undefined;
    public string content = "[Something goes here]";
    public string matchContent = "[Matches with me]";
    public TMP_Text label;

    private void Update()
    {
        label.text = content;
    }

    private void UpdateText()
    {
        if (label != null)
        {
            label.text = content;
        }
        else
        {
            Debug.LogWarning("Text component for " + gameObject.name + " is missing.");
        }
    }

    public void SetContent(string content)
    {
        this.content = content;
        UpdateText();
    }

    public void SetMatch(string match)
    {
        matchContent = match;
    }

    // true if match, false if not
    public bool CheckMatch(string content) 
    {
        return matchContent.Equals(content);
    }
}
