using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.controller
{
    [Route("api/protifolio")]
    [ApiController]
    public class PortifolioController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;

        private readonly IPortifolioRepository _portifolioRepo;
        private readonly IstockRepository _stockRepo;
        public PortifolioController(UserManager<AppUser> userManager, IstockRepository stockRepo, IPortifolioRepository portifolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portifolioRepo = portifolioRepo;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortifolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortifolio = await _portifolioRepo.GetUserPortifolioAsync(appUser);
            return Ok(userPortifolio);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortifolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock == null) return BadRequest("stock not found");

            var userPortifolio = await _portifolioRepo.GetUserPortifolioAsync(appUser);

            if (userPortifolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("stock already in portifolio");

            var portofolio = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id

            };
            await _portifolioRepo.CreateAsync(portofolio);
            if (portofolio == null)
            {
                return StatusCode(500, "could not create");
            }
            else
            {
                return Created();
            }

        }
        [HttpDelete]
        [Authorize]

        public async Task<IActionResult> DeletePortfolio(string symbol)
        {

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portifolioRepo.GetUserPortifolioAsync(appUser);

            var filterStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filterStock.Count == 1)
            {
              await _portifolioRepo.DeletePortfolio(appUser,symbol);      
            }else
            {
                return BadRequest("stock not in your Portfolio");
            }
            return Ok("nice :) ");
        }
        
    }
}