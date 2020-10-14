using System;
using System.IO;
using Test.Framework.Logging;
using OpenQA.Selenium;
namespace Test.Framework
{
    public static class Extension
    {
        public static string RemoveXPath(this By locator)
        {
            string partOfRemove = "By.XPath: ";
            return locator.ToString().Remove(0, partOfRemove.Length - 1);
        }

        public static string GetFullPathDirectory(this string path)
        {
            string pathDownload = null;
            try
            {
                if (path == null)
                {
                    pathDownload = Directory.GetCurrentDirectory();
                    Log.Info($"The directory for download files has been set {pathDownload}");
                    return pathDownload;
                }
                pathDownload = Path.GetFullPath(path);
                Log.Info($"The directory for download files has been set {pathDownload}");
                return pathDownload;
            }            
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Unexpected error occurred during to get path download {path}.");
                return pathDownload;
            }
        }
    }
}
