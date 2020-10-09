using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Task_5.Models;
using Task_5.DialogWindows;
using Test.Framework;
using Test.Framework.Configuration;
using Test.Framework.Elements;
using Test.Framework.Logging;

namespace Task_5.PageObjects
{
    public class DownloadPage
    {       
        List<Button> OsList = new List<Button>();
        public Button CarouselIndicator = new Button(By.CssSelector("button[data-at-selector='carouselIndicator']"), "Carousel Indicator");
        public SendingDownloadLinkDialogBox SendingDownloadLinkDialogBox = new SendingDownloadLinkDialogBox();
        public Button SendingLink = new Button(By.CssSelector("button[data-at-selector='appInfoSendToEmail']"), "Send link to Email.");
        public Button CarouselRigt = new Button(By.CssSelector("div[data-at-selector='nextPageArrow']"), "Carousel rigt");
        public List<Button> GetOsList()
        {
            try
            {
                OsList = WebDriver.FindElements<Button>(By.CssSelector("div.u-osTile.js-osFilter.ng-star-inserted"));
                return OsList;
            }
            catch(Exception ex)
            {
                Log.Error(ex,$"Unexpected error occurred during getting Os list.");
                return OsList;
            }
        }
        public Product GetProduct(Product testingProduct)
        {
            Product product = new Product();
            try 
            { 
                List<Product> products = WebDriver.FindElements<Product>(By.CssSelector("div[data-at-selector='downloadApplicationCard']"));
                product = products.Where(p => 
                {
                    Label label = p.FindInsideElement<Label>(By.CssSelector("div.u-productLogotype__name[data-at-selector='serviceName']"));
                    string s = label.Text;
                    if (String.IsNullOrEmpty(label.Text))
                        Waits.ExplicitWait.TextToBePresentInElement<Label>(label, testingProduct.Name, TimeSpan.FromSeconds(ConfigurationManager.Configuration.GetIntParam("waits:explicitWait:TextToBePresentInElement:slider")));                   
                    return label.Text.ToLower().Trim().Equals(testingProduct.Name.Trim().ToLower());
                })
                    .FirstOrDefault();
                if (product == null)
                    throw new NullReferenceException($"Element with text \"{testingProduct.Name}\" has not found.");               
                return product;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"Unexpected error occurred during getting Product.");
                return product;
            }

        }
        public Button GetSendingLink(Product product) 
        {            
            SendingLink = product.FindInsideElement<Button>(By.CssSelector("button[data-at-selector='appInfoSendToEmail']"));
            return SendingLink;
        }
    }
}
