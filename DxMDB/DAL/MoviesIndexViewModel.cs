using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DxMDB.DAL
{
    public class MoviesIndexViewModel
    {
        public List<Movie> Movies;
        public int PageNumber { get; set; }
        public int NumberOfPages { get; set; }
        public int NumberOfRows { get; }
        public int NumberOfColumns { get; }
        public string key { get; set; }

        public MoviesIndexViewModel()
        {
            PageNumber = NumberOfPages = 0;
            NumberOfColumns = 1;
            NumberOfRows = 1;
            key = "";
        }
    }
}