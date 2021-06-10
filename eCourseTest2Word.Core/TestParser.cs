using System.Collections.Generic;
using System.Linq;
using eCourseTest2Word.Core.Parsers;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace eCourseTest2Word.Core
{
    public class TestParser
    {
        /// <summary>
        /// Словарь кэшированных парсеров для каждого типа задания
        /// </summary>
        private readonly Dictionary<string, QuestionParserBase> _parserMap;

        public TestParser()
        {
            _parserMap = new Dictionary<string, QuestionParserBase>();
        }

        /// <summary>
        /// Парсит тест и достает из него задания
        /// </summary>
        /// <param name="testHtmlText"> HTML-тест в виде текста </param>
        /// <returns> Массив собранных заданий </returns>
        public QuestionBase[] Parse(string testHtmlText)
        {
            // Выделить все блоки с вопросами
            // Для каждого блока определить тип
            // Соотнести тип с обработчиком блока

            var document = new HtmlDocument();
            document.LoadHtml(testHtmlText);
            var root = document.DocumentNode;

            var questionNodes = root.QuerySelectorAll("div[id^='question']");
            var parsedQuestionList = new List<QuestionBase>();
            foreach (var questionNode in questionNodes)
            {
                var parser = GetQuestionParser(questionNode);
                var question = parser.Parse(questionNode);
                parsedQuestionList.Add(question);
            }

            return parsedQuestionList.ToArray();
        }

        /// <summary>
        /// Выдает парсер для задания
        /// </summary>
        /// <param name="questionNode"> HTML-элемент задания </param>
        /// <returns> Парсер нужного типа </returns>
        private QuestionParserBase GetQuestionParser(HtmlNode questionNode)
        {
            // Выделить из класса тип
            // Если для типа есть парсер, отдать парсер
            // Если для типа нет парсера, создать, сохранить и вернуть

            var questionType = questionNode.GetClasses().ToArray()[1];
            if (_parserMap.ContainsKey(questionType))
                return _parserMap[questionType];

            var parser = (QuestionParserBase) null;
            switch (questionType)
            {
                case "multichoice":
                    parser = new QuestionParserMultiChoice();
                    break;

                case "match":
                    parser = new QuestionParserMatch();
                    break;

                case "shortanswer":
                    parser = new QuestionParserShortAnswer();
                    break;
            }
            _parserMap[questionType] = parser;
            
            return parser;
        }
    }
}
