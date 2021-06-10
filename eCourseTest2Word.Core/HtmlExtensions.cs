using System;
using HtmlAgilityPack;

namespace eCourseTest2Word.Core
{
    public static class HtmlExtensions
    {
        public static void ToConsole(this HtmlNode node) => Console.WriteLine(node.WriteTo());
    }
}