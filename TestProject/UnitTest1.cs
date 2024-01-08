using TestProject.Services;

namespace TestProject
{
    public class Tests
    {
        private static string _timestamp = (DateTime.Now).ToString("yyyyMMddHHmmssffff");
        private string firtName = $"TestFirstName";
        private string _lastName = $"TestLastName";
        private string _nationalIdentificationNumber = "T"+ _timestamp.Substring(_timestamp.Length - 13);
        private string _mobileNumber = _timestamp.Substring(_timestamp.Length - 8);
        private string _EMAIL = $"Test_{_timestamp}@email.com";
        private string _DEPARTMENT = $"Product and Technology"; 
        private string _ROLE = $"Administrator";
        private string _PASSWORD = $"*password_{_timestamp}!";

        private const string _URL = "http://192.168.100.210:81";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SeleniumTestRegister()
        {
            SeleniumService _service = new SeleniumService();

            _service.GoTo(_URL);
            // TODO ADD WAIT
            _service.ExplicitWait("//input[@value='Sign Up']", 5000);
            _service.Click("//input[@value='Sign Up']");
            // TODO ADD WAIT
            _service.WaitForDom();
            _service.EnterText("//input[@id='FirstNameFieldId']", firtName);
            _service.EnterText("//input[@id='LastNameFieldId']", _lastName);
            _service.EnterText("//input[@id='NationalIdentificationNumberFieldId']", _nationalIdentificationNumber);
            _service.EnterText("//input[@id='MobileNumberFieldId']", _mobileNumber);
            _service.EnterText("//input[@id='EmailFieldId']", _EMAIL);

            _service.Click("//select[@id='DepartmentComboBoxId']");
            _service.Click($"//option[text()='{_DEPARTMENT}']");
            
            _service.Click("//select[@id='RoleComboBoxId']");
            _service.Click($"//option[text()='{_ROLE}']");


            _service.EnterText("//input[@id='PasswordFieldId']", _PASSWORD);
            _service.EnterText("//input[@id='ConfirmPasswordFieldId']", _PASSWORD);

            _service.Click("//input[@value='Sign Up']");

            // Thread.Sleep(20000);

            _service.ExplicitWait("//label[text()='Employee']/preceding-sibling::input", 5000);
            _service.Click("//label[text()='Employee']/preceding-sibling::input");
            _service.Click("//input[@value='Submit']");

            Assert.Pass();
        }
    }
}