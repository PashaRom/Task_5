using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using OpenQA.Selenium;
using Test.Framework.Configuration;
using Test.Framework.Logging;
using Test.Framework.Elements;
namespace Test.Framework
{
    public class WebDriver
    {
        private static IWebDriver webDriver;                    
        public WebDriver()
        {           
            try 
            {                
                Log.Info("Create web driver.");
                string activeBrowserName = ConfigurationManager.Configuration.GetStringParam("activeBrowser");
                if(activeBrowserName.ToLower().Trim().Equals(NameOfBrowser.Chrome.ToString().ToLower()))
                    webDriver = new CreatorWebDriver().CreateDriver(NameOfBrowser.Chrome);
                else if(activeBrowserName.ToLower().Trim().Equals(NameOfBrowser.Firefox.ToString().ToLower()))
                    webDriver = new CreatorWebDriver().CreateDriver(NameOfBrowser.Firefox);                
            }
            catch (Exception ex) 
            {
                Log.Fatal(ex, "The WebDriver has not been created.");
            }
        }
        public static string Url
        {
            get
            {                
                return webDriver.Url;
            }
            set
            {
                Log.Info($"Set url - {value}");
                webDriver.Url = value;
            }
        }
        public static IWebElement FindElement(By locator) 
        {
            Log.Info($"Find element {locator.ToString()}");
            try 
            { 
                return webDriver.FindElement(locator);
            }
            catch(NoSuchElementException ex)
            {
                Log.Error(ex, $"No element matches locator {locator.ToString()}");
                return null;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Error occurred during finding element {locator.ToString()}");
                return null;
            }
        }
        public static ReadOnlyCollection<IWebElement> FindElements(By locator)
        {
            return webDriver.FindElements(locator);
        }
        public static List<T> FindElements<T>(By locator) 
            where T : class, IElement, new()            
        {
            List<T> elements = new List<T>();
            ReadOnlyCollection<IWebElement> webElements = webDriver.FindElements(locator);
            foreach (IWebElement element in webElements)
            {
                elements.Add(new T() { WebElement = element });
            }
            return elements;
        }        
        public static void ExecuteScript(string javaScript) 
        {
            Log.Info($"Execute java script \"{javaScript}\"");
            IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;
            js.ExecuteScript(javaScript);
        }
        public static IWebDriver GetWebDriver() 
        {
            return webDriver;
        }
        public static IOptions Manage()
        {
            return webDriver.Manage();
        }
        public static INavigation Navigate()
        {
            return webDriver.Navigate();
        }
        public static ITargetLocator SwitchTo()
        {
            return webDriver.SwitchTo();
        }
        public static string Title {
            get
            {
                return webDriver.Title;
            } 
        }
        public static string PageSource {
            get
            {
                return webDriver.PageSource;
            } 
        }
        public static string CurrentWindowHandle {
            get
            {
                return webDriver.CurrentWindowHandle;
            }
        }
        public static ReadOnlyCollection<string> WindowHandles {
            get
            {
                return webDriver.WindowHandles;
            }
        }
        public void Quit() 
        {
            try { 
                webDriver.Quit();
                Log.Info($"{webDriver.GetType()} was quit.");
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Unexpected error occurred during quiting driver.");
            }
        }
        public void Close() 
        {
            webDriver.Close();
            Log.Info($"{webDriver.GetType()} was closed.");
        }
    }
}
