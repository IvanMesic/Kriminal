using System;
using System.Linq;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}