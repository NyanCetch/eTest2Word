using System.Linq;
using HtmlAgilityPack;

namespace eTest2Word.Core
{
    public static class HtmlTestUtility
    {
        public const string INPUT_EMPTY_PLACEHOLDER = "<пусто>";

        /// <summary>
        /// Проверяет, можно ли упростить узел и дальше
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool IsComplex(HtmlNode node)
        {
            return (IsImageNode(node) || IsTextNode(node)) == false;
        }

        public static bool IsTextNode(HtmlNode node)
        {
            return node.Name is "#text";
        }

        public static bool IsImageNode(HtmlNode node)
        {
            return node.Name is "img";
        }

        /// <summary>
        /// Метод для подготовки ноды с заданием теста
        /// убирает лишнюю стилизацию и оставляет голый текст и картинки
        /// </summary>
        /// <param name="node"></param>
        public static void SimplifyQuestionBlock(HtmlNode node)
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

        public static string GetInputValue(HtmlNode inputNode)
        {
            var value = inputNode.GetAttributeValue("value", string.Empty);
            if (string.IsNullOrEmpty(value))
                value = INPUT_EMPTY_PLACEHOLDER;

            return value;
        }
        
        /// <summary>
        /// Упрощает Html-элемент до простого текста или элемента изображения
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static HtmlNode SimplifyBlock(HtmlNode node)
        {
            // Если уже текст или изображение, их же и возвращаем обратно
            if (!IsComplex(node))
                return node;

            // Убираем ненужную подпись
            if (node.Name is "label")
                return null;
            
            // Из поле ввода вытаскиваем значение
            if (node.Name is "input")
                return HtmlNode.CreateNode(GetInputValue(node));
            
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
        private static bool NeedMerge(HtmlNode node)
        {
            return node.ChildNodes.Count(IsTextNode) > 1;
        }

        /// <summary>
        /// Объединит одинаковые блоки текста
        /// </summary>
        /// <param name="parent"></param>
        private static void MergeTextNodes(HtmlNode parent)
        {
            var textNodes = parent.ChildNodes.Where(IsTextNode);
            var fullText = string.Join("", textNodes.Select(n => n.InnerText));
            var fullTextNode = HtmlNode.CreateNode(fullText);

            parent.ChildNodes.Clear();
            parent.AppendChild(fullTextNode);
        }
    }
}