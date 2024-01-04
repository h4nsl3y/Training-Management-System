using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainingManagementSystem.ViewModel
{
    public class TrainingEnrollmentViewModel : ISystemEntity
    {

        public int TrainingId { get; set; }
        public string Title { get; set; }
        public int DepartmentId { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime StartDate { get; set; }
        public string ShortDescription { get; set; }

        public int EnrollmentId { get; set; }
        public int AccountId { get; set; }
        public int StateId { get; set; } = 1;
        public DateTime SubmissionDate { get; set; }

        public int PrerequisiteId { get; set; }
    }
}