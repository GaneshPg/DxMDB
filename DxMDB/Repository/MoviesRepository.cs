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
        private MovieDBContext context;

        public MoviesRepository()
        {
            this.context = new MovieDBContext();
        }

        public int GetMovieCount()
        {
            return context.Movies.Count();
        }

        public List<Movie> GetMovies()
        {
            return context.Movies.Include(movie => movie.Producer).ToList();
        }

        public Movie GetMovieById(int id)
        {
            return context.Movies.Find(id);
        }

        public void AddMovie(Movie movie, List<int> selectedActorsList)
        {
            context.Movies.Add(movie);
            context.SaveChanges();
            foreach (int currentId in selectedActorsList)
            {
                movie.Actors.Add(new Actor { Id = currentId });
            }
            context.Entry(movie).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteMovie(int id)
        {
            Movie movie = GetMovieById(id);
            var entry = context.Entry(movie);
            if (entry.State == EntityState.Detached)
                context.Movies.Attach(movie);
            context.Movies.Remove(movie);
            context.SaveChanges();
        }

        public void UpdateMovie(Movie movie, List<int> selectedActorsList)
        {
            //context.Entry(movie).State = EntityState.Modified;
            DeleteAllMovieActors(movie.Id);
            foreach (int currentId in selectedActorsList)
            {
                movie.Actors.Add(new Actor { Id = currentId });
            }
            context.SaveChanges();
        }

        public void AddNewMovieActor(Movie movie, int actorId)
        {
            context.Movies.Add(movie);
            Actor actor = new Actor { Id = actorId };
            context.Actors.Attach(actor);
            movie.Actors.Add(actor);
            context.SaveChanges();
        }

        public void AddMovieActor(int movieId, int actorId)
        {
            Movie movie = GetMovieById(movieId);
            Actor actor = context.Actors.Find(actorId);
            context.Actors.Attach(actor);
            movie.Actors.Add(actor);
            context.SaveChanges();
        }

        public void DeleteAllMovieActors(int movieId)
        {
            Movie movie = GetMovieById(movieId);
            movie.Actors.Clear();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}