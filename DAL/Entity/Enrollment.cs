using DAL.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAL.Entity
{
    public class Enrollment : ISystemEntity
    {
        [Key] 
        public int EnrollmentId { get; set; }
        [Required(ErrorMessage = "Employee information is required.")]
        public int AccountId { get; set; }
        [Required(ErrorMessage = "Training infromation are required. ")]
        public int TrainingId { get; set; }
        public int StateId { get; set; } = (int)EnrollmentStateEnum.Waiting_For_Approval;
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
    }
}