using Microsoft.EntityFrameworkCore;
using OkrManager.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OKRManager.Data;


namespace OkrManager.Repositories
{

    public class Repository<T> : IRepository<T> where T : class
    {
        //private readonly ApplicationDbContext _dbContext;
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        private DbSet<T> _dbSet;
        
        public Repository(/*ApplicationDbContext dbContext*/)
        {
            //_dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public void Create(T entidade)
        {
            _dbSet.Add(entidade);
            _dbContext.SaveChanges();
        }

        public void Update(T entidade)
        {
            _dbSet.Update(entidade);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            T? entidade = GetById(id);
            if (entidade != null)
            {
                _dbSet.Remove(entidade);
                _dbContext.SaveChanges();
            }
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }
        
        
        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }
        
    }
}
