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
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during SendKey to the target element {Name} which has locator {Locator}");
            }
        }
    }    
}
