﻿using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.EnrollmentRepositories
{
    public interface IEnrollmentRepository
    {
        Task<Response<Enrollment>> GetEnrollmentByEmailAsync(int accountId);
        Task<Response<Enrollment>> GetEnrollmentIdByDeadline();
        Task SelectTrainingParticipants(int trainingId);
    }
}
