using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using Task_5.Models;
using Task_5.Utilits;
using Task_5.Steps;
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
                return ExcelData.GetData($"{Directory.GetCurrentDirectory()}\\{ ConfigurationManager.Configuration.GetStringParam("testDataFile")}");
            }
        }        
        [SetUp]
        public void Setup()
        {            
            InstanceWebDriver instanceWebDriver = InstanceWebDriver.GetInstanceWebDriver();
            WebDriver webDriver = instanceWebDriver.GetDriver();
        }

        [Test]
        [TestCaseSource(nameof(TestItems))]
        public void CheckDownloadUrl(TestItem testItem)
        {
            StepsHomePage.Autorization(testItem);
            Assert.IsTrue(StepsDashboardPage.IsOpenDashboardPage(),
                "The Dashboard page has not been opened. Might be autorization has been failed.");
            StepsDashboardPage.ClickMainMenuDownload();
            StepsDownloadPage.ClickButtonSendEmail(testItem.Product);
            (bool Result, string ExpectedValue) chekedEmail = StepsDownloadPage.CheckSendingEmail(testItem.User);
            Assert.IsTrue(chekedEmail.Result,
                $"Actual email has value \"{testItem.User.Login}\". Getting expected email has got value \"${chekedEmail.ExpectedValue}\".");
            DateTime sentEmail  = StepsDownloadPage.ClickButtonSendLink();
            Log.Info(12, $"Get the download url string for \"{testItem.Product.OsName} - {testItem.Product.Name}\"");
            string emailHtmlBody = Email.GetContentEmail(sentEmail,ConfigurationManager.TestData.GetStringParam("sendigEmail"));
            string downloadLink = Html.GetAttribute(emailHtmlBody, "a.green-button.text-transform_none.at-GetDownloadLink", "href");
            Log.Info($"Assert download link. Expected data is {ConfigurationManager.TestData.GetStringParam("containDowloadLink")}, actual data is {downloadLink}");
            Assert.IsTrue(downloadLink.Contains(ConfigurationManager.TestData.GetStringParam("containDowloadLink")),
                $"The download link \"{downloadLink}\" do not contain \"{ConfigurationManager.TestData.GetStringParam("containDowloadLink")}\"");           
        }
        [TearDown]
        public void TearDown()
        {
            InstanceWebDriver instanceWebDriver = InstanceWebDriver.GetInstanceWebDriver();
            WebDriver webDriver = instanceWebDriver.GetDriver();            
            webDriver.Quit();
            InstanceWebDriver.QuitInstanceWebDriver();
        }
    }
}