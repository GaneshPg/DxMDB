using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DxMDB.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        [NameFormatValidation]
        public string Name { get; set; }

        public string Gender { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime DOB { get; set; }

        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public Actor()
        {
            Movies = new HashSet<Movie>();
        }
    }
}