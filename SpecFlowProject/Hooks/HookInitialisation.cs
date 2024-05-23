using OpenQA.Selenium;
using SpecFlowProject.Drivers;
using TechTalk.SpecFlow;

namespace SpecFlowProject.Hooks
{
    [Binding]
    public sealed class HookInitialisation
    {
        private readonly ScenarioContext _scenarioContext;

        public HookInitialisation(ScenarioContext scenarioContext) => _scenarioContext =  scenarioContext;
        


        [BeforeScenario]
        public void BeforeScenarioWithTag()
        {
            Console.WriteLine("Initialising Selenium webdriver");
            SeleniumDriver seleniumDriver = new SeleniumDriver(_scenarioContext);
            _scenarioContext.Set(seleniumDriver, "SeleniumDriver");
        }
        [AfterScenario]
        public void AfterScenario()
        {
            Console.WriteLine("Selenium webdriver action : QUIT");
            _scenarioContext.Get<IWebDriver>("Webdriver").Quit();
        }
    }
}