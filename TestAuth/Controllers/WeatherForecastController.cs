using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularShop.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestAuth.Models;

namespace TestAuth.Controllers
{
    [Authorize(Policy = "Member")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMfaService _mfaService;
        private readonly UserManager<User> _userManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMfaService mfaService, UserManager<User> userManager)
        {
            _logger = logger;
            _mfaService = mfaService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            
            var userObj = HttpContext.User;
            var user = await _userManager.GetUserAsync(userObj);
            var key = await _mfaService.GetKeyAsync(user);


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Test")]
        public IActionResult Test([FromBody] Test Email)
        {
            if (Email.Email == "connordownes@smasltd.com")
            {
                return Ok(true);
            }
            return Ok(false);
        }
    }

    public class Test 
    {
        public string Email { get; set; }
    }
}
