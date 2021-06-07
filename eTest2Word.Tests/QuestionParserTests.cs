using System;
using System.IO;
using eTest2Word.Core.Parsers;
using NUnit.Framework;

namespace eTest2Word.Tests
{
    [TestFixture]
    public class QuestionParserTests
    {
        [TestCase("ShortAnswer_1", TestName = "Сбор ответов из поле ввода")]
        public void ShortAnswerTest(string testName)
        {
            Assert.IsTrue(Helpers.TryCheck(testName, node =>
            {
                var parser = new QuestionParserShortAnswer();
                var question = (QuestionShortAnswer) parser.Parse(node);

                return question.Answers;
            }));
        }

        [TestCase("Match_1", TestName = "Соответствие ответов")]
        public void MatchTest(string caseName)
        {
            Assert.IsTrue(Helpers.TryCheck(caseName, node =>
            {
                var parser = new QuestionParserMatch();
                var question = (QuestionMatch) parser.Parse(node);

                return question.MatchMap;
            }));
        }
    }
}