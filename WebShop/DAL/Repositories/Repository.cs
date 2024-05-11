using System;
using System.Linq;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DataContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<T>();
    }

    public virtual IList<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public virtual T GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public virtual void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }
}