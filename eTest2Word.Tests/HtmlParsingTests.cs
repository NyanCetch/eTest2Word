using System;
using eTest2Word.Core;
using HtmlAgilityPack;
using NUnit.Framework;

namespace eTest2Word.Tests
{
    [TestFixture]
    public class HtmlParsingTests
    {
        [TestCase(
            "<div><p>Hello <span>this <span>fucking</span></span> World </p><p>My Friends</p></div", 
            ExpectedResult = "Hello this fucking World My Friends",
            TestName = "Вложенные элементы")]
        [TestCase(
            "<div><span><i>Кернинг&nbsp;</i>характеризует величину межсимвольного пробела в ...</span><p><br></p></div>", 
            ExpectedResult = "Кернинг&nbsp;характеризует величину межсимвольного пробела в ...",
            TestName = "Стилизованный текст")]
        [TestCase(
            "<div><p>Проявленная фотопленка служат эталоном <label>Ответ</label><input type='text' value='черно-белого'> цвета.</p></div>",
            ExpectedResult = "Проявленная фотопленка служат эталоном черно-белого цвета.",
            TestName = "Поле ввода внутри текста")]
        public string SimplifyNodeTest(string htmlText)
        {
            var parser = new SimpleParser();
            var node = HtmlNode.CreateNode(htmlText);
            var resultNode = parser.SimplifyBlock(node);

            return resultNode.InnerText;
        }
    }
}