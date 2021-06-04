using System;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace eTest2Word.Core
{
    public class SimpleParser
    {
        /// <summary>
        /// Проверяет, можно ли упростить узел и дальше
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsComplex(HtmlNode node)
        {
            return (IsImageNode(node) || IsTextNode(node)) == false;
        }

        public bool IsTextNode(HtmlNode node)
        {
            return node.Name is "#text";
        }

        public bool IsImageNode(HtmlNode node)
        {
            return node.Name is "img";
        }

        public void SimplifyQuestionBlock(HtmlNode node)
        {
            // По очереди пробежаться по всем детям, упростить их до текста или изображения
            // и обернуть в параграфы
            for (var i = 0; i < node.ChildNodes.Count; ++i)
            {
                var childNode = node.ChildNodes[i];
                var simplifiedNode = SimplifyBlock(childNode);
                if (simplifiedNode == null)
                    continue;
                
                var newNode = HtmlNode.CreateNode($"<p>{simplifiedNode.WriteTo()}</p>");
                node.ReplaceChild(newNode, childNode);
            }
        }
        
        /// <summary>
        /// Упрощает Html-элемент до простого текста или элемента изображения
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public HtmlNode SimplifyBlock(HtmlNode node)
        {
            // Если уже текст или изображение, их же и возвращаем обратно
            if (!IsComplex(node))
                return node;

            // Убираем ненужную подпись
            if (node.Name is "label")
                return null;
            
            // Из поле ввода вытаскиваем значение
            if (node.Name is "input")
                return HtmlNode.CreateNode(node.GetAttributeValue("value", string.Empty));
            
            // У пустых и непарных тегов ничего нет
            if (node.ChildNodes == null || node.ChildNodes.Count == 0)
                return null;
            
            while (node.ChildNodes.Any(IsComplex))
            {
                var complexNode = node.ChildNodes.First(IsComplex);
                var simpleNode = SimplifyBlock(complexNode);
                
                // Если из элемента не удалось достать ничего,
                // тогда убираем за ненадобностью
                if (simpleNode == null || IsTextNode(simpleNode) && string.IsNullOrEmpty(simpleNode.InnerText))
                {
                    node.RemoveChild(complexNode);
                    continue;
                }

                // После успешного упрощения, заменяем элемент на его упрощенную версию
                node.ReplaceChild(simpleNode, complexNode);
                
                // Также не забываем про склейку текстовых элементов,
                // т.к. они сами это не сделают
                if (NeedMerge(node))
                    MergeTextNodes(node);
            }

            return node.FirstChild;
        }

        /// <summary>
        /// Проверка на то, нужно ли склеивать текстовые элементы у ноды
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool NeedMerge(HtmlNode node)
        {
            return node.ChildNodes.Count(IsTextNode) > 1;
        }

        /// <summary>
        /// Объединит одинаковые блоки текста
        /// </summary>
        /// <param name="parent"></param>
        public void MergeTextNodes(HtmlNode parent)
        {
            var textNodes = parent.ChildNodes.Where(IsTextNode);
            var fullText = string.Join("", textNodes.Select(n => n.InnerText));
            var fullTextNode = HtmlNode.CreateNode(fullText);

            parent.ChildNodes.Clear();
            parent.AppendChild(fullTextNode);
        }
    }
}