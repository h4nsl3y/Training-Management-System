using BLL.NotificationBusinessLogics;
using DAL.Entity;
using DAL.Enum;
using DAL.Logger;
using DAL.Repository.ApplicationProcessRepositories;
using DAL.Repository.EnrollmentRepositories;
using DAL.Repository.GenericRepositories;
using DAL.Repository.TrainingRepositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL.AutomaticProcess
{
    public class BackgroundJob: IJob
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly INotificationBusinessLogic _notificationBusinessLogic;
        private readonly IApplicationProcessRepository _applicationProcessRepository;
        public BackgroundJob(IEnrollmentRepository enrollmentRepository, 
                            INotificationBusinessLogic notificationBusinessLogic, 
                            IApplicationProcessRepository applicationProcessRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _notificationBusinessLogic = notificationBusinessLogic;
            _applicationProcessRepository = applicationProcessRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Response<Enrollment> enrollmentResponse = await _enrollmentRepository.GetTrainingByDeadlineAsync();
            if (enrollmentResponse.Data.Any()) 
            {
                foreach (Enrollment enrollment in enrollmentResponse.Data)
                {
                    await _enrollmentRepository.SelectTrainingParticipants(enrollment.TrainingId);
                };
                await SetNotification();
            }
        }
        public async Task SetNotification()
        {
            var response = await _applicationProcessRepository.GetAccountTraining();
            foreach (AccountTraining accountTraining in response.Data)
            {
                await _notificationBusinessLogic.AddNotificationAsync(accountTraining.AccountId, accountTraining.StateId, accountTraining.Title,"",accountTraining.Email);
            }
        }
    }
}
