using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace eCourseTest2Word.Core.Parsers
{
    public abstract class QuestionParserBase
    {
        /// <summary>
        /// Парсит информацию по заданию
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public QuestionBase Parse(HtmlNode node)
        {
            ParseAnswers(node, out var question);
            question.DescriptionItems = ParseDescription(node);
            question.StatusType = ParseAnswerStatus(node);

            return question;
        }

        /// <summary>
        /// Парсит ответы задания
        /// </summary>
        /// <param name="node"></param>
        /// <param name="question"></param>
        protected abstract void ParseAnswers(HtmlNode node, out QuestionBase question);

        /// <summary>
        /// Парсит из HTML-представления задания описание
        /// </summary>
        /// <param name="node">HTML-нода задания</param>
        /// <exception cref="ArgumentException">Возникает если дочерние элементы ноды не были упрощены до текста или изображения</exception>
        private QuestionBase.DescriptionItem[] ParseDescription(HtmlNode node)
        {
            // В случае если никакое описание не найдено, возвращаем пустой массив
            var descriptionNode = node.SelectSingleNode("//div[@class='qtext']");
            if (descriptionNode?.ChildNodes == null || descriptionNode.ChildNodes.Count == 0)
                return new QuestionBase.DescriptionItem[0];
            
            // Не забываем про то, что перед сборкой описания,
            // блок вопроса нужно предварительно подготовить
            HtmlTestUtility.SimplifyQuestionBlock(descriptionNode);
            
            var itemList = new List<QuestionBase.DescriptionItem>();
            foreach (var childNode in descriptionNode.ChildNodes)
            {
                var item = new QuestionBase.DescriptionItem();
            
                if (HtmlTestUtility.IsNonEmptyTextNode(childNode))
                {
                    item.Type = QuestionBase.DescriptionItem.ItemType.Text;
                    item.Content = WebUtility.HtmlDecode(childNode.InnerText);
                }
                else if (HtmlTestUtility.IsImageNode(childNode))
                {
                    item.Type = QuestionBase.DescriptionItem.ItemType.Image;
                    item.Content = childNode.GetAttributeValue("src", string.Empty);
                }
                else
                {
                    throw new ArgumentException("Среди дочерних элементов есть элемент, который не является текстом или изображением!");
                }
            
                itemList.Add(item);
            }

            return itemList.ToArray();
        }

        /// <summary>
        /// Парсит состояние ответа на задание на основании количества полученных баллов
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private QuestionBase.AnswerStatusType ParseAnswerStatus(HtmlNode node)
        {
            var gradeNode = node.SelectSingleNode("//*[@class='grade']");
            if (gradeNode != null)
            {
                var gradeText = gradeNode.InnerText.Replace(",", ".");
                var match = Regex.Match(gradeText, @".*(?'earned'[01]\.[0-9]).*(?'total'(\?1))");
                
                if (match.Success)
                {
                    var earned = double.Parse(match.Groups["earned"].Value);
                    var total = double.Parse(match.Groups["total"].Value);

                    switch (earned)
                    {
                        case 0.0:
                            return QuestionBase.AnswerStatusType.Incorrect;
                    
                        case 1.0:
                            return QuestionBase.AnswerStatusType.Correct;
                    
                        default:
                            return QuestionBase.AnswerStatusType.Partially;
                    }
                }
            }
            
            return QuestionBase.AnswerStatusType.None;
        }
    }
}