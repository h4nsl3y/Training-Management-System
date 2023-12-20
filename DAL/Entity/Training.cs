using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAL.Entity
{
    public class Training : ISystemEntity
    {
        [Key] 
        public int TrainingId { get; set; }
        [Required(ErrorMessage = "Training title is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Department  is required.")]
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Available seats number is required.")]
        public int SeatNumber { get; set; }
        [Required(ErrorMessage = "Training enrollment deadline is required.")]
        public DateTime Deadline { get; set; }
        [Required(ErrorMessage = "Training starting date is required.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Training ending date title is required.")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Short description title is required.")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Long description title is required.")]
        public string LongDescription { get; set; }
    }
}