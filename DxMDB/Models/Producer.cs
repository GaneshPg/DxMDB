using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DxMDB.Models
{
    public class Producer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Gender { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        public string Bio { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}