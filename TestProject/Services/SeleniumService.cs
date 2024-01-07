using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Services
{
    public class SeleniumService
    {
        private IWebDriver _driver;
        public SeleniumService()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
        }

        public void GoTo(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }
        public void Click(string xpath)
        {
            _driver.FindElement(By.XPath(xpath)).Click();
        }
        public void Clear(string xpath)
        {
            _driver.FindElement(By.XPath(xpath)).Clear();
        }
        public bool IsElementPresent(string xpath)
        {
            bool flag = _driver.FindElement(By.XPath(xpath)).Enabled;
            return flag;
        }
        public void EnterText(string xpath, string text)
        {
            IWebElement element = _driver.FindElement(By.XPath(xpath));
            element.SendKeys(text);
        }
        public void DoubleClick(string xpath)
        {
            IWebElement element = _driver.FindElement(By.XPath(xpath));
            Actions action = new Actions(_driver);
            action.DoubleClick(element).Perform();
        }
        public void ImplicitWait(int millisecond)
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(millisecond);
        }
        public void Close()
        {
            _driver.Close();
        }
        public void SwitchToNewWindow(int index)
        {
            _driver.SwitchTo().Window(_driver.WindowHandles[index]);
        }
        public void HoverOver(string xpath)
        {
            IWebElement element = _driver.FindElement(By.XPath(xpath));
            Actions action = new Actions(_driver);
            action.MoveToElement(element).Perform();
        }
    }
}
