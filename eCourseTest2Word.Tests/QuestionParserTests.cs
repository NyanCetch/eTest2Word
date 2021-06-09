using System;
using System.IO;
using eTest2Word.Core.Parsers;
using NUnit.Framework;

namespace eTest2Word.Tests
{
    [TestFixture]
    public class QuestionParserTests
    {
        [TestCase("ShortAnswer_1", TestName = "Поля для ответов встроены в текст вопроса")]
        public void ShortAnswerTest(string testName)
        {
            Assert.IsTrue(Helpers.IsCheckPartiallyCorrect(testName, node =>
            {
                var parser = new QuestionParserShortAnswer();
                var question = (QuestionShortAnswer) parser.Parse(node);

                return question.Answers;
            }));
        }

        [TestCase("Match_1", TestName = "Соответствие ответов")]
        public void MatchTest(string caseName)
        {
            Assert.IsTrue(Helpers.IsCheckPartiallyCorrect(caseName, node =>
            {
                var parser = new QuestionParserMatch();
                var question = (QuestionMatch) parser.Parse(node);

                return question.MatchMap;
            }));
        }

        [TestCase("MultiChoice_1", TestName = "Один верный")]
        [TestCase("MultiChoice_2", TestName = "Несколько верных")]
        public void MultiChoiceTest(string caseName)
        {
            Assert.IsTrue(Helpers.IsCheckPartiallyCorrect(caseName, node =>
            {
                var parser = new QuestionParserMultiChoice();
                var question = (QuestionMultiChoice) parser.Parse(node);

                return question;
            }));
        }
    }
}