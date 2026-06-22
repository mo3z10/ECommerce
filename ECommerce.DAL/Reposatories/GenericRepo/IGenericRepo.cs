using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL.Reposatories.GenericRepo
{
    public interface IGenericRepo<Tentity> where Tentity : class
    {
        Task<IQueryable<Tentity>> GetAllAsync();

        Task<Tentity> GetByIdAsync(int Id);
        Task CreateAsync(Tentity TEntity);
        Task UpdateAsync(Tentity TEntity);
        Task DeleteAsync(Tentity tentity);
        Task SetRowVersion(Product product, byte[] rowVersion);
        
    }
}
