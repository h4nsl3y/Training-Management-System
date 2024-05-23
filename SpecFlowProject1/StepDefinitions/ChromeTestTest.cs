using OpenQA.Selenium;
using SpecFlowProject1.Drivers;
using SpecFlowProject1.Helpers;
using System.Diagnostics.Metrics;

namespace SpecFlowProject1.StepDefinitions
{
    [Binding]
    public sealed class ChromeTestTest
    {
        private ScenarioContext _scenarioContext;
        private SeleniumHelper _seleniumHelper;
        public ChromeTestTest(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("I navigate to LambdaTest App on following environment")]
        public void GivenINavigateToLambdaTestAppOnFollowingEnvironment(Table table)
        {
            _seleniumHelper = new SeleniumHelper(_scenarioContext.Get<SeleniumDriver>("SeleniumDriver").SetUp());
            _seleniumHelper.GoTo("https://www.google.com");
        }

        [Given("I select the first item")]
        public void GivenISelectTheFirstItem()
        {
            ScenarioContext.Current.Pending();
        }

        [Given("I select the second item")]
        public void GivenISelectTheSecondItem()
        {
            ScenarioContext.Current.Pending();
        }

        [Given("I enter the new value in the textbox")]
        public void GivenIEnterTheNewValueInTheTextbox()
        {
            ScenarioContext.Current.Pending();
        }
       
        [When("I click on Submit button")]
        public void WhenIClickOnSubmitButton()
        {
            ScenarioContext.Current.Pending();
        }

        [Then("I verify whether the item is added to the list")]
        public void ThenIVerifyWhetherTheItemIsAddedToTheList()
        {
            ScenarioContext.Current.Pending();
        }
    }
}