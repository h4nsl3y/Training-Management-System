using TestProject.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.ComponentModel.DataAnnotations;
using DAL.Repository.AccountRepositories;
using Moq;
using DAL.Entity;
using DAL.Enum;
using BLL.AccountBusinessLogics;
using DAL.Logger;
using System.Collections.Generic;
using System.Reflection;
using OpenQA.Selenium.DevTools.V118.Network;
using System.Data.Common;
using System;

namespace TestProject
{
    [TestFixture]
    public class UnitTest
    {
        private Mock<IAccountRepository> _stubAccountRepository;
        private AccountBusinessLogic _accountBusinessLogic;

        private List<Account> _accountList;

        [SetUp]
        public void Setup()
        {
            _accountList = new List<Account>()
            {
                new Account(){
                    AccountId = 1,
                    FirstName = "AAA",
                    OtherName = "",
                    LastName = "DDD",
                    NationalIdentificationNumber = "A1234567890123",
                    MobileNumber = "51234567",
                    Email = "AAADDD@email.com",
                    DepartmentId = (int)DepartmentEnum.Product_and_Technology ,
                    ManagerId = 2,
                    Password = "$2a$13$xEdplDaM43mKq9xDkSXzr.ytbHFJKLZy/YadSd2/KIS6h0qjWNcXi", // employee
                    RoleId = (int)RoleEnum.Employee },

                new Account(){
                    AccountId = 2,
                    FirstName = "BBB",
                    OtherName = "",
                    LastName = "DDD",
                    NationalIdentificationNumber = "B1234567890123",
                    MobileNumber = "52234567",
                    Email = "BBBDDD@email.com",
                    DepartmentId = (int)DepartmentEnum.Product_and_Technology ,
                    ManagerId = 3,
                    Password = "$2a$13$hqDOc5VS7WtZ1l.uFbL/1Oi9aIfTqmEsvRL8VYwADp6hWqeb.z5la", // manager
                    RoleId = (int)RoleEnum.Manager },

                new Account(){
                    AccountId = 3,
                    FirstName = "CCC",
                    OtherName = "",
                    LastName = "DDD",
                    NationalIdentificationNumber = "C1234567890123",
                    MobileNumber = "53234567",
                    Email = "CCCDDD@email.com",
                    DepartmentId = (int)DepartmentEnum.Product_and_Technology ,
                    Password = "$2a$13$H1f8wryG3Lk0KtzV7ayvQ.3I2bsCbFpjw6HysbIJsxkr/M5K8ryOG", // administrator
                    RoleId = (int)RoleEnum.Administrator }
            };

            _stubAccountRepository = new Mock<IAccountRepository>();

            _stubAccountRepository.Setup(accountRepository => accountRepository.AddAsync(It.IsAny<Account>()))
                .ReturnsAsync((Account accountInstance) =>
                {
                    _accountList.Add(accountInstance);
                    return new Response<bool> { Success = true, Data = { true } };
                });

            _stubAccountRepository.Setup(accountRepository => accountRepository.AuthenticateAsync(It.IsAny<string>())).
                ReturnsAsync((string userEmail) => 
                    new Response<Account>() { Success = true, Data = { _accountList.FirstOrDefault(account => account.Email == userEmail) } });
            _stubAccountRepository.Setup(accountRepository => accountRepository.DuplicatedAsync(It.IsAny<Dictionary<string, object>>())).
                ReturnsAsync((Dictionary<string, object> conditions) =>
                {
                    Response<bool> result = new() { Success = true };
                    foreach (var condition in conditions)
                    {
                        bool isDuplicate = _accountList.Any(account => account.GetType().GetProperty(condition.Key, 
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(account).Equals(condition.Value));
                        result.Data.Add(isDuplicate);
                    }
                    return result;
                });
            _stubAccountRepository.Setup(accountRepository => accountRepository.GetAsync(It.IsAny<Dictionary<string, object>>())).
                ReturnsAsync((Dictionary<string, object> dictionary) => {
                    string attribute = dictionary.Keys.First();
                    var value = dictionary.Values.First();
                    return new Response<Account>()
                    {
                        Success = true,
                        Data = _accountList.Where(account => account.GetType().GetProperty(attribute,
                            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(account).Equals(value)).ToList() ?? new List<Account>()
                    };
                });
            _stubAccountRepository.Setup(accountRepository => accountRepository.GetAllAsync(It.IsAny<Dictionary<string, object>>())).
                ReturnsAsync((Dictionary<string, object> dictionary) => {
                    string attribute = dictionary.Keys.First() ?? string.Empty;
                    var value = dictionary.Values.First() ?? string.Empty;

                    try
                    {
                        return (attribute.Equals(string.Empty) && value.Equals(string.Empty)) ?
                        new Response<Account>()
                        { Success = true, Data = _accountList } :

                        new Response<Account>()
                        {
                            Success = true,
                            Data = _accountList.Where(account => account.GetType().GetProperty(attribute, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(account).Equals(value)).ToList(),
                        };
                    }
                    catch
                    {
                        return new Response<Account>() { Success = false, Data = new List<Account>() };
                    }
                    
                });
            _stubAccountRepository.Setup(accountRepository => accountRepository.GetActiveRequestEmployeeAsync(It.IsAny<int>())).
                ReturnsAsync((int managerId) =>
                    new Response<Account>()
                    {
                        Success = true,
                        Data = _accountList.Where(account => account.ManagerId == managerId).ToList(),
                    });
            _stubAccountRepository.Setup(accountRepository => accountRepository.GetManagerListAsync()).
                ReturnsAsync(
                    new Response<Account>()
                {
                    Success = true,
                    Data = _accountList.Where(account =>
                    account.RoleId == (int)RoleEnum.Manager ||
                    account.RoleId == (int)RoleEnum.Administrator
                    ).ToList()
                } );

            _accountBusinessLogic = new AccountBusinessLogic(_stubAccountRepository.Object, null);
        }

        [Test]
        public async Task Test_AddAccountAsync()
        {
            //Arrange
            Account accountInstance = new()
            {
                AccountId = 4,
                FirstName = "TestFirstName",
                OtherName = "",
                LastName = "TestLastName",
                NationalIdentificationNumber = "T1234567890123",
                MobileNumber = "56543217",
                Email = "Test@email.com",
                DepartmentId = (int)DepartmentEnum.Product_and_Technology,
                Password = "tess_password!", 
                RoleId = (int)RoleEnum.Employee
            };
            //Act 
            Response<bool> response = await _accountBusinessLogic.AddAccountAsync(accountInstance);
            Account lastAddedAccount = _accountList.Last();
            //Assert
            Assert.IsTrue(response.Success && accountInstance.AccountId == lastAddedAccount.AccountId);
        }

        [Test]
        [TestCase("AAADDD@email.com", "manager", ExpectedResult = false)]
        [TestCase("AAADDD@email.com", "employee", ExpectedResult = true)]
        public async Task<bool> Test_AuthenticateUserError1(string employeeEmail, string password)
        {
            //Arrange
            //Act
            Response<bool> response = await _accountBusinessLogic.AuthenticatedAsync(employeeEmail, password);
            //Assert
            return response.Data.First();
        }

        [Test]
        [TestCase("CCCDDD@email.com", "C1234567890123", "53234567", ExpectedResult = true)]
        [TestCase("fakeEmail", "C1234567890123", "53234567", ExpectedResult = true)]
        [TestCase("fakeEmail", "fakeId", "FakeNumber", ExpectedResult = false)]
        public async Task<bool> Test_IsDuplicatedAsync(string email, string nationalIdentificationNumber, string mobileNumber)
        {
            //Arrange
            //Act
            Response<bool> response = await _accountBusinessLogic.IsDuplicatedAsync(email, nationalIdentificationNumber, mobileNumber);
            //Assert
            return response.Data.Any(value => value == true);
        }

        [Test]
        [TestCase("Email", "AAADDD@email.com", ExpectedResult = true)]
        [TestCase("Email", "FakeEmail", ExpectedResult = false)]
        [TestCase("AccountId", 1, ExpectedResult = true)]
        [TestCase("AccountID", 100, ExpectedResult = false)]
        public async Task<bool> Test_GetAccountAsync(string columnName, object value)
        {
            //Arrange
            Dictionary<string, object> dictionary = new () { { columnName, value } };
            //Act 
            Response<Account> response = await _accountBusinessLogic.GetAccountAsync(dictionary);
            //Assert
            return response.Data.Any(account => account.GetType().GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(account).Equals(value));
        }

        [Test]
        [TestCase("","",ExpectedResult = 3 )]
        [TestCase("Email", "AAADDD@email.com", ExpectedResult = 1)]
        [TestCase("FakeField", "AAADDD@email.com", ExpectedResult = 0)]
        [TestCase("Email", "FakeEmail", ExpectedResult = 0)]
        public async Task<int> Test_GetAllAccountAsync(string columnName, object value)
        {
            //Arrange
            Dictionary<string, object> dictionary = new() { { columnName, value } };
            //Act
            Response<Account> response = await _accountBusinessLogic.GetAllAccountAsync(dictionary);
            //Assert
            return response.Data.Count;
        }

        [Test]
        [TestCase("AAADDD@email.com", ExpectedResult = true)]
        [TestCase("FakeEmail", ExpectedResult = false)]
        public async Task<bool> Test_GetByEmailAsync(string email)
        {
            //Arrange
            //Act
            Response<Account> response = await _accountBusinessLogic.GetByEmailAsync(email);
            //Assert
            return response.Data.Any(value => value.Email == email);
        }

        [Test]
        public async Task Test_GetManagerListAsync()
        {
            //Arrange
            //Act
            Response<Account> response = await _accountBusinessLogic.GetManagerListAsync();
            //Assert
            Assert.IsTrue(response.Data.Any(value => 
                value.RoleId == (int)RoleEnum.Manager ||
                value.RoleId == (int)RoleEnum.Administrator
            ));
        }
    }
}