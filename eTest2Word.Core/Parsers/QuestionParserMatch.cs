using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace eTest2Word.Core.Parsers
{
    public class QuestionParserMatch : QuestionParserBase
    {
        protected override void ParseAnswers(HtmlNode node, out QuestionBase question)
        {
            var result = new QuestionMatch
            {
                MatchMap = new Dictionary<string, string>()
            };

            var matchItemNodes = node.SelectNodes("//tr[starts-with(@class, 'r')]");

            // Достать все подписи и значения из поле ввода

            foreach (var matchItemNode in matchItemNodes.ToArray())
            {
                var leftValue = matchItemNode.Descendants().First(d => d.Name == "p").InnerText;
                var options = matchItemNode.Descendants().Where(d => d.Name == "option").ToArray();
                var selectedValue = options.First(d => d.Attributes.Contains("selected")).InnerText;
                result.MatchMap[leftValue] = selectedValue;
            }

            /*foreach (var row in rows)
            {
                var leftValue = row.SelectSingleNode("/td[1]/p").InnerText;
                var selectedValue = row.SelectSingleNode("//option[@selected]").InnerText;
                
                result.MatchMap[leftValue] = selectedValue;
            }*/

            question = result;
        }
    }
}