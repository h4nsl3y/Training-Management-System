﻿using BLL.AccountTrainingBusinessLogics;
using DAL.Entity;
using DAL.Logger;
using DAL.Repository.ApplicationProcessRepositories;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ApplicationProcessBusinessLogics
{
    public class ApplicationProcessBusinessLogic : IApplicationProcessBusinessLogic
    {
        private readonly ILogger _logger;
        private readonly IApplicationProcessRepository _applicationProcessRepository;
        public ApplicationProcessBusinessLogic(ILogger logger, IApplicationProcessRepository applicationProcessRepository)
        {
            _logger = logger;
            _applicationProcessRepository = applicationProcessRepository;
        }
        public async Task<Response<byte[]>> CreateExcelFile(int trainingId)
        {
            try
            {
                Response<AccountTraining> response = await _applicationProcessRepository.GetAccountTrainingData(trainingId);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Data");

                    worksheet.Cells[1, 1].Value = response.Data[0].Title;
                    worksheet.Cells[1, 2].Value = response.Data[0].StartDate;
                    // Define headers
                    worksheet.Cells[2, 1].Value = "UserName";
                    worksheet.Cells[2, 2].Value = "MobileNumber";
                    worksheet.Cells[2, 3].Value = "Email";
                    worksheet.Cells[2, 3].Value = "DepartmentId";
                    worksheet.Cells[2, 3].Value = "ManagerName";
                    worksheet.Cells[2, 3].Value = "Title";
                    worksheet.Cells[2, 3].Value = "StartDate";

                    for (int i = 0; i < response.Data.Count(); i++)
                    {
                        AccountTraining data = response.Data[i];

                        worksheet.Cells[i + 2, 1].Value = data.UserName;
                        worksheet.Cells[i + 2, 2].Value = data.MobileNumber;
                        worksheet.Cells[i + 2, 3].Value = data.Email;
                        worksheet.Cells[i + 2, 4].Value = data.DepartmentId;
                        worksheet.Cells[i + 2, 5].Value = data.ManagerName;
                    }
                    return new Response<byte[]> { Success = true, Data = { excelPackage.GetAsByteArray() } }; 
                }
            }

            
            catch (Exception exception)
            {
                _logger.Log(exception);
                return new Response<byte[]> { Success = false , Message = "File could not be generated"};
            }
        }

        
        public bool SendEmail(string Subject, string Body, string recipientEmail)
        {
            string senderEmail = "TrainingManagementSystem@ceridian.com";
            var smtpClent = new SmtpClient("relay.ceridian.com")
            {
                Port = 25,
                EnableSsl = true,
                UseDefaultCredentials = true,
            };
            var mailMessage = new MailMessage(senderEmail, recipientEmail)
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = true
            };
            try
            {
                smtpClent.Send(mailMessage);
                return true;
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return false;
            }
        }

    }
}
