﻿using OpenQA.Selenium;
namespace Test.Framework.Elements
{
    public class Label : BaseElement
    {
        public Label():base(){ }
        public Label(By locator, string name) : base(locator, name) { }
    }
}
