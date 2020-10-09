using Test.Framework;
using Task_5.Forms;
namespace Task_5.PageObjects
{
    public class HomePage
    {
        public AutorizationForm Autorization = new AutorizationForm();        
        public void Load(string url)
        {
            WebDriver.Url = url;
        }        
    }
}
