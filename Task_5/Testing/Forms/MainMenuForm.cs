using OpenQA.Selenium;
using Test.Framework.Elements;
namespace Task_5.Forms
{
    public class MainMenuForm
    {
        public Button Download = new Button(By.CssSelector("ul[data-at-selector='priorityMainMenu'] a[data-at-menu='Downloads']"),"Download");
    }
}
