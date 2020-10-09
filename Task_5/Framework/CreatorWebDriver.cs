using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Test.Framework.Configuration;
using Test.Framework.Logging;
namespace Test.Framework
{
    public class CreatorWebDriver {
         public IWebDriver CreateDriver(NameOfBrowser nameOfBrowser)
        {
            IWebDriver webDriver = null;
            try
            {
                string activeBrowser = nameOfBrowser.ToString().ToLower();
                string pathDownloadDir = ConfigurationManager.Configuration.GetStringParam("pathDownloadFiles").GetFullPathDirectory();
                string pathToDriver = ConfigurationManager.Configuration.GetStringParam($"browsers:{activeBrowser}:pathToDriver");
                bool leaveBrowserRunning = ConfigurationManager.Configuration.GetBooleanParam($"browsers:{activeBrowser}:options:leaveBrowserRunning");
                string startSizeWindow = ConfigurationManager.Configuration.GetStringParam($"browsers:{activeBrowser}:options:startSizeWindow");
                string mode = ConfigurationManager.Configuration.GetStringParam($"browsers:{activeBrowser}:options:mode");
                int implicitWait = ConfigurationManager.Configuration.GetIntParam($"browsers:{activeBrowser}:options:implicitWait");
                int explicitWait = ConfigurationManager.Configuration.GetIntParam($"browsers:{activeBrowser}:options:explicitWait");
                string intlAcceptLanguages = ConfigurationManager.Configuration.GetStringParam($"browsers:{activeBrowser}:options:intl.accept_languages");
                switch (nameOfBrowser)
                {
                    case NameOfBrowser.Chrome:                        
                        ChromeOptions chromeOptions = new ChromeOptions();
                        Log.Info("Initialization ChromeDriver options.");
                        chromeOptions.LeaveBrowserRunning = leaveBrowserRunning;
                        chromeOptions.AddArgument(startSizeWindow);
                        chromeOptions.AddArgument(mode);
                        chromeOptions.AddArgument("safebrowsing-disable-download-protection");
                        chromeOptions.AddUserProfilePreference("safebrowsing", "enabled");
                        chromeOptions.AddUserProfilePreference("download.default_directory", pathDownloadDir);
                        chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                        chromeOptions.AddUserProfilePreference("intl.accept_languages", intlAcceptLanguages);                        
                        webDriver = CreateChromeDriver(pathToDriver, chromeOptions, TimeSpan.FromSeconds(implicitWait));                       
                        break;
                    case NameOfBrowser.Firefox:
                        FirefoxOptions firefoxOptions = new FirefoxOptions();
                        Log.Info("Initialization FirefoxDriver options.");
                        firefoxOptions.AddArgument(startSizeWindow);
                        firefoxOptions.SetPreference("browser.download.folderList", 2);
                        firefoxOptions.SetPreference("browser.download.manager.showWhenStarting", false);
                        firefoxOptions.SetPreference("browser.download.dir", pathDownloadDir);
                        firefoxOptions.SetPreference("browser.download.downloadDir", pathDownloadDir);
                        firefoxOptions.SetPreference("browser.download.defaultFolder", pathDownloadDir);
                        firefoxOptions.SetPreference("browser.download.useDownloadDir", true);
                        firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/octet-stream");
                        webDriver = CreateFirefoxDriver(pathToDriver, firefoxOptions, TimeSpan.FromSeconds(implicitWait));
                        break;

                }
                return webDriver == null ? throw new NullReferenceException($"The error has been creating {webDriver.ToString()} driver.") : webDriver;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected error occurred during creating WebDriwer");
                return webDriver;
            }
        }        
        private IWebDriver CreateChromeDriver(string pathToDriver, ChromeOptions chromeOptions, TimeSpan implicitWait) {
            IWebDriver chromeDriver = null;
            try
            {
                Log.Info("Create CromeDriver.");
                chromeDriver = new ChromeDriver(
                    pathToDriver,
                    chromeOptions);
                chromeDriver.Manage().Timeouts().ImplicitWait = implicitWait;                
                return chromeDriver;
            }
            catch (Exception ex) {
                Log.Fatal(ex, "Unexpected error occurred during creation chrome driver.");
                return chromeDriver;
            }            
        }
        private IWebDriver CreateFirefoxDriver(string pathToDriver, FirefoxOptions firefoxOptions, TimeSpan implicitWait)
        {
            IWebDriver firefoxDriver = null;
            try 
            {
                Log.Info("Create FirefoxDriver.");
                firefoxDriver = new FirefoxDriver(
                    pathToDriver,
                    firefoxOptions);
                firefoxDriver.Manage().Timeouts().ImplicitWait = implicitWait;                                
                return firefoxDriver;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected error occurred during creating firefox driver.");
                return firefoxDriver;
            }            
        }
    }
}