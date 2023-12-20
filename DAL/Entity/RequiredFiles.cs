using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAL.Entity
{
    public class RequiredFiles : ISystemEntity
    {
        [Key] 
        public int FileId { get ; set; }
        [Required(ErrorMessage = "File name is required.")]
        public string FileName { get; set; }
        [Required(ErrorMessage = "File type is required.")]
        public string FileType { get; set; }
        [Required(ErrorMessage = "File data is required.")]
        public byte[] FileData { get; set; }
        [Required(ErrorMessage = "AccountId is required.")]
        public int AccountId { get; set; }
        [Required(ErrorMessage = "prerequisiteId data is required.")]
        public int PrerequisiteId {  get; set; }
    }
}