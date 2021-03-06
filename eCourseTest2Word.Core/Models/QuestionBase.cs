public abstract class QuestionBase
{
    public enum QuestionType
    {
        Match,
        ShortAnswer,
        MultiChoice
    }

    public class DescriptionItem
    {
        public enum ItemType
        {
            Text,
            Image
        }
        
        public ItemType Type { get; set; }
        public string Content { get; set; }
    }

    public enum AnswerStatusType
    {
        None,
        Correct,
        Incorrect,
        Partially
    }
    
    public DescriptionItem[] DescriptionItems { get; set; }
    public AnswerStatusType StatusType { get; set; }
    public QuestionType Type { get; protected set; }
}