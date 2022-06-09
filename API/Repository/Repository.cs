using API.Context;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class Repository<Context, Entity, Key> : IRepository<Entity, Key>
        where Entity : class
        where Context : MyContext
    {
        private readonly MyContext context;
        private readonly DbSet<Entity> entities;

        public Repository(MyContext _context)
        {
            this.context = _context;
            entities = context.Set<Entity>();
        }

        public IEnumerable<Entity> Get()
        {
            return entities.ToList();
        }

        public Entity Get(Key key)
        {
            return entities.Find(key);
        }

        public int Insert(Entity ent)
        {
            int insertRes = 400;

            try
            {
                entities.Add(ent);
                insertRes = context.SaveChanges();
                insertRes = 200;
            }
            catch
            {
                return insertRes;
            }
            return insertRes;
        }

        public int Update(Entity ent)
        {
            int updateRes = 400;

            try
            {
                context.Entry(ent).State = EntityState.Modified;
                updateRes = context.SaveChanges();
                updateRes = 200;
            }
            catch 
            {
                return  updateRes;
            }

            return updateRes;
        }

        public int Delete(Key key)
        {
            int delRes = 400;
            Entity ent = entities.Find(key);

            if (ent == null) return delRes;

            try
            {
                entities.Remove(ent);
                delRes = context.SaveChanges();
                delRes = 200;
            }
            catch
            {
                return delRes; //Return 400 - Error
            }

            return delRes;
        }
    }
}
