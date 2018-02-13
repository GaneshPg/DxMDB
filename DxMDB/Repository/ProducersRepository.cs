using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DxMDB.DAL;


namespace DxMDB.Repository
{
    public class ProducersRepository { 
    private MovieDBContext context;

    public ProducersRepository()
    {
        this.context = new MovieDBContext();
    }

    public List<Producer> GetProducers()
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

    public void DeleteProducer(int id)
    {
            Producer producer = GetProducerById(id);
            var entry = context.Entry(producer);
            if (entry.State == EntityState.Detached)
                context.Producers.Attach(producer);
            context.Producers.Remove(producer);
            context.SaveChanges();
            context.SaveChanges();
    }

    public void UpdateProducer(Producer producer)
    {
        context.Entry(producer).State = EntityState.Modified;
        context.SaveChanges();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
}