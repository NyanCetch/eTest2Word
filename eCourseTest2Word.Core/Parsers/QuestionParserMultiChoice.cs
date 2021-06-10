using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace eCourseTest2Word.Core.Parsers
{
    public class QuestionParserMultiChoice : QuestionParserBase
    {
        protected override void ParseAnswers(HtmlNode node, out QuestionBase question)
        {
            var result = new QuestionMultiChoice();

            var options = node.SelectNodes("//div[starts-with(@class, 'r')]");

            var answerList = new List<string>();
            var selectedList = new List<int>();
            for (var i = 0; i < options.Count; ++i)
            {
                var ind = i;
                var option = options[i];
                
                var text = option.Descendants().First(n => n.Name == "p").InnerText;
                answerList.Add(text);

                var isChecked = option.Descendants().First(n => n.Name == "input")
                    .GetAttributes().Any(attr => attr.Name == "checked");
                
                if (isChecked)
                    selectedList.Add(ind);
            }

            result.AnswerMethod = QuestionMultiChoice.AnswerMethodType.OneOption;
            var anyInput = options.Descendants().First(n => n.Name == "input");
            if (anyInput.GetAttributeValue("type", string.Empty) == "checkbox")
                result.AnswerMethod = QuestionMultiChoice.AnswerMethodType.SeveralOptions;

            result.Options = answerList.ToArray();
            result.Selected = selectedList.ToArray();

            question = result;
        }
    }
}