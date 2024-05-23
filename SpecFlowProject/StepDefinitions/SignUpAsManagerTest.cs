using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using SpecFlowProject.Drivers;
using SpecFlowProject.Helpers;

namespace SpecFlowProject.StepDefinitions
{
    [Binding]
    public sealed class SignUpAsManagerTest
    {
        private ScenarioContext _scenarioContext;
        private SeleniumHelper _seleniumHelper;
        public SignUpAsManagerTest(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        [Given(@"I navigate to TrainingManagementSystem App on following environment")]
        public void GivenINavigateToTrainingManagementSystemAppOnFollowingEnvironment(Table table)
        {
            Dictionary<string, string> dictionary = ToDictionary(table);
            
            _seleniumHelper = new SeleniumHelper(_scenarioContext.Get<SeleniumDriver>("SeleniumDriver").SetUp(driver: dictionary["Browser"]));
            _seleniumHelper.GoTo(dictionary["URL"]);
        }

        [Given(@"I wait for DOM to be ready")]
        public void GivenIWaitForDOMToBeReady()
        {
            _seleniumHelper.WaitForDom(5000);
        }

        [Given(@"I click on Sign Up button")]
        public void GivenIClickOnSignUpButton()
        {
            _seleniumHelper.Click("//input[@value='Sign Up']");
        }

        [Given(@"I Fill the form")]
        public void GivenIFillTheForm(Table table)
        {
            Dictionary<string, string> dictionary = ToDictionary(table);

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.EnterText("//input[@id='FirstNameFieldId']", dictionary["FirstName"]);
            _seleniumHelper.EnterText("//input[@id='LastNameFieldId']", dictionary["LastName"]);
            _seleniumHelper.EnterText("//input[@id='NationalIdentificationNumberFieldId']", dictionary["IdentificationNber"]);
            _seleniumHelper.EnterText("//input[@id='MobileNumberFieldId']", dictionary["MobileNumber"]);
            _seleniumHelper.EnterText("//input[@id='EmailFieldId']", dictionary["Email"]);
            _seleniumHelper.Click("//select[@id='DepartmentComboBoxId']");
            _seleniumHelper.Click($"//option[text()='{dictionary["Department"]}']");
            _seleniumHelper.Click("//select[@id='RoleComboBoxId']");
            _seleniumHelper.Click($"//option[text()='{dictionary["Role"]}']");
            _seleniumHelper.EnterText("//input[@id='PasswordFieldId']", dictionary["Password"]);
            _seleniumHelper.EnterText("//input[@id='ConfirmPasswordFieldId']", dictionary["Password"]);
        }

        [Given(@"I select role")]
        public void GivenISelectRole(Table table)
        {
            Dictionary<string, string> dictionary = ToDictionary(table);

            _seleniumHelper.ExplicitWait($"//label[text()='{dictionary["Role"]}']/preceding-sibling::input", 5000);
            _seleniumHelper.Click($"//label[text()='{dictionary["Role"]}']/preceding-sibling::input");
        }

        [When(@"I click on Submit button and wait for DOM to be ready")]
        public void WhenIClickOnSubmitButtonAndWaitForDOMToBeReady()
        {
            _seleniumHelper.Click("//input[@value='Submit']");
        }

        [Then(@"correct element should be displayed")]
        public void ThenCorrectElementShouldBeDisplayed(Table table)
        {
            Dictionary<string, string> dictionary = ToDictionary(table);
            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.ExplicitWait($"//h2[contains(text(),'{dictionary["FirstName"]}') and contains( text(),'{dictionary["LastName"]}')]", 5000);
            ClassicAssert.IsTrue(_seleniumHelper.IsElementPresent($"//h2[contains(text(),'{dictionary["FirstName"]}') and contains( text(),'{dictionary["LastName"]}')]"));
        }

        // UTILS
        public static Dictionary<string, string> ToDictionary(Table table)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var row in table.Rows)
            {
                dictionary.Add(row[0], row[1]);
                Log($"{row[0]} : {row[1]}");
            }
            return dictionary;
        }

        public static void Log(string data)
        {
            string _filepath = "C:\\Users\\P12B20B\\source\\repos\\assignment\\test.txt";
            if (File.Exists(_filepath)) { Write(_filepath,data); }
            else { File.Create(_filepath); Write(_filepath,data); }
        }
        private static void Write(string _filepath,string data)
        {
            using (StreamWriter streamWriter = new StreamWriter(_filepath, true))
            {
                streamWriter.WriteLine(data);
                streamWriter.Flush();
            }
        }
    }
}