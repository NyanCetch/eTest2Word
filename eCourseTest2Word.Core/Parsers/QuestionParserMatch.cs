using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace eCourseTest2Word.Core.Parsers
{
    public class QuestionParserMatch : QuestionParserBase
    {
        protected override void ParseAnswers(HtmlNode node, out QuestionBase question)
        {
            var result = new QuestionMatch
            {
                MatchMap = new Dictionary<string, string>()
            };

            var matchItemNodes = node.QuerySelectorAll("tr[class^='r']");

            // Достать все подписи и значения из поле ввода

            foreach (var matchItemNode in matchItemNodes.ToArray())
            {
                var leftValue = matchItemNode.QuerySelector("td[class='text'] > p").InnerText;
                var options = matchItemNode.QuerySelectorAll("option");
                var selectedValue = options.First(d => d.Attributes.Contains("selected")).InnerText;
                
                result.MatchMap[leftValue] = selectedValue;
            }

            question = result;
        }
    }
}