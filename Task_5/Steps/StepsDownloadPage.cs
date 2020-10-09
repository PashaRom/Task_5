using System;
using System.Linq;
using System.Collections.Generic;
using Task_5.Models;
using Task_5.PageObjects;
using Test.Framework;
using Test.Framework.Elements;
using Test.Framework.Logging;
using Test.Framework.Configuration;
using System.Drawing;
using Polly;
using Polly.Retry;
namespace Task_5.Steps
{
    public static class StepsDownloadPage
    {
        public static void ClickButtonSendEmail(Product testingProduct)
        {            
            DownloadPage downloadPage = new DownloadPage();
            List<Button> osButtons = downloadPage.GetOsList();
            Button osButton = osButtons.Where(o=>o.Text.ToLower().Trim().Equals(testingProduct.OsName.Trim().ToLower())).FirstOrDefault();
            Log.Info(7, $"Click \"{testingProduct.OsName}\" button in \"Os menu\".");
            osButton.Click();           
            Product product = downloadPage.GetProduct(testingProduct);
            Button sendingLink = downloadPage.GetSendingLink(product);           
            Point pointSendingLink = sendingLink.Location;            
            string scrollToSendingLink = $"window.scrollBy({pointSendingLink.X},{pointSendingLink.Y})";
            Log.Info(8, $"Scroll to \"{sendingLink.Name}\", locator {sendingLink.Locator} which has poin X={pointSendingLink.X} and Y={pointSendingLink.Y}.");
            WebDriver.ExecuteScript(scrollToSendingLink);
            Log.Info(9, $"Click \"{testingProduct.Name}\" button in the products list.");
            sendingLink.Click();
        }
        public static (bool Result, string ExpectedValue) CheckSendingEmail(User user)
        {
            string expectedValue = String.Empty;
            try 
            { 
                Log.Info(10, $"Email \"{user.Login}\" is checking on dialog boox.");
                DownloadPage downloadPage = new DownloadPage();
                downloadPage.SendingDownloadLinkDialogBox.Email.GetElement();
                expectedValue = downloadPage
                    .SendingDownloadLinkDialogBox
                    .Email
                    .GetAttribute("value")
                    .Trim()
                    .ToLower();

                if (expectedValue.Equals(user.Login.ToLower().Trim())) 
                {
                    Log.Info($"Actual email has value \"{user.Login}\". Getting expected email has got value \"${expectedValue}\"");
                    return (true, expectedValue);
                }
                else 
                {
                    Log.Error($"Actual email has value \"{user.Login}\". Getting expected email has got value \"${expectedValue}\".");
                    return (false, expectedValue);
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during checking emails on the sending email dialog box. Cheking email was {user.Login}.");
                return (false, expectedValue);
            }            
        }
        public static DateTime ClickButtonSendLink()
        {
            DownloadPage downloadPage = new DownloadPage();
            downloadPage.SendingDownloadLinkDialogBox.SendButton.GetElement();            
            RetryPolicy retryPolicy = Waits.Polly.WaitAndRetryNullReferenceException(ConfigurationManager.Configuration.GetIntParam("waits:polly:captchaWeiting:numberWeiting"), ConfigurationManager.Configuration.GetIntParam("waits:polly:captchaWeiting:time"));
            retryPolicy.Execute(()=> 
            {
                Button temp = Waits.ExplicitWait.ElementToBeClickable<Button>(downloadPage.SendingDownloadLinkDialogBox.SendButton,TimeSpan.FromMilliseconds(1000));
                if (temp == null)
                    throw new NullReferenceException();                
            });
            Log.Info(11, $"Click to button {downloadPage.SendingDownloadLinkDialogBox.SendButton.Name} on dialog box.");
            DateTime sentDateTime = DateTime.Now;
            downloadPage.SendingDownloadLinkDialogBox.SendButton.Click();
            return sentDateTime;
        }
    }
}
