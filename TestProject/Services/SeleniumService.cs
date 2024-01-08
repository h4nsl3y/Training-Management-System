using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace TestProject.Services
{
    public class SeleniumService
    {
        private IWebDriver _driver;
        public SeleniumService()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--incognito");
            _driver = new ChromeDriver(options);
            _driver.Manage().Window.Maximize();
        }
        public void Clear(string xpath) => _driver.FindElement(By.XPath(xpath)).Clear();
        public void Click(string xpath) => _driver.FindElement(By.XPath(xpath)).Click();
        public void Close() => _driver.Close();
        public void DoubleClick(string xpath)
        {
            IWebElement element = _driver.FindElement(By.XPath(xpath));
            Actions action = new Actions(_driver);
            action.DoubleClick(element).Perform();
        }
        public void EnterText(string xpath, string text)
        {
            IWebElement element = _driver.FindElement(By.XPath(xpath));
            element.SendKeys(text);
        }
        public void ExplicitWait(string xpath, int millisecond)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(millisecond))
            {
                PollingInterval = TimeSpan.FromMilliseconds(200),
            };
            wait.IgnoreExceptionTypes(typeof(ElementNotInteractableException));
            wait.Until(d => d.FindElement(By.XPath(xpath)));
        }
        public void GoTo(string url) => _driver.Navigate().GoToUrl(url);
        public void HoverOver(string xpath)
        {
            IWebElement element = _driver.FindElement(By.XPath(xpath));
            Actions action = new Actions(_driver);
            action.MoveToElement(element).Perform();
        }
        public void ImplicitWait(int millisecond) => _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(millisecond);
        public bool IsElementPresent(string xpath) => _driver.FindElement(By.XPath(xpath)).Enabled;
        public void SwitchToNewWindow(int index) => _driver.SwitchTo().Window(_driver.WindowHandles[index]);
        public void WaitForDom()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(webDriver => ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState").Equals("complete"));
        }
    }
}
