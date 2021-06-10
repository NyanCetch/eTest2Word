using System;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eCourseTest2Word.Tests
{
    public static class Helpers
    {
        public const string DataDir = "TestsData";
        public const string DataSuffix = "_Data";
        public const string ResultSuffux = "_Result";
        public const string FileExtension = ".txt";

        public static HtmlNode TextToHtmlNode(string text)
        {
            return HtmlNode.CreateNode(text);
        }

        private static string GetFileName(string name, string suffix)
        {
            return name + suffix + FileExtension;
        }

        private static string GetTextFromFile(string fileName)
        {
            var files = Directory.GetFiles(DataDir, fileName, SearchOption.AllDirectories);
            var fileText = File.ReadAllText(files[0]);
            return fileText;
        }

        public static HtmlNode GetDataFromFile(string dataName)
        {
            var dataFileName = GetFileName(dataName, DataSuffix);
            var dataText = GetTextFromFile(dataFileName);
            var html = TextToHtmlNode(dataText);
            return html;
        }

        private static string ResultToText(object obj)
        {
            var objJsonText = JsonConvert.SerializeObject(obj);
            return objJsonText;
        }

        public static bool IsResultCorrect(object result, string expectedResultName, bool partialCheck = false)
        {
            var resultJsonText = ResultToText(result);
            var expectedFileName = GetFileName(expectedResultName, ResultSuffux);
            var expectedText = GetTextFromFile(expectedFileName);

            var resultJObj = JObject.Parse(resultJsonText);
            var expectedJObj = JObject.Parse(expectedText);
            
            Console.WriteLine("Expected:\n\n" + expectedText);
            Console.WriteLine();
            Console.WriteLine("Result:\n\n" + resultJsonText);

            if (partialCheck)
                return IsPartialEquals(expectedJObj, resultJObj);
            
            return JToken.DeepEquals(resultJObj, expectedJObj);
        }

        /// <summary>
        /// Для неполного сравнения json-объектов (в случае, когда необходимо опустить проверку части полей)
        /// </summary>
        /// <param name="jObj1"></param>
        /// <param name="jObj2"></param>
        /// <returns></returns>
        private static bool IsPartialEquals(JObject jObj1, JObject jObj2)
        {
            var propNames1 = jObj1.Properties().Select(p => p.Name);
            var propNames2 = jObj2.Properties().Select(p => p.Name);
            var intersectProps = propNames1.Intersect(propNames2);
            
            foreach (var propName in intersectProps)
            {
                var prop1 = jObj1[propName];
                var prop2 = jObj2[propName];

                if (!JToken.DeepEquals(prop1, prop2))
                    return false;
            }

            return true;
        }

        public static bool IsCheckPartiallyCorrect(string testName, Func<HtmlNode, object> handler)
        {
            var html = GetDataFromFile(testName);
            var result = handler(html);
            return IsResultCorrect(result, testName, true);
        }

        public static bool IsCheckFullCorrect(string testName, Func<HtmlNode, object> handler)
        {
            var html = GetDataFromFile(testName);
            var result = handler(html);
            return IsResultCorrect(result, testName);
        }
    }
}