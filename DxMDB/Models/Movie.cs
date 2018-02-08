using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Display(Name="Year of Release")]
        public int Yor { get; set; }

        [Display(Name="Poster Image")]
        public string PosterFilePath { get; set; }

        public string Plot { get; set; }

        [Required]
        public int ProducerId { get; set; }

        public virtual Producer Producer { get; set; }

        public virtual ICollection<Actor> Actors { get; set; }

        public Movie()
        {
            Actors = new HashSet<Actor>();
        }
    }
}