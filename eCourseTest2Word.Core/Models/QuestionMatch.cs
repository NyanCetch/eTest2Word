using System.Collections.Generic;

public class QuestionMatch : QuestionBase
{
    public Dictionary<string, string> MatchMap { get; set; }

    public QuestionMatch()
    {
        Type = QuestionType.Match;
    }
}