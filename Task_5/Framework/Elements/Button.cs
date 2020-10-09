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
            catch (StaleElementReferenceException ex)
            {
                Log.Error(ex, $"Error occurred during Submit. The target element {Name} which has locator {Locator} is no longer valid in the document DOM.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during Submit to the target element {Name} which has locator {Locator}");
            }
        }
    }
}
