using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Polly.Retry;
using OpenQA.Selenium;
using Task_5.Models;
using Task_5.DialogWindows;
using Test.Framework;
using Test.Framework.Waits;
using Test.Framework.Configuration;
using Test.Framework.Elements;
using Test.Framework.Logging;
namespace Task_5.PageObjects
{
    public class DownloadPage
    {       
        List<Button> operatingSystemButtons = new List<Button>();        
        List<Product> products = new List<Product>();
        public Button CarouselIndicator = new Button(By.CssSelector("button[data-at-selector='carouselIndicator']"), "Carousel Indicator");
        public SendingDownloadLinkDialogBox SendingDownloadLinkDialogBox = new SendingDownloadLinkDialogBox();
        public Button SendingLink = new Button(By.CssSelector("button[data-at-selector='appInfoSendToEmail']"), "Send link to Email.");
        public Button CarouselRigt = new Button(By.CssSelector("div[data-at-selector='nextPageArrow']"), "Carousel rigt");
        
        public Product GetProduct(Product testingProduct)
        {
            Product product = new Product();
            try 
            { 
                products = WebDriver.FindElements<Product>(By.CssSelector("div[data-at-selector='downloadApplicationCard']"));
                product = products.Where(product => 
                {
                    Label label = product.FindInsideElement<Label>(By.CssSelector("div.u-productLogotype__name[data-at-selector='serviceName']"));
                    if (String.IsNullOrEmpty(label.Text))
                        ExplicitWait.TextToBePresentInElement<Label>(label, testingProduct.Name, TimeSpan.FromSeconds(ConfigurationManager.Configuration.GetIntParam("waits:explicitWait:TextToBePresentInElement:slider")));                   
                    return label.Text.ToLower().Trim().Equals(testingProduct.Name.Trim().ToLower());
                })
                    .FirstOrDefault();
                if (product == null)
                    throw new NullReferenceException($"Element with text \"{testingProduct.Name}\" has not found.");               
                return product;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during getting product.");
                return product;
            }
        }

        public Button GetSendingLink(Product product) 
        {            
            SendingLink = product.FindInsideElement<Button>(By.CssSelector("button[data-at-selector='appInfoSendToEmail']"));
            return SendingLink;
        }

        public void ClickButtonSendEmail(Product testingProduct)
        {
            try {                
                operatingSystemButtons = WebDriver.FindElements<Button>(By.CssSelector("div.u-osTile.js-osFilter.ng-star-inserted")); ;
                Button operatingSystemButton = operatingSystemButtons.Where(operatingSystem => operatingSystem.Text.ToLower().Trim().Equals(testingProduct.OperatingSystem.Trim().ToLower())).FirstOrDefault();
                Log.Info($"Click \"{testingProduct.OperatingSystem}\" button in \"Operating System menu\".");
                operatingSystemButton.Click();
                Product product = GetProduct(testingProduct);
                Button sendingLink = GetSendingLink(product);
                Point pointSendingLink = sendingLink.Location;
                string scrollToSendingLink = $"window.scrollBy({pointSendingLink.X},{pointSendingLink.Y})";
                Log.Info($"Scroll to \"{sendingLink.Name}\", locator {sendingLink.Locator} which has poin X={pointSendingLink.X} and Y={pointSendingLink.Y}.");
                WebDriver.ExecuteScript(scrollToSendingLink);
                Log.Info($"Click \"{testingProduct.Name}\" button in the products list.");
                sendingLink.Click();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during clikc button \"Send email\".");                
            }
        }

        public (bool Result, string ExpectedValue) CheckSendingEmail(User user)
        {
            string expectedValue = String.Empty;
            try
            {  
                SendingDownloadLinkDialogBox.Email.GetElement();
                expectedValue = SendingDownloadLinkDialogBox
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
            catch (Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during checking emails on the sending email dialog box. Cheking email was {user.Login}.");
                return (false, expectedValue);
            }
        }

        public DateTime ClickButtonSendLink()
        {            
            SendingDownloadLinkDialogBox.SendButton.GetElement();
            RetryPolicy retryPolicy = PollyWait.WaitAndRetryNullReferenceException(ConfigurationManager.Configuration.GetIntParam("waits:polly:captchaWeiting:numberWeiting"), ConfigurationManager.Configuration.GetIntParam("waits:polly:captchaWeiting:time"));
            retryPolicy.Execute(() =>
            {
                Button temp = ExplicitWait.ElementToBeClickable<Button>(SendingDownloadLinkDialogBox.SendButton, TimeSpan.FromMilliseconds(1000));
                if (temp == null)
                    throw new NullReferenceException();
            });
            Log.Info($"Click to button {SendingDownloadLinkDialogBox.SendButton.Name} on dialog box.");
            DateTime sentDateTime = DateTime.Now;
            SendingDownloadLinkDialogBox.SendButton.Click();
            return sentDateTime;
        }
    }
}
