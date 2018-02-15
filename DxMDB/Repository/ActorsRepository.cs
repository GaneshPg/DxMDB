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
        private MovieDBContext context = new MovieDBContext();

        public List<Actor> GetAllActors()
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

        public void UpdateActor(Actor actor)
        {
            context.Entry(actor).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteActor(int id)
        {
            context.Entry(GetActorById(id)).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}