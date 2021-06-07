using System;
using System.IO;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eTest2Word.Tests
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
            var projectDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..");
            var filesDirPath = Path.Combine(projectDirPath, DataDir);
            var files = Directory.GetFiles(filesDirPath, fileName, SearchOption.AllDirectories);
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

        public static bool IsResultCorrect(object result, string expectedResultName)
        {
            var resultJsonText = ResultToText(result);
            var expectedResultFileName = GetFileName(expectedResultName, ResultSuffux);
            var expectedResultText = GetTextFromFile(expectedResultFileName);

            var resultToken = JToken.Parse(resultJsonText);
            var expectedResultToken = JToken.Parse(expectedResultText);
            
            return JToken.DeepEquals(resultToken, expectedResultToken);
        }

        public static bool TryCheck(string testName, Func<HtmlNode, object> handler)
        {
            var html = GetDataFromFile(testName);
            var result = handler(html);
            return IsResultCorrect(result, testName);
        }
    }
}