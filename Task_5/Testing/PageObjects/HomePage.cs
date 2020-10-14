using Test.Framework;
using Task_5.Forms;
using Test.Framework.Logging;
namespace Task_5.PageObjects
{
    public class HomePage
    {
        public AutorizationForm AutorizationForm = new AutorizationForm();
        
        public void Load(string url)
        {
            WebDriver.Url = url;
        }

        public void Autorization(string login, string password)
        {
            AutorizationForm.Login.GetElement();
            AutorizationForm.Password.GetElement();
            AutorizationForm.Signin.GetElement();
            Log.Info($"Send key {AutorizationForm.Login.Name}: {login}.");
            AutorizationForm.Login.SendKeys(login);
            Log.Info(3, $"Send key {AutorizationForm.Password.Name}: {password}.");
            AutorizationForm.Password.SendKeys(password);
            Log.Info(4, $"Submit button {AutorizationForm.Password.Name}: {password}.");
            AutorizationForm.Signin.Submit();
        }
    }
}
