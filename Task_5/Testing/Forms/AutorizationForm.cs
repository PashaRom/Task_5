using OpenQA.Selenium;
using Test.Framework.Elements;
namespace Task_5.Forms
{
    public class AutorizationForm {       
        public Input Login = new Input(By.CssSelector("form[data-at-selector='signInContent'] input[name='email']"),"Login");
        public Input Password = new Input(By.CssSelector("form[data-at-selector='signInContent'] input[name='password']"),"Password");
        public Button Signin = new Button(By.CssSelector("form[data-at-selector='signInContent'] button[type='submit']"), "Sign in");
    }
}
