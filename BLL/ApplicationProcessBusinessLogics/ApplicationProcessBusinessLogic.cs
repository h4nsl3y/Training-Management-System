using BLL.AccountTrainingBusinessLogics;
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
using System.Drawing;
using System.Windows.Media;

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

                    worksheet.Cells[1, 1].Value = $"Training title : {response.Data[0].Title}";
                    DateTime dateTime = response.Data[0].StartDate;
                    string startDate = dateTime.ToString("yyyy-MM-dd HH:mm");
                    worksheet.Cells[1, 2].Value = $"Date : {startDate}";

                    worksheet.Cells[2, 1].Value = "User Name";
                    worksheet.Cells[2, 2].Value = "Mobilenumber";
                    worksheet.Cells[2, 3].Value = "Email";
                    worksheet.Cells[2, 4].Value = "Department";
                    worksheet.Cells[2, 5].Value = "Manager Name";

                    worksheet.Cells[1,1].Style.Font.Bold = true;
                    worksheet.Cells["A3:E3"].Style.Font.Bold = true;

                    worksheet.Cells["A1:B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells["A1:B1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    worksheet.Cells["A2:E2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells["A2:E2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                    for (int i = 0; i < response.Data.Count(); i++)
                    {
                        AccountTraining data = response.Data[i];

                        worksheet.Cells[i + 3, 1].Value = data.UserName;
                        worksheet.Cells[i + 3, 2].Value = data.MobileNumber;
                        worksheet.Cells[i + 3, 3].Value = data.Email;
                        worksheet.Cells[i + 3, 4].Value = data.DepartmentName;
                        worksheet.Cells[i + 3, 5].Value = data.ManagerName;

                        worksheet.Cells.AutoFitColumns();


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
