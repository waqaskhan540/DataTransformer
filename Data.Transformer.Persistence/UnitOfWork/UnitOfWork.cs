using Data.Transformer.Persistence.Context;
using Data.Transformer.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public DatabaseFacade Database => _dbContext.Database;

        private readonly DatabaseContext _dbContext;
        public UnitOfWork(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
