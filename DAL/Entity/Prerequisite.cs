using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAL.Entity
{
    public class Prerequisite : ISystemEntity

    {
        [Key]
        public int PrerequisiteId {  get; set; }
        [Required(ErrorMessage = "Prerequiste description is required.")]
        public string PrerequisiteDescription {  get; set; }
    }
}