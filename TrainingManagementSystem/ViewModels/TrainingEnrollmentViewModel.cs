using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainingManagementSystem.ViewModels
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
        public int EmployeeId { get; set; }
        public int StateId { get; set; } = 1;

        public int PrerequisiteId { get; set; }
    }
}