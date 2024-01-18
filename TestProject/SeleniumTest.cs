using TestProject.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace TestProject
{
    public class SeleniumTests
    {
        private SeleniumHelper _seleniumHelper;

        private static DateTime currentDate = DateTime.Now;
        private static string _timestamp = (currentDate).ToString("ddMMyyyyHHmmssffff");

        private static string _department = $"Product and Technology";
        private static string _password = $"{_timestamp}!";

        //MAnager details
        private string _managerFirstName = $"Manager";
        private string _managerLastName = _timestamp;
        private string _managerNationalIdentificationNumber = "TM" + _timestamp.Substring(_timestamp.Length - 12);
        private string _managerMobileNumber = _timestamp.Substring(_timestamp.Length - 8);
        private string _managerEmail = $"Manager{_timestamp}@email.com";
        //Employee details
        private string _employeeFirstName = "Employee_TestFirstName";
        private string _employeeLastName = "Employee_TestLastName";
        private string _employeeNationalIdentificationNumber = "TE" + _timestamp.Substring(_timestamp.Length - 12);
        private string _employeeMobileNumber = _timestamp.Substring(_timestamp.Length - 7) + "9";
        private string _employeeEmail = $"Employee{_timestamp}@email.com";

        private string _trainingTitleToApprove = "Advance Transactional SQL";
        private string _trainingTitleToReject = "JavaScript";
        private string _rejectionComment = "Rejection comment";
        private const string _URL = "https://localhost:81/";

        [SetUp]
        public void Setup()
        {
            _seleniumHelper = new SeleniumHelper();
        }

        [Test, Order(1)]
        public void SeleniumTest_SignUp_Manager()
        {
            _seleniumHelper.GoTo(_URL);
            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.Click("//input[@value='Sign Up']");

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.EnterText("//input[@id='FirstNameFieldId']", _managerFirstName);
            _seleniumHelper.EnterText("//input[@id='LastNameFieldId']", _managerLastName);
            _seleniumHelper.EnterText("//input[@id='NationalIdentificationNumberFieldId']", _managerNationalIdentificationNumber);
            _seleniumHelper.EnterText("//input[@id='MobileNumberFieldId']", _managerMobileNumber);
            _seleniumHelper.EnterText("//input[@id='EmailFieldId']", _managerEmail);
            _seleniumHelper.Click("//select[@id='DepartmentComboBoxId']");
            _seleniumHelper.Click($"//option[text()='{_department}']");
            _seleniumHelper.Click("//select[@id='RoleComboBoxId']");
            _seleniumHelper.Click($"//option[text()='Manager']");
            _seleniumHelper.EnterText("//input[@id='PasswordFieldId']", _password);
            _seleniumHelper.EnterText("//input[@id='ConfirmPasswordFieldId']", _password);
            _seleniumHelper.DoubleClick("//input[@value='Sign Up']");

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.ExplicitWait($"//h2[contains(text(),'{_managerFirstName}') and contains( text(),'{_managerLastName}')]", 5000);
            Assert.IsTrue(_seleniumHelper.IsElementPresent($"//h2[contains(text(),'{_managerFirstName}') and contains( text(),'{_managerLastName}')]"));
            _seleniumHelper.Close();
        }
        [Test, Order(2)]
        public void SeleniumTest_SignUp_Employee()
        {
            _seleniumHelper.GoTo(_URL);
            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.Click("//input[@value='Sign Up']");

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.EnterText("//input[@id='FirstNameFieldId']", $"{_employeeFirstName}");
            _seleniumHelper.EnterText("//input[@id='LastNameFieldId']", _employeeLastName);
            _seleniumHelper.EnterText("//input[@id='NationalIdentificationNumberFieldId']", _employeeNationalIdentificationNumber);
            _seleniumHelper.EnterText("//input[@id='MobileNumberFieldId']", _employeeMobileNumber);
            _seleniumHelper.EnterText("//input[@id='EmailFieldId']", _employeeEmail);
            _seleniumHelper.Click("//select[@id='DepartmentComboBoxId']");
            _seleniumHelper.Click($"//option[text()='{_department}']");
            _seleniumHelper.Click("//select[@id='ManagerComboBoxId']");
            _seleniumHelper.Click($"//option[contains(text(), '{_managerFirstName}') and contains(text(),'{_managerLastName}')]");
            _seleniumHelper.Click("//select[@id='RoleComboBoxId']");
            _seleniumHelper.Click($"//option[text()='Employee']");
            _seleniumHelper.EnterText("//input[@id='PasswordFieldId']", _password);
            _seleniumHelper.EnterText("//input[@id='ConfirmPasswordFieldId']", _password);
            _seleniumHelper.DoubleClick("//input[@value='Sign Up']");

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.ExplicitWait($"//h2[contains(text(),'{_employeeFirstName}') and contains( text(),'{_employeeLastName}')]", 5000);
            Assert.IsTrue(_seleniumHelper.IsElementPresent($"//h2[contains(text(),'{_employeeFirstName}') and contains( text(),'{_employeeLastName}')]"));
            _seleniumHelper.Close();
        }
        [Test, Order(3)]
        public void SeleniumTest_SignIn_employee()
        {
            _seleniumHelper.GoTo(_URL);
            _seleniumHelper.WaitForDom(5000);

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.EnterText("//input[@id='employeeEmailId']", _employeeEmail);
            _seleniumHelper.EnterText("//input[@id='employeePasswordId']", _password);
            _seleniumHelper.Click("//input[@value='Sign In']");

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.ExplicitWait($"//h2[contains(text(),'{_employeeFirstName}') and contains( text(),'{_employeeLastName}')]", 5000);
            Assert.IsTrue(_seleniumHelper.IsElementPresent($"//h2[contains(text(),'{_employeeFirstName}') and contains( text(),'{_employeeLastName}')]"));
            _seleniumHelper.Close();
        }
       [Test, Order(4)]
        public void SeleniumTest_EnrollTraining()
        {
            string startDate = (currentDate.AddDays(5)).ToString("ddMMyyyyHHmmtt");
            string endDate = (currentDate.AddDays(10)).ToString("ddMMyyyyHHmmtt");
            string deadlineDate = (currentDate.AddDays(1)).ToShortDateString();

            _seleniumHelper.GoTo(_URL);
            _seleniumHelper.WaitForDom(5000);

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.EnterText("//input[@id='employeeEmailId']", _employeeEmail);
            _seleniumHelper.EnterText("//input[@id='employeePasswordId']", _password);
            _seleniumHelper.Click("//input[@value='Sign In']");

            _seleniumHelper.WaitForDom(5000);

            _seleniumHelper.ExplicitWait($".//td[text()='{_trainingTitleToApprove}']//following-sibling::td/button", 8000);
            _seleniumHelper.Click($".//td[text()='{_trainingTitleToApprove}']//following-sibling::td/button");
            _seleniumHelper.ExplicitWait("//div[@id = 'screenOverlay' and contains(@style,'visible')]", 5000);

            _seleniumHelper.Click("//button[@id = 'enrollBtn' and contains(@style,'visible')]");

            _seleniumHelper.ExplicitWait("//p[@id='notificationMessage' and contains(text(),'Successfully enrolled')]", 2000);
            Assert.IsTrue(_seleniumHelper.IsElementPresent("//p[@id='notificationMessage' and contains(text(),'Successfully enrolled')]"));


            _seleniumHelper.Click($".//td[text()='{_trainingTitleToReject}']//following-sibling::td/button");
            _seleniumHelper.ExplicitWait($"//div[@id = 'screenOverlay' and contains(@style,'visible')]", 5000);

            _seleniumHelper.Click("//button[@id = 'enrollBtn' and contains(@style,'visible')]");

            _seleniumHelper.ExplicitWait("//p[@id='notificationMessage' and contains(text(),'Successfully enrolled')]", 2000);
            Assert.IsTrue(_seleniumHelper.IsElementPresent("//p[@id='notificationMessage' and contains(text(),'Successfully enrolled')]"));

            _seleniumHelper.Close();
        }
        [Test, Order(5)]
        public void SeleniumTest_Approve_Manager()
        {
            _seleniumHelper.GoTo(_URL);
            _seleniumHelper.WaitForDom(5000);

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.EnterText("//input[@id='employeeEmailId']", _managerEmail);
            _seleniumHelper.EnterText("//input[@id='employeePasswordId']", _password);
            _seleniumHelper.Click("//input[@value='Sign In']");

            _seleniumHelper.ExplicitWait($"//label[text()='Manager']/preceding-sibling::input", 5000);
            _seleniumHelper.Click($"//label[text()='Manager']/preceding-sibling::input");
            _seleniumHelper.Click("//input[@value='Submit']");

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.ExplicitWait($"//h2[contains(text(),'{_managerFirstName}') and contains( text(),'{_managerLastName}')]", 5000);

            _seleniumHelper.Click($"//table[@id='employeeEnrollmentTableId']//td[contains(text(),'{_employeeFirstName}') and contains( text(),'{_employeeLastName}')]//following-sibling::td//button[@id='detailBtn']");
            _seleniumHelper.ExplicitWait("//div[@id = 'screenOverlay' and contains(@style,'visible')]", 5000);

            _seleniumHelper.Click($"//td[text()='{_trainingTitleToApprove}']//following-sibling::td//button[text()='Approve']");
            _seleniumHelper.Close();
        }
        [Test, Order(6)]
        public void SeleniumTest_Reject_Request_Manager()
        {
            _seleniumHelper.GoTo(_URL);
            _seleniumHelper.WaitForDom(5000);

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.EnterText("//input[@id='employeeEmailId']", _managerEmail);
            _seleniumHelper.EnterText("//input[@id='employeePasswordId']", _password);
            _seleniumHelper.Click("//input[@value='Sign In']");

            _seleniumHelper.ExplicitWait($"//label[text()='Manager']/preceding-sibling::input", 5000);
            _seleniumHelper.Click($"//label[text()='Manager']/preceding-sibling::input");
            _seleniumHelper.Click("//input[@value='Submit']");

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.ExplicitWait($"//h2[contains(text(),'{_managerFirstName}') and contains( text(),'{_managerLastName}')]", 5000);

            _seleniumHelper.Click($"//table[@id='employeeEnrollmentTableId']//td[contains(text(),'{_employeeFirstName}') and contains( text(),'{_employeeLastName}')]//following-sibling::td//button[@id='detailBtn']");
            _seleniumHelper.ExplicitWait("//div[@id = 'screenOverlay' and contains(@style,'visible')]", 5000);

            _seleniumHelper.ExplicitWait("//td[text()='JavaScript']//following-sibling::td//button[text()='Reject']", 5000);

            _seleniumHelper.Click("//td[text()='JavaScript']//following-sibling::td//button[text()='Reject']");
            _seleniumHelper.Click("//div[@id = 'commentContainerId' and contains(@style,'visible')]");
            _seleniumHelper.EnterText("//textarea[@id='rejectionReasonid']", _rejectionComment);
            _seleniumHelper.Click("//div[@id='commentContainerId']//button[text()='Submit']");

            _seleniumHelper.Close();
        }
        [Test, Order(7)]
        public void SeleniumTest_CheckUpdateState_employee()
        {
            _seleniumHelper.GoTo(_URL);
            _seleniumHelper.WaitForDom(5000);

            _seleniumHelper.WaitForDom(5000);
            _seleniumHelper.EnterText("//input[@id='employeeEmailId']", _employeeEmail);
            _seleniumHelper.EnterText("//input[@id='employeePasswordId']", _password);
            _seleniumHelper.Click("//input[@value='Sign In']");

            //_seleniumHelper.ExplicitWait("//label[text()='Employee']/preceding-sibling::input", 5000);
            //_seleniumHelper.Click("//label[text()='Employee']/preceding-sibling::input");
            //_seleniumHelper.Click("//input[@value='Submit']");

            _seleniumHelper.WaitForDom(5000);

            _seleniumHelper.ExplicitWait($"//button[text()='Submitted training request']", 5000);
            _seleniumHelper.Click("//button[text()='Submitted training request']");

            _seleniumHelper.ImplicitWait(5000);

            _seleniumHelper.Click("//td[text()='Approved']//following-sibling::td//button[@id='detailBtn']");
            _seleniumHelper.ExplicitWait("//div[@id = 'screenOverlay' and contains(@style,'visible')]", 5000);
            Assert.IsTrue(_seleniumHelper.IsElementPresent($"//div[@id='screenOverlay']//p[contains(text(),'{_trainingTitleToApprove}')]"));
            _seleniumHelper.Click("//div[@id='screenOverlay']//button[text()='Close']");

            _seleniumHelper.ImplicitWait(5000);

            _seleniumHelper.Click("//td[text()='Rejected']//following-sibling::td//button[@id='detailBtn']");
            _seleniumHelper.ExplicitWait("//div[@id = 'screenOverlay' and contains(@style,'visible')]", 5000);
            Assert.IsTrue(_seleniumHelper.IsElementPresent($"//div[@id='screenOverlay']//p[contains(text(),'{_trainingTitleToReject}')]"));

            _seleniumHelper.Close();
        }
    }
}