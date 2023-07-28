using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Diagnostics;
using System.IO;
namespace AutomationTest.Core
{
    public static class WebDriverFactory
    {
        public static IWebDriver CreateWebDriver(Browser browser, string FolderDriver, bool headless)
        {
            IWebDriver webDriver = null;
            FileVersionInfo chromeVersion = null;
            if (Directory.Exists(@"C:\Program Files\Google\Chrome\Application"))
                chromeVersion = FileVersionInfo.GetVersionInfo(@"C:\Program Files\Google\Chrome\Application\chrome.exe");
            else
                chromeVersion = FileVersionInfo.GetVersionInfo(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe");
            switch (browser)
            {
                case Browser.Firefox:
                    var optionsFireFox = new FirefoxOptions();
                    if (headless)
                        optionsFireFox.AddArguments("--headless", "--disable-gpu", "window-size=1920,1080");
                    webDriver = new FirefoxDriver(FolderDriver, optionsFireFox);
                    break;
                case Browser.Chrome:
                    var options = new ChromeOptions();
                    if (headless)
                        options.AddArguments("--headless", "--disable-gpu", "window-size=1920,1080", "--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, como Gecko) Chrome/74.0.3729.157 Safari/537.36");
                    else
                        options.AddArguments("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, como Gecko) Chrome/74.0.3729.157 Safari/537.36");
                    webDriver = new ChromeDriver($"{FolderDriver}\\{chromeVersion.FileMajorPart}", options, System.TimeSpan.FromSeconds(120));
                    break;
            }
            if (!headless)
                webDriver.Manage().Window.Maximize();
            return webDriver;
        }
    }
}