using BLL.NotificationBusinessLogics;
using DAL.Entity;
using DAL.Enum;
using DAL.Logger;
using DAL.Repository.ApplicationProcessRepositories;
using DAL.Repository.EnrollmentRepositories;
using DAL.Repository.GenericRepositories;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BLL.EnrollmentBusinesslogics
{
    public class EnrollmentBusinesslogic : IEnrollmentBusinessLogic
    {
        private readonly IGenericRepository<Enrollment> _genericRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IApplicationProcessRepository _applicationProcessRepository;
        private readonly ILogger _logger;
        private readonly Response<bool> _resultBoolError;
        private readonly Response<Enrollment> _resultError;
        public EnrollmentBusinesslogic(IEnrollmentRepository enrollmenyRepository,
            IApplicationProcessRepository applicationProcessRepository, 
            IGenericRepository<Enrollment> genericRepository, ILogger logger)
        {
            _enrollmentRepository = enrollmenyRepository;
            _applicationProcessRepository = applicationProcessRepository;
            _genericRepository = genericRepository;
            _logger = logger;
            _resultBoolError = new Response<bool> { Success = false, Message = "an Error has been encounter" };
            _resultError = new Response<Enrollment> { Success = false, Message = "an Error has been encounter" };
        }
        public async Task<Response<bool>> AddEnrollmentAsync(Enrollment enrollment, string trainingTitle, string email, string comment)
        {
            try
            {
                return await _genericRepository.AddAsync(enrollment);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultBoolError;
            }
        }
        public async Task<Response<Enrollment>> GetEnrollmentByAccountAsync(int accountId)
        {
            try
            {
                return await _enrollmentRepository.GetEnrollmentByEmailAsync(accountId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
                return _resultError;
            }
        }
        public async Task<Response<byte[]>> CreateExcelFile(int trainingId)
        {
            try
            {
                Response<AccountTraining> response = await _applicationProcessRepository.GetAccountTrainingByTraining(trainingId);
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
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells["A2:E2"].Style.Font.Bold = true;
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
                return new Response<byte[]> { Success = false, Message = "File could not be generated" };
            }
        }
        public async Task RenewSelection(Enrollment enrollment)
        {
            try
            {
                await _enrollmentRepository.SelectTrainingParticipants(enrollment.TrainingId);
            }
            catch (Exception exception)
            {
                _logger.Log(exception);
            }
        }
    }
}