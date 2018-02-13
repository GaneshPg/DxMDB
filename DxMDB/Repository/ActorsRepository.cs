using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DxMDB.DAL;

namespace DxMDB.Repository
{
    public class ActorsRepository
    {
        private MovieDBContext context;

        public ActorsRepository()
        {
            this.context = new MovieDBContext();
        }

        public List<Actor> GetActors()
        {
            return context.Actors.ToList();
        }

        public Actor GetActorById(int id)
        {
            return context.Actors.Find(id);
        }

        public void AddActor(Actor actor)
        {
            context.Actors.Add(actor);
            context.SaveChanges();
        }

        public void DeleteActor(int id)
        {
            Actor actor = GetActorById(id);
            var entry = context.Entry(actor);
            if (entry.State == EntityState.Detached)
                context.Actors.Attach(actor);
            context.Actors.Remove(actor);
            context.SaveChanges();
        }

        public void UpdateActor(Actor actor)
        {
            context.Entry(actor).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}