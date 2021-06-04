public class QuestionMultiChoice : QuestionBase
{
    public enum AnswerMethodType
    {
        SeveralOptions,
        OneOption
    }
    
    public AnswerMethodType AnswerMethod { get; set; }
    public string[] Options { get; set; }
    public int[] Selected { get; set; }
}