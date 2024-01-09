using DAL.Entity;
using DAL.Logger;
using DAL.Repository.EnrollmentRepositories;
using DAL.Repository.GenericRepositories;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AutomaticProcess
{
    public class BackgroundJob: IJob
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        public BackgroundJob(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Response<Enrollment> enrollmentResponse = await _enrollmentRepository.GetEnrollmentIdByDeadline();
            foreach (Enrollment enrollment in enrollmentResponse.Data)
            {
                await _enrollmentRepository.SelectTrainingParticipants(enrollment);
            }
        }
    }
}
