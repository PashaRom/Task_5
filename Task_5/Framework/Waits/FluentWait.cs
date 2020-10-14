using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Test.Framework.Elements;
using Test.Framework.Logging;
namespace Test.Framework.Waits
{
    public static class FluentWait
    {
        public static T Create<T> (By locator, TimeSpan timeOut, TimeSpan pollingInterval)
            where T : BaseElement, new()
        {
            try
            {
                DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(WebDriver.InstanceWebDriver().GetDriver());
                fluentWait.Timeout = timeOut;
                fluentWait.PollingInterval = pollingInterval;
                fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                fluentWait.Message = "Element to be searched not found.";
                T element = new T();
                element.WebElement = fluentWait.Until(x => x.FindElement(locator));
                return element;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "$Unexpected error occurred during creating fluent wait.");
                return null;
            }
        }                
    }
}
