using OpenQA.Selenium;

namespace Test.Framework.Elements
{
    public class Link : BaseElement
    {
        public Link() : base() { }
        public Link(By locator, string name) : base(locator, name) { }
    }
}
