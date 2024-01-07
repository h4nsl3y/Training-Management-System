using TestProject.Services;

namespace TestProject
{
    public class Tests
    {
        private const string _FIRSTNAME = "TestFirstName";
        private const string _OTHERNAME = "TestOtherName";
        private const string _LASTNAME = "TestLastName";
        private const string _NATIONALIDENTIFICATIONNUMBER = "T0123456789123";
        private const string _MOBILENUMBER = "56789023";
        private const string _EMAIL = "Test@email.com";
        private const string _DEPARTMENT = "Product and Technology"; 
        private const string _ROLE = "Administrator";
        private const string _PASSWORD = "*password!";


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SeleniumTestRegister()
        {
            SeleniumService _service = new SeleniumService();

            _service.GoTo("http://localhost:56075/");
            // TODO ADD WAIT
            _service.Click("//input[@value='Sign Up']");
            // TODO ADD WAIT
            _service.EnterText("//input[@id='FirstNameFieldId']", _FIRSTNAME);
            _service.EnterText("//input[@id='OtherNameFieldId']", _OTHERNAME);
            _service.EnterText("//input[@id='LastNameFieldId']", _LASTNAME);
            _service.EnterText("//input[@id='NationalIdentificationNumberFieldId']", _NATIONALIDENTIFICATIONNUMBER);
            _service.EnterText("//input[@id='MobileNumberFieldId']", _MOBILENUMBER);
            _service.EnterText("//input[@id='EmailFieldId']", _FIRSTNAME);

            _service.Click("//input[@id='DepartmentComboBoxId']");
            _service.Click($"//option[text()='{_DEPARTMENT}']");
            
            _service.EnterText("//input[@id='RoleComboBoxId']", _FIRSTNAME);
            _service.Click($"//option[text()='{_ROLE}']");


            _service.EnterText("//input[@id='PasswordFieldId']", _PASSWORD);
            _service.EnterText("//input[@id='ConfirmPasswordFieldId']", _PASSWORD);

            _service.Click("//option[@value='Sign Up']");

            Thread.Sleep(20000);

            Assert.Pass();
        }
    }
}