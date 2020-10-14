using OpenQA.Selenium;
using Test.Framework.Elements;
namespace Task_5.DialogWindows
{
    public class SendingDownloadLinkDialogBox
    {
        public Button SendButton = new Button(By.CssSelector("button[data-at-selector='installerSendSelfBtn']"), "Send");
        public Input Email = new Input(By.CssSelector("input[data-at-selector='emailInput']"), "Email");
    }
}
