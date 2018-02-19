using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DxMDB.DAL;

namespace DxMDB.Repository
{
    public class MoviesRepository
    {
        private MovieDBContext context = new MovieDBContext();

        public List<Movie> GetAllMovies()
        {
            return context.Movies.Include(movie => movie.Producer).ToList();
        }

        public Movie GetMovieById(int id)
        {
            return context.Movies.Find(id);
        }

        public int GetCount()
        {
            return context.Movies.Count();
        }

        public void SetActors(Movie movie, List<int> selectedActorsList)
        {
            movie.Actors.Clear();
            foreach(int actorId in selectedActorsList)
            {
                Actor actor = new Actor { Id = actorId };
                context.Actors.Attach(actor);
                movie.Actors.Add(actor);
            }
        }

        public void AddMovie(Movie movie, List<int> selectedActorsList)
        {
            SetActors(movie, selectedActorsList);
            context.Movies.Add(movie);
            context.SaveChanges();
        }

        public void UpdateMovie(Movie movie, List<int> selectedActorsList, Movie attachedMovie)
        {
            SetActors(movie, selectedActorsList);
            //if (context.Entry(movie).State == EntityState.Detached)
            //    context.Movies.Attach(movie);
            context.Entry(attachedMovie).State = EntityState.Detached;
            context.Entry(movie).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteMovie(int id)
        {
            context.Entry(GetMovieById(id)).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}