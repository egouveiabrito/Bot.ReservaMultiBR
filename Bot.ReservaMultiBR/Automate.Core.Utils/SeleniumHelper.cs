using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using OpenQA.Selenium.Chrome;
using System.Drawing;

namespace AutomationTest.Core
{
    public class SeleniumHelper : IDisposable
    {
        IWebDriver WebDriver = new ChromeDriver();
        public readonly ConfigurationHelper Configuration;
        public WebDriverWait Wait;
        public SeleniumHelper(ConfigurationHelper configuration)
        {
            Configuration = configuration;

            Wait = new WebDriverWait(WebDriver, TimeSpan.FromMilliseconds(10000));
        }
        public string GetUrl()
        {
            return WebDriver.Url;
        }
        public void GoToUrl(string url)
        {
            try
            {
                WebDriver.Navigate().GoToUrl(url);
            }
            catch (Exception erro)
            {
                Console.WriteLine(erro.Message);
                Finalizar();
            }
        }
        public bool VaidateContentUrl(string content)
        {
            return Wait.Until(ExpectedConditions.UrlContains(content));
        }
        public void ClickByTextLink(string textLink)
        {
            var link = Wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText(textLink)));
            link.Click();
        }
        public void ClickByButtomId(string buttomId)
        {
            var buttom = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(buttomId)));
            Wait.Until(ExpectedConditions.ElementToBeClickable(buttom)).Click();
        }
        public void ClickByButtomCssSelector(string cssSelector)
        {
            var buttom = Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(cssSelector)));
            Wait.Until(ExpectedConditions.ElementToBeClickable(buttom)).Click();
        }
        public void ClickByXPath(string xPath)
        {
            var element = Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));
            element.Click();
        }

        public void ExecuteScript()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)WebDriver;
            string title = (string)js.ExecuteScript("javascript:acessarSistema(3436, `newM`, 1)");
        }
        public void SetTab()
        {
            WebDriver.SwitchTo().Window(WebDriver.WindowHandles[1]);
        }
        public void Delay(int delay)
        {
            Thread.Sleep(delay);
        }
        public string GetURL()
        {
            return WebDriver.Url;
        }
        public void ClickById(string id)
        {
            var element = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
            element.Click();

        }
        public IWebElement GetElementById(string id)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)));
        }
        public IWebElement GetElementByClass(string classeCss)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(classeCss)));
        }
        public IWebElement GetElementByXPath(string xPath)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));
        }
        public string GetTextByXPath(string xPath)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath))).Text;
        }
        public bool ExistXpath(string xpath)
        {
            return WebDriver.FindElement(By.XPath(xpath)).Enabled;
        }
        public void FillTextBoxById(string fieldId, string value)
        {
            var field = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(fieldId)));
            field.SendKeys(value);
        }
        public void FillTextBoxByXPath(string fieldId, string value)
        {
            var field = Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(fieldId)));
            field.SendKeys(value);
        }
        public void FillDropDownById(string fieldId, string value)
        {
            var field = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(fieldId)));
            var selectElement = new SelectElement(field);
            selectElement.SelectByValue(value);
        }
        public string GetTextElementByClasseCss(string className)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(className))).Text;
        }
        public void MouseOverByXPath(string xpath)
        {
            var element = Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath)));
            Actions action = new Actions(WebDriver);
            action.MoveToElement(element).Perform();
        }
        public void MoveScrollByXPath(string xpath)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)WebDriver;
            IWebElement element = Wait.Until(ExpectedConditions.ElementExists(By.XPath(xpath))).FindElement(By.XPath(xpath));
            js.ExecuteScript("arguments[0].scrollIntoView();", element);
        }
        public string GetTextElementById(string id)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id))).Text;
        }
        public string GetTextBoxValueById(string id)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)))
                .GetAttribute("value");
        }
        public IEnumerable<IWebElement> GetListByClass(string className)
        {
            return Wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName(className)));
        }
        public bool ElementByIdIsVisible(string id)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id))).Displayed;
        }
        public bool ElementByIdIsInvisible(string id)
        {
            return Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id(id)));
        }
        public bool ExistsElementById(string id)
        {
            return ElementExists(By.Id(id));
        }
        public bool ExistsElementByClassName(string className)
        {
            return ElementExists(By.ClassName(className));
        }
        public bool ExistsElementByCssSelector(string cssSelector)
        {
            return ElementExists(By.CssSelector(cssSelector));
        }
        public bool RadioButtomIsSelected(string id)
        {
            var value = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id))).GetAttribute("checked");
            return value == "true";
        }
        public void BackNavigation(int vezes = 1)
        {
            for (var i = 0; i < vezes; i++)
            {
                WebDriver.Navigate().Back();
            }
        }


        public bool ElementExistsNoWaiting(By by)
        {
            try
            {
                var a = WebDriver.FindElement(by);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool ElementExists(By by)
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(by)).FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public void Close()
        {
            try
            {
                WebDriver.Close();
            }
            catch (Exception)
            {
            }
        }
        public void Finalizar()
        {
            try
            {
                if (WebDriver != null)
                {
                    WebDriver.Close();
                    WebDriver.Quit();
                }
            }
            catch
            {
            }
        }

        public void Dispose()
        {
            WebDriver.Dispose();
        }
    }
}