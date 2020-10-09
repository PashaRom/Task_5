using System;
using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Test.Framework.Logging;

namespace Task_5.Utilits
{
    public static class Html
    {
        public static string GetAttribute(string html, string cssSelector, string attribute)
        {
            string valueAttribute = String.Empty;
            try 
            {                
                IHtmlDocument angle = new HtmlParser().ParseDocument(html);
                var element = angle.QuerySelector(cssSelector);
                valueAttribute = element.GetAttribute(attribute);
                return valueAttribute;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during GetAttribute \"{attribute}\" from the element which has CSS selector \"{cssSelector}\".");
                return valueAttribute;
            }
        }
    }
}
