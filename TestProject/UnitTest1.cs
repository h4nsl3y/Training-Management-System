using TestProject.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestProject
{
    public class Tests
    {
        private static string _timestamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
        private string _firstName = $"TestFirstName";
        private string _lastName = $"TestLastName";
        private string _nationalIdentificationNumber = "T"+ _timestamp.Substring(_timestamp.Length - 13);
        private string _mobileNumber = _timestamp.Substring(_timestamp.Length - 8);
        private string _email = $"Test_{_timestamp}@email.com";
        private string _department = $"Product and Technology"; 
        private string _role = $"Administrator";
        private string _password = $"*password_{_timestamp}!";

        private const string _URL = "http://localhost:81/";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SeleniumTestRegister()
        {
            SeleniumHelper _seleniumHelper = new SeleniumHelper();

            _seleniumHelper.GoTo(_URL);
            // TODO ADD WAIT
            _seleniumHelper.ExplicitWait("//input[@value='Sign Up']", 5000);
            _seleniumHelper.Click("//input[@value='Sign Up']");
            // TODO ADD WAIT
            _seleniumHelper.WaitForDom();
            _seleniumHelper.EnterText("//input[@id='FirstNameFieldId']", _firstName);
            _seleniumHelper.EnterText("//input[@id='LastNameFieldId']", _lastName);
            _seleniumHelper.EnterText("//input[@id='NationalIdentificationNumberFieldId']", _nationalIdentificationNumber);
            _seleniumHelper.EnterText("//input[@id='MobileNumberFieldId']", _mobileNumber);
            _seleniumHelper.EnterText("//input[@id='EmailFieldId']", _email);

            _seleniumHelper.Click("//select[@id='DepartmentComboBoxId']");
            _seleniumHelper.Click($"//option[text()='{_department}']");
            
            _seleniumHelper.Click("//select[@id='RoleComboBoxId']");
            _seleniumHelper.Click($"//option[text()='{_role}']");


            _seleniumHelper.EnterText("//input[@id='PasswordFieldId']", _password);
            _seleniumHelper.EnterText("//input[@id='ConfirmPasswordFieldId']", _password);

            _seleniumHelper.Click("//input[@value='Sign Up']");

            // Thread.Sleep(20000);

            _seleniumHelper.ExplicitWait("//label[text()='Employee']/preceding-sibling::input", 5000);
            _seleniumHelper.Click("//label[text()='Employee']/preceding-sibling::input");
            _seleniumHelper.Click("//input[@value='Submit']");

            
            _seleniumHelper.WaitForDom();
            Assert.IsTrue(_seleniumHelper.IsElementPresent($"//h2[text()='{_firstName} {_lastName}']"));
            //Assert.Pass();
            //Assert.AreEqual("DummyAddress", customerAddress);
        }
    }
}