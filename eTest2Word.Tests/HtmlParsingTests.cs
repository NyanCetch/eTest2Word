using eTest2Word.Core;
using HtmlAgilityPack;
using NUnit.Framework;

namespace eTest2Word.Tests
{
    [TestFixture]
    public class HtmlParsingTests
    {
        private SimpleParser _parser;
        
        [SetUp]
        public void Setup()
        {
            _parser = new SimpleParser();
        }
        
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
        [TestCase(
            "<p><img src=''><br></p>",
            ExpectedResult = "<img src=''>",
            TestName = "Изображение")]
        public string SimplifyNodeTest(string htmlText)
        {
            var node = HtmlNode.CreateNode(htmlText);
            var resultNode = _parser.SimplifyBlock(node);

            return resultNode.WriteTo();
        }

        [TestCase(
            "<div><p>Lorem ipsum dolor</p><p><img src=''><br></p><p><span>sit amet, consectetur</span><br></p></div>", 
            ExpectedResult = "<div><p>Lorem ipsum dolor</p><p><img src=''></p><p>sit amet, consectetur</p></div>",
            TestName = "Вопрос с изображением и стилизованным текстом")]
        public string SimplifyQuestionNodeTest(string htmlText)
        {
            var node = HtmlNode.CreateNode(htmlText);
            _parser.SimplifyQuestionBlock(node);

            return node.WriteTo();
        }
    }
}