using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;

namespace api.controller
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ItokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, ItokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                Username = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    message = e.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

            if(user == null)
            return Unauthorized("Invalid username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if(!result.Succeeded) return Unauthorized("Invalid password or username"); 
            return Ok (
                new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }
    }
}