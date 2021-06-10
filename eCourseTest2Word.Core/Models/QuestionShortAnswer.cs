public class QuestionShortAnswer : QuestionBase
{
    public string[] Answers { get; set; }

    public QuestionShortAnswer()
    {
        Type = QuestionType.ShortAnswer;
    }
}