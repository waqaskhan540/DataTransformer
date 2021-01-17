using Data.Transformer.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        
        DatabaseFacade Database { get; }
    }
}
