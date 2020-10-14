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

        private static WebDriver instanceWebDriver = null;
        private static IWebDriver driver;
        
        private WebDriver()
        {            
            try
            {                
                 Log.Info("Create web driver.");
                 string activeBrowserName = ConfigurationManager.Configuration.GetStringParam("activeBrowser");
                 if (activeBrowserName.ToLower().Trim().Equals(NameOfBrowser.Chrome.ToString().ToLower()))
                    driver = new CreatorWebDriver().CreateDriver(NameOfBrowser.Chrome);
                 else if (activeBrowserName.ToLower().Trim().Equals(NameOfBrowser.Firefox.ToString().ToLower()))
                    driver = new CreatorWebDriver().CreateDriver(NameOfBrowser.Firefox);               
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The WebDriver has not been created.");
            }
        }
        
        public static WebDriver InstanceWebDriver()
        {
            if (instanceWebDriver == null) {                
                instanceWebDriver = new WebDriver();
            }
            return instanceWebDriver;
        }

        public static string Url
        {
            get
            {                
                return driver.Url;
            }
            set
            {
                Log.Info($"Set url - {value}");
                driver.Url = value;
            }
        }

        public static IWebElement FindElement(By locator) 
        {
            Log.Info($"Find element {locator.ToString()}");
            try 
            { 
                return driver.FindElement(locator);
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
            return driver.FindElements(locator);
        }

        public static List<T> FindElements<T>(By locator) 
            where T : class, IElement, new()            
        {
            List<T> elements = new List<T>();
            ReadOnlyCollection<IWebElement> webElements = driver.FindElements(locator);
            foreach (IWebElement element in webElements)
            {
                elements.Add(new T() { WebElement = element });
            }
            return elements;
        }  
        
        public static void ExecuteScript(string javaScript) 
        {
            Log.Info($"Execute java script \"{javaScript}\"");
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(javaScript);
        }

        public IWebDriver GetDriver() 
        {
            return driver;
        }

        public static IOptions Manage()
        {
            return driver.Manage();
        }

        public static INavigation Navigate()
        {
            return driver.Navigate();
        }

        public static ITargetLocator SwitchTo()
        {
            return driver.SwitchTo();
        }

        public static string Title {
            get
            {
                return driver.Title;
            } 
        }

        public static string PageSource {
            get
            {
                return driver.PageSource;
            } 
        }

        public static string CurrentWindowHandle {
            get
            {
                return driver.CurrentWindowHandle;
            }
        }

        public static ReadOnlyCollection<string> WindowHandles {
            get
            {
                return driver.WindowHandles;
            }
        }

        public void Quit() 
        {
            try { 
                driver.Quit();
                Log.Info($"{driver.GetType()} was quit.");
                instanceWebDriver = null;
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Unexpected error occurred during quiting driver.");
            }
        }

        public void Close() 
        {
            driver.Close();
            Log.Info($"{driver.GetType()} was closed.");
        }
    }
}
