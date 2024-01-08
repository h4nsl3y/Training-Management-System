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

namespace TestProject
{
    public class UnitTest
    {
        private Mock<IAccountRepository> _stubAccountRepository;
        private Mock<ILogger> _stubLogger;
        private AccountBusinessLogic _accountBusinessLogic;

        [SetUp]
        public void Setup()
        {
            Random rnd = new Random(32000);
            List<Account> Accounts = new List<Account>()
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
            } ;

            _stubAccountRepository = new Mock<IAccountRepository>();
            _stubLogger = new Mock<ILogger>();

            _stubAccountRepository.Setup(accountRepository => accountRepository.AddAsync(It.IsAny<Account>())).ReturnsAsync(new Response<bool> { Success = true , Data = { true } });
            _stubAccountRepository.Setup(accountRepository => accountRepository.AuthenticateAsync(It.IsAny<string>())).
                ReturnsAsync((string userEmail) => {
                    return new Response<Account>() { Success = true, Data = { Accounts.FirstOrDefault(account => account.Email == userEmail) } };
                });
            _stubAccountRepository.Setup(accountRepository => accountRepository.DuplicatedAsync(It.IsAny<Dictionary<string, object>>())).
                ReturnsAsync((Dictionary<string, object> conditions) =>
                {
                    Response<bool> result  = new Response<bool>(){ Success = true };
                    foreach (var condition in conditions)
                    {
                        bool isDuplicate = Accounts.Any(account => account.GetType().GetProperty(condition.Key).GetValue(account) == condition.Value);
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
                        Data = { Accounts.First(account => account.GetType().GetProperty(attribute, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(account) == value) },
                    };
                });
            _stubAccountRepository.Setup(accountRepository => accountRepository.GetAllAsync (It.IsAny<Dictionary<string, object>>())).
                ReturnsAsync((Dictionary<string, object> dictionary) => {
                    string attribute = dictionary.Keys.First();
                    var value = dictionary.Values.First();

                    return new Response<Account>()
                    {
                        Success = true,
                        Data = Accounts.Where(account => account.GetType().GetProperty(attribute, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(account) == value).ToList() ,
                    };
                });
            _stubAccountRepository.Setup(accountRepository => accountRepository.GetActiveRequestEmployeeAsync(It.IsAny<int>())).
                ReturnsAsync((int managerId) =>
                {
                    return new Response<Account>()
                    {
                        Success = true,
                        Data = Accounts.Where(account => account.ManagerId == managerId).ToList(),
                    };
                });
            _stubAccountRepository.Setup(accountRepository => accountRepository.GetManagerListAsync()).
                ReturnsAsync(new Response<Account>() { Success = true, Data = Accounts.Where(account => account.RoleId == (int)RoleEnum.Manager).ToList() });

            _accountBusinessLogic = new AccountBusinessLogic(_stubAccountRepository.Object, _stubLogger.Object);
        }

        [Test]
        public async Task Test_AuthenticateUser()
        {
            //Arrange
            string employeeEmail = "AAADDD@email.com";
            string password = "employee";
            //Act
            Response<bool> response = await _accountBusinessLogic.AuthenticatedAsync(employeeEmail, password);
            //Assert
            Assert.IsTrue(response.Data.First());

        }

    }
}