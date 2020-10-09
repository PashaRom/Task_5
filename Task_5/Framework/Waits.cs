using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Test.Framework.Elements;
using Test.Framework.Logging;
using Polly;
using Polly.Retry;

namespace Test.Framework
{
    public static class Waits
    {
        public static T Fluent<T> (By locator, TimeSpan timeOut, TimeSpan pollingInterval)
            where T : BaseElement, new()
        {
            try
            {
                DefaultWait<IWebDriver> fluentWait = new DefaultWait<IWebDriver>(WebDriver.GetWebDriver());
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
        public static class ExplicitWait
        {
            private static WebDriverWait webDriverWait = null;
            public static IWebElement ElementExist(By locator, TimeSpan timeSpanExplicitWaitlocator)
            {                
                try
                {
                    webDriverWait = new WebDriverWait(WebDriver.GetWebDriver(), timeSpanExplicitWaitlocator);
                    IWebElement webElement = webDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
                    return webElement;
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Unexpected error occurred during ExplicitWait.");
                    return null;
                }
            }
            public static T ElementToBeClickable<T>(BaseElement baseElement, TimeSpan timeSpanExplicitWaitlocator)
                where T : BaseElement, new()
            {                
                try
                {
                    webDriverWait = new WebDriverWait(WebDriver.GetWebDriver(), timeSpanExplicitWaitlocator);
                    T element = new T();
                    element.WebElement = webDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(baseElement.WebElement));
                    return element;
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Unexpected error occurred during ExplicitWaitToBeClickable.");
                    return null;
                }
            }
            public static IWebElement ElementIsVisible(By locator, TimeSpan timeSpanExplicitWaitlocator)
            {                
                try
                {
                    webDriverWait = new WebDriverWait(WebDriver.GetWebDriver(), timeSpanExplicitWaitlocator);
                    IWebElement webElement = webDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
                    return webElement;
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Unexpected error occurred during ExplicitWaitElementIsVisible.");
                    return null;
                }
            }
            public static bool TextToBePresentInElement<T>(T element, string text, TimeSpan timeSpanExplicitWaitlocator)
                where T : BaseElement, new()
            {               
                try
                {
                    WebDriverWait webDriverWait = new WebDriverWait(WebDriver.GetWebDriver(), timeSpanExplicitWaitlocator);
                    return webDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(element.WebElement, text));                    
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Unexpected error occurred during ExplicitWaitElementIsVisible.");
                    return false;
                }
            }
        }
        public static class Polly
        {
            public static RetryPolicy WaitAndRetryNullReferenceException(int numberWaiting, int time)
            {
                try
                {
                    RetryPolicy policy = Policy
                    .Handle<NullReferenceException>()
                    .WaitAndRetry(numberWaiting, retryAttempt => TimeSpan.FromSeconds(time));
                    return policy;
                }
                catch(Exception ex)
                {
                    Log.Fatal(ex, "Unexpected error occurred during ExplicitWaitElementIsVisible.");
                    throw new NullReferenceException();
                }
                
            }
        }
    }
}
