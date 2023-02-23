using System.Collections;
using System.Collections.Generic;

public class MatchPair
{
    private string word;
    private string definiton;

    public void SetWord(string word)
    {
        this.word = word;
    }

    public void SetDefintion(string definition)
    {
        this.definiton = definition;
    }

    public string GetWord()
    {
        return word;
    }

    public string GetDefintion()
    {
        return definiton;
    }
}
