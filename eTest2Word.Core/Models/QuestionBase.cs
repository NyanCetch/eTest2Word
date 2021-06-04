public abstract class QuestionBase
{
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
        Correct,
        Incorrect,
        Partially
    }
    
    public DescriptionItem[] DescriptionItems { get; set; }
    public AnswerStatusType StatusType { get; set; }
}