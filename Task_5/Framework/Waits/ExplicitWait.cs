using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Test.Framework.Elements;
using Test.Framework.Logging;
namespace Test.Framework.Waits
{
    public static class ExplicitWait
    {
        private static WebDriverWait webDriverWait = null;
        public static IWebElement ElementExist(By locator, TimeSpan timeSpanExplicitWaitlocator)
        {
            try
            {
                webDriverWait = new WebDriverWait(WebDriver.InstanceWebDriver().GetDriver(), timeSpanExplicitWaitlocator);
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
                webDriverWait = new WebDriverWait(WebDriver.InstanceWebDriver().GetDriver(), timeSpanExplicitWaitlocator);
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
                webDriverWait = new WebDriverWait(WebDriver.InstanceWebDriver().GetDriver(), timeSpanExplicitWaitlocator);
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
                WebDriverWait webDriverWait = new WebDriverWait(WebDriver.InstanceWebDriver().GetDriver(), timeSpanExplicitWaitlocator);
                return webDriverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(element.WebElement, text));
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected error occurred during ExplicitWaitElementIsVisible.");
                return false;
            }
        }
    }
}
