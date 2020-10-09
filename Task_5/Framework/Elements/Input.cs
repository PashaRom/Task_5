using System;
using Test.Framework.Logging;
using OpenQA.Selenium;
namespace Test.Framework.Elements
{
    public class Input : BaseElement
    {       
        public Input(By locator, string name) : base(locator, name) { }
        
        public void SendKeys(string text)
        {
            Log.Info($"Send key \"{text}\" to the target element {Name} which has locator {Locator}.");
            try
            {
                base.WebElement.SendKeys(text);
            }
            catch (ElementNotVisibleException ex)
            {
                Log.Error(ex, $"Error occurred during SendKey. The target element {Name} which has locator {Locator} is not visible.");
            }
            catch (InvalidElementStateException ex)
            {
                Log.Error(ex, $"Error occurred during SendKey. The target element {Name} which has locator {Locator} is not enabled.");
            }
            catch (StaleElementReferenceException ex)
            {
                Log.Error(ex, $"Error occurred during SendKey. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
            }
            catch (NullReferenceException ex)
            {
                Log.Error(ex, $"The error occurred during SendKey.The target element {Name} which has locator {Locator} has null value");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during SendKey to the target element {Name} which has locator {Locator}");
            }
        }
    }    
}
