using BLL.NotificationBusinessLogics;
using DAL.Entity;
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
        private readonly ITrainingRepository _trainingRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly INotificationBusinessLogic _notificationBusinessLogic;
        private readonly IApplicationProcessRepository _applicationProcessRepository;
        public BackgroundJob(ITrainingRepository trainingRepository,
                            IEnrollmentRepository enrollmentRepository, 
                            INotificationBusinessLogic notificationBusinessLogic, 
                            IApplicationProcessRepository applicationProcessRepository)
        {
            _trainingRepository = trainingRepository;
            _enrollmentRepository = enrollmentRepository;
            _notificationBusinessLogic = notificationBusinessLogic;
            _applicationProcessRepository = applicationProcessRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Response<Training> trainingResponse = await _trainingRepository.GetTrainingByDeadlineAsync();
            if (trainingResponse.Data.Any()) 
            {
                foreach (Training training in trainingResponse.Data)
                {
                    await _enrollmentRepository.SelectTrainingParticipants(training.TrainingId);
                };
                await SetNotification();
            }
        }
        public async Task SetNotification()
        {
            var response = await _applicationProcessRepository.GetAccountTraining();

            foreach (AccountTraining accountTraining in response.Data)
            {
                await _notificationBusinessLogic.AddNotificationAsync(
                   new Notification
                   {
                       AccountId = accountTraining.AccountId,
                       Subject = "Confirmation",
                       Body = $"Your enrollment reguarding the training \n: '{accountTraining.Title}' \nhas been confirmed.\n\nThis message is computer-generated , please do not reply."
                   },
                   accountTraining.Email);
            }
        }
    }
}
