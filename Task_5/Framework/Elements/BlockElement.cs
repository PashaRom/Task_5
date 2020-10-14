using System;
using OpenQA.Selenium;
using Test.Framework.Logging;
namespace Test.Framework.Elements
{
    public abstract class BlockElement : IElement
    {
        private IWebElement blockElement;        
        public IWebElement WebElement
        {
            get
            {
                return this.blockElement;
            }
            set
            {
                this.blockElement = value;
            }
        }

        public T FindInsideElement<T>(By insideElementLocator)
            where T : BaseElement, new()
        {
            Log.Info($"Find the target elements {insideElementLocator} inside the current element {blockElement.TagName} which has class {blockElement.GetProperty("class")} is searched.");
            try
            {
                T baseElement = new T();                
                baseElement.WebElement = blockElement.FindElement(insideElementLocator);
                return baseElement;
            }            
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during finding elements {insideElementLocator} inside the current element {blockElement.TagName} which has class {blockElement.GetProperty("class")}.");
                return null;
            }
        }        
    }
}
