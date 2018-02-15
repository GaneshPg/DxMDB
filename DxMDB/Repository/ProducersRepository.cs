using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DxMDB.DAL;

namespace DxMDB.Repository
{
    public class ProducersRepository
    {
        private MovieDBContext context = new MovieDBContext();

        public List<Producer> GetAllProducers()
        {
            return context.Producers.ToList();
        }

        public Producer GetProducerById(int id)
        {
            return context.Producers.Find(id);
        }

        public void AddProducer(Producer producer)
        {
            context.Producers.Add(producer);
            context.SaveChanges();
        }

        public void UpdateProducer(Producer producer)
        {
            context.Entry(producer).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteProducer(int id)
        {
            context.Entry(GetProducerById(id)).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}