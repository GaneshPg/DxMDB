using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DxMDB.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Yor { get; set; }
        public string Plot { get; set; }
        [Required]
        public int ProducerId { get; set; }
        public virtual Producer Producer { get; set; }
        public virtual ICollection<MovieActor> MovieActors { get; set; }
    }
}