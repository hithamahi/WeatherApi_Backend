using Microsoft.AspNetCore.Mvc;
using WeatherApi.Model;
using WeatherApi.Services;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavouriteController : ControllerBase
    {
        private readonly FavouriteService _favouriteService;

        public FavouriteController(FavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        
        //Different user - different favourites logic

        [HttpPost("add")]
        public async Task<IActionResult> AddFavourite(string email, string city)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(city))
                return BadRequest("Email and city are required.");

            var user = await _favouriteService.GetByEmailAsync(email);

            if (user == null)
            {
                user = new UserFavourites
                {
                    Email = email,
                    Favourites = new List<string> { city }
                };
                await _favouriteService.InsertUserFavouriteAsync(user);
            }
            else if (!user.Favourites.Contains(city, StringComparer.OrdinalIgnoreCase))
            {
                user.Favourites.Add(city);
                await _favouriteService.ReplaceUserFavouriteAsync(email, user);
            }

            return Ok(user.Favourites);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> DeleteFavourite(string email, string city)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(city))
                return BadRequest("Email and city are required.");

            var user = await _favouriteService.GetByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var cityRemoved = user.Favourites.RemoveAll(c => string.Equals(c, city, StringComparison.OrdinalIgnoreCase)) > 0;

            if (cityRemoved)
            {
                await _favouriteService.ReplaceUserFavouriteAsync(email, user);
                return Ok($"{city} removed from favourites.");
            }
            else
            {
                return NotFound($"{city} not found in user's favourites.");
            }
        }


        [HttpGet("get")]
        public async Task<IActionResult> GetFavourites(string email)
        {
            var user = await _favouriteService.GetByEmailAsync(email);
            if (user == null)
            {
                return Ok(new List<string>());
            }
            return Ok(user.Favourites);
        }




    }
}