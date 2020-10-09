using Test.Framework.Logging;
using Test.Framework.Configuration;
using Task_5.Models;
using Task_5.PageObjects;
namespace Task_5.Steps
{
    class StepsHomePage
    {
        public static void Autorization(TestItem testItem)
        {
            string url = ConfigurationManager.Configuration.GetStringParam("url");
            Log.Info(1, $"Go to home page {url}.");
            HomePage homePage = new HomePage();
            homePage.Load(url);
            Log.Info($"Autorization.");
            homePage.Autorization.Login.GetElement();
            homePage.Autorization.Password.GetElement();
            homePage.Autorization.Signin.GetElement();
            Log.Info(2, $"Send key {homePage.Autorization.Login.Name}: {testItem.User.Login}.");
            homePage.Autorization.Login.SendKeys(testItem.User.Login);
            Log.Info(3, $"Send key {homePage.Autorization.Password.Name}: {testItem.User.Password}.");
            homePage.Autorization.Password.SendKeys(testItem.User.Password);
            Log.Info(4, $"Submit button {homePage.Autorization.Password.Name}: {testItem.User.Password}.");
            homePage.Autorization.Signin.Submit();
        }
    }
}
