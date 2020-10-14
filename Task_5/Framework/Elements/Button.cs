using System;
using OpenQA.Selenium;
using Test.Framework.Logging;
namespace Test.Framework.Elements
{
    public class Button : BaseElement
    {
        public Button():base() { }
        public Button(By by, string name) : base(by, name) { }
        
        public void Submit()
        {
            Log.Info($"Submit to the target element {Name} which has locator {Locator}.");
            try
            {
                base.WebElement.Submit();
            }           
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during Submit to the target element {Name} which has locator {Locator}");
            }
        }
    }
}
