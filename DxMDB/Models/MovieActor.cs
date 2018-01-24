using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DxMDB.Models
{
    public class MovieActor
    {
        [Key]
        [Column(Order = 1)]
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ActorId { get; set; }
        public virtual Actor Actor { get; set; }

    }
}