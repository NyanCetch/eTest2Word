using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace eTest2Word.Core.Parsers
{
    /// <summary>
    /// Парсер для задания типа "введите ответ"
    /// </summary>
    public class QuestionParserShortAnswer : QuestionParserBase
    {
        protected override void ParseAnswers(HtmlNode node, out QuestionBase question)
        {
            var result = new QuestionShortAnswer();

            var inputNodes = node.SelectNodes("//input[@type='text']");
            var valueList = new List<string>();
            
            if (inputNodes != null && inputNodes.Count > 0)
            {
                foreach (var inputNode in inputNodes)
                {
                    var value = HtmlTestUtility.GetInputValue(inputNode);
                    valueList.Add(value);
                }
            }
            result.Answers = valueList.Distinct().ToArray();
            
            question = result;
        }
    }
}