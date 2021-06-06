using System.Collections.Generic;
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

            foreach (var matchItemNode in matchItemNodes)
            {
                var leftValue = matchItemNode.SelectSingleNode("/td[@class='text']/p").InnerText;
                var selectedValue = matchItemNode.SelectSingleNode("//option[@selected]").InnerText;

                result.MatchMap[leftValue] = selectedValue;
            }
            
            question = result;
        }
    }
}