using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.interfaces
{
    public interface IPortifolioRepository
    {
        Task<List<Stock>> GetUserPortifolioAsync(AppUser user);

        Task<Portfolio> CreateAsync(Portfolio portfolio);

        Task<Portfolio> DeletePortfolio(AppUser appUser,string symbol);
    }
}