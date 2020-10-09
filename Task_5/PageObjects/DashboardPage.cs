using OpenQA.Selenium;
using Test.Framework.Elements;
using Task_5.Forms;
namespace Task_5.PageObjects
{
    public class DashboardPage
    {
        public Label Login = new Label(By.CssSelector("ul.menu-utility span.menu-item__text.a-direction-ltr"), "Login");
        public MainMenuForm MainMenuForm = new MainMenuForm();
    }
}
