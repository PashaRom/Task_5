using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using Task_5.Models;
using Task_5.PageObjects;
using Task_5.Utilits;
using Test.Framework;
using Test.Framework.Logging;
using Test.Framework.Configuration;
namespace Task_5.Test
{
    [TestFixture]
    public class KasperskyTests
    {
        public static IEnumerable<TestItem> TestItems
        {
            get
            {               
                return ExcelUtil.GetData($"{Directory.GetCurrentDirectory()}\\{ ConfigurationManager.Configuration.GetStringParam("testDataFile")}");
            }
        }
        
        [SetUp]
        public void Setup()
        {           
            WebDriver webDriver = WebDriver.InstanceWebDriver();
        }

        [Test]
        [TestCaseSource(nameof(TestItems))]
        public void CheckDownloadUrl(TestItem testItem)
        {
            string url = ConfigurationManager.Configuration.GetStringParam("url");
            Log.Info(1, $"Go to home page {url}.");
            HomePage homePage = new HomePage();
            homePage.Load(url);
            Log.Info(2, $"Autorization.");
            homePage.Autorization(testItem.User.Login,testItem.User.Password);
            Log.Info(3, "Checking Dashboard Page is opened.");
            DashboardPage dashboardPage = new DashboardPage();
            Log.Info(4, "Checking Dashboard Page is opened.");            
            Assert.IsTrue(dashboardPage.IsOpenDashboardPage(),
                "The Dashboard page has not been opened. Might be autorization has been failed.");
            Log.Info(5, "Click \"Download\" button in \"Main menu\".");
            dashboardPage.ClickMainMenuDownload();
            Log.Info(6, $"Click \"Send email\" button in \"Operating System menu\".");
            DownloadPage downloadPage = new DownloadPage();
            downloadPage.ClickButtonSendEmail(testItem.Product);
            Log.Info(7, $"Email \"{testItem.User.Login}\" is checking on dialog box.");
            (bool Result, string ExpectedValue) chekedEmail = downloadPage.CheckSendingEmail(testItem.User);
            Assert.IsTrue(chekedEmail.Result,
                $"Actual email has value \"{testItem.User.Login}\". Getting expected email has got value \"${chekedEmail.ExpectedValue}\".");
            Log.Info(8, $"Send email from dialogbox.");
            DateTime sentEmail  = downloadPage.ClickButtonSendLink();
            Log.Info(9, $"Get the download url string for \"{testItem.Product.OperatingSystem} - {testItem.Product.Name}\"");
            string emailHtmlBody = EmailUtil.GetContentEmail(sentEmail,ConfigurationManager.TestData.GetStringParam("sendigEmail"));
            string downloadLink = HtmlUtil.GetAttribute(emailHtmlBody, "a.green-button.text-transform_none.at-GetDownloadLink", "href");
            Log.Info($"Assert download link. Expected data is {ConfigurationManager.TestData.GetStringParam("containDowloadLink")}, actual data is {downloadLink}");
            Assert.IsTrue(downloadLink.Contains(ConfigurationManager.TestData.GetStringParam("containDowloadLink")),
                $"The download link \"{downloadLink}\" do not contain \"{ConfigurationManager.TestData.GetStringParam("containDowloadLink")}\"");           
        }

        [TearDown]
        public void TearDown()
        {            
            WebDriver.InstanceWebDriver().Quit();
        }
    }
}