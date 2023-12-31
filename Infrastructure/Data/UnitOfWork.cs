﻿using Core.Entities;
using Core.Interfaces;
using System.Collections;

namespace Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly StoreContext _context;
    private Dictionary<string, object> _repositories = new(); 
    public UnitOfWork(StoreContext context) => _context = context;

    public void Dispose() => _context.Dispose();
    public bool HasChanges() => _context.ChangeTracker.HasChanges();
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    public IGenericRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T).Name;

        if(!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(T)), _context);
            _repositories.Add(type, repositoryInstance!);
        }

        return (IGenericRepository<T>)_repositories[type];
    }
}
