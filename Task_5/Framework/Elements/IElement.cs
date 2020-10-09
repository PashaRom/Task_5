using OpenQA.Selenium;

namespace Test.Framework.Elements
{
    public interface IElement
    {
        public IWebElement WebElement{ get; set; }
    }
}
