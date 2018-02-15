using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DxMDB.DAL
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name="Year of Release")]
        [Range(1950,2030)]
        public int YearOfRelease { get; set; }

        [Display(Name="Poster Image")]
        public string PosterFilePath { get; set; }

        [DataType(DataType.MultilineText)]
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