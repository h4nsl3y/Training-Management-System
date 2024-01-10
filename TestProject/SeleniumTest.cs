using TestProject.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.ComponentModel.DataAnnotations;

namespace TestProject
{
    public class SeleniumTests
    {
        private SeleniumHelper _seleniumHelper;
        private Random _rand;

        private static DateTime currentDate = DateTime.Now;
        private static string _timestamp = (currentDate).ToString("ddMMyyyyHHmmssffff");
        private string _firstName = $"TestFirstName";
        private string _lastName = $"TestLastName";
        private string _nationalIdentificationNumber = "T"+ _timestamp.Substring(_timestamp.Length - 13);
        private string _mobileNumber = _timestamp.Substring(_timestamp.Length - 8);
        private string _email = $"Test_{_timestamp}@email.com";
        private string _department = $"Product and Technology"; 
        private string _role = $"Administrator";
        private string _password = $"*password_{_timestamp}!";

        private string _trainingTitle = $"TestTrainint_{_timestamp}";

        private const string _URL = "https://localhost:81/";

        [SetUp]
        public void Setup()
        {
            _seleniumHelper = new SeleniumHelper();
            _rand = new Random();
        }
        #region SeleniumTest
        [Test, Order(1)]
        public void SeleniumTest_SignUp()
        {
            _seleniumHelper.GoTo(_URL);
            _seleniumHelper.WaitForDom(5000);
            //_seleniumHelper.ExplicitWait("//input[@value='Sign Up']", 5000);
            _seleniumHelper.Click("//input[@value='Sign Up']");

            _seleniumHelper.WaitForDom(5000);
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

            _seleniumHelper.ExplicitWait("//label[text()='Administrator']/preceding-sibling::input", 5000);
            _seleniumHelper.Click("//label[text()='Administrator']/preceding-sibling::input");
            _seleniumHelper.Click("//input[@value='Submit']");

            _seleniumHelper.WaitForDom(5000);

            Assert.IsTrue(_seleniumHelper.IsElementPresent($"//h2[text()='{_firstName} {_lastName}']"));
            _seleniumHelper.Close();

        }
/*        [Test]
        public void SeleniumTestAddTraining()
        {
            string startDate = (currentDate.AddDays(5)).ToString("ddMMyyyyHHmmtt");
            string endDate = (currentDate.AddDays(10)).ToString("ddMMyyyyHHmmtt");
            string deadlineDate = (currentDate.AddDays(1)).ToShortDateString();

            _seleniumHelper.ExplicitWait("//h3[text()='Training']//following-sibling::button[text()='Add']", 5000);
            _seleniumHelper.Click("//h3[text()='Training']//following-sibling::button[text()='Add']");

            _seleniumHelper.ExplicitWait("//div[@id='screenOverlay' and contains(@style,'visible')]", 5000);
            _seleniumHelper.EnterText("//input[@id='trainingTitleId']", _trainingTitle);
            _seleniumHelper.Click("//select[@id='trainingDepartmentId']");
            _seleniumHelper.Click("//select[@id='trainingDepartmentId']//child::option[last()]");

            _seleniumHelper.EnterText("//input[@id='trainingStartDateId']", startDate);
            _seleniumHelper.EnterText("//input[@id='trainingEndDateId']", endDate);
            _seleniumHelper.EnterText("//input[@id='trainingDeadLineId']", deadlineDate);

            _seleniumHelper.EnterText("//input[@id='trainingSeatAvailableId']", _rand.Next(0, 32767).ToString());
            _seleniumHelper.EnterText("//textArea[@id='trainingShortDescriptionId']","Short test description");
            _seleniumHelper.EnterText("//textArea[@id='trainingLongDescriptionId']", "Long test description");
            _seleniumHelper.Click("//button[@id='submitTrainingDetailsBtn']");

            Assert.IsTrue(_seleniumHelper.IsElementPresent("//p[@id='notificationMessage' and text()='Training has been successfully registered']"));

            _seleniumHelper.Close();

        }*/


        [Test, Order(2)]
        public void SeleniumTest_SignIn()
        {
            _seleniumHelper.GoTo(_URL);
            _seleniumHelper.WaitForDom(5000);

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.EnterText("//input[@id='employeeEmailId']", _email);
            _seleniumHelper.EnterText("//input[@id='employeePasswordId']", _password);
            _seleniumHelper.Click("//input[@value='Sign In']");

            _seleniumHelper.ExplicitWait("//label[text()='Employee']/preceding-sibling::input", 5000);
            _seleniumHelper.Click("//label[text()='Employee']/preceding-sibling::input");
            _seleniumHelper.Click("//input[@value='Submit']");

            _seleniumHelper.WaitForDom(5000);

            Assert.IsTrue(_seleniumHelper.IsElementPresent($"//h2[text()='{_firstName} {_lastName}']"));
            _seleniumHelper.Close();
        }


       [Test, Order(3)]
        public void SeleniumTest_CreateTraining()
        {

            string startDate = (currentDate.AddDays(5)).ToString("ddMMyyyyHHmmtt");
            string endDate = (currentDate.AddDays(10)).ToString("ddMMyyyyHHmmtt");
            string deadlineDate = (currentDate.AddDays(1)).ToShortDateString();

            _seleniumHelper.GoTo(_URL);
            _seleniumHelper.WaitForDom(5000);

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.EnterText("//input[@id='employeeEmailId']", _email);
            _seleniumHelper.EnterText("//input[@id='employeePasswordId']", _password);
            _seleniumHelper.Click("//input[@value='Sign In']");

            _seleniumHelper.ExplicitWait("//label[text()='Employee']/preceding-sibling::input", 5000);
            _seleniumHelper.Click("//label[text()='Employee']/preceding-sibling::input");
            _seleniumHelper.Click("//input[@value='Submit']");

            _seleniumHelper.WaitForDom(5000);

            //Assert.IsTrue(_seleniumHelper.IsElementPresent($"//h2[text()='{_firstName} {_lastName}']"));

            //_seleniumHelper.WaitToDissapear(".//div[@id='spinnerContainer']", 5000);
            //_seleniumHelper.ImplicitWait(8000);
            _seleniumHelper.ExplicitWait(".//td[text()='Advance Transactional SQL']//following-sibling::td/button", 8000);
            _seleniumHelper.Click(".//td[text()='Advance Transactional SQL']//following-sibling::td/button");
            _seleniumHelper.ExplicitWait("//div[@id = 'screenOverlay' and contains(@style,'visible')]", 5000);

            _seleniumHelper.Click("//button[@id = 'enrollBtn' and contains(@style,'visible')]");

            _seleniumHelper.ExplicitWait("//p[@id='notificationMessage' and contains(text(),'Successfully enrolled')]", 2000);
            Assert.IsTrue(_seleniumHelper.IsElementPresent("//p[@id='notificationMessage' and contains(text(),'Successfully enrolled')]"));

            _seleniumHelper.Close();
        }
        #endregion
    }
}