using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DxMDB.DAL
{
    public class Producer
    {
        public int Id { get; set; }

        [Required]
        [NameFormatValidation]
        public string Name { get; set; }

        [Required]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [Display(Name = "Date of Birth")]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}