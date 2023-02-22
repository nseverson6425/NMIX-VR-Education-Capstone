using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Undefined,
    Definition,
    Word
}
public class MatchBlock : MonoBehaviour
{
    [SerializeField] BlockType type = BlockType.Undefined;
    [SerializeField] string content = "[Something goes here]";
}
