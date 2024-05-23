using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlowProject1.Drivers
{
    public class SeleniumDriver
    {
        private IWebDriver _driver;
        private readonly ScenarioContext _scenarioContext;

        public SeleniumDriver(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;

        public IWebDriver SetUp(string driver = "chrome", List<string>? argument = null)
        {
            switch (driver.ToLower())
            {
                case "firefox":
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AddArgument("-private");
                    _driver = new FirefoxDriver(firefoxOptions);
                    break;
                case "edge":
                    EdgeOptions edgeOptions = new EdgeOptions();
                    edgeOptions.AddArguments("inprivate");
                    _driver = new EdgeDriver(edgeOptions);
                    break;
                default:
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("--incognito");
                    _driver = new ChromeDriver(chromeOptions);
                    break;
            }
            _scenarioContext.Set(_driver, "WebDriver");
            _driver.Manage().Window.Maximize();
            return _driver;
        }
    }
}
