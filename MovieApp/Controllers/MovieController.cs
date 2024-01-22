using Microsoft.AspNetCore.Mvc;
using MovieApp.Interface;
using MovieApp.Services;

namespace MovieApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IOmdbService _omdbService;
        private readonly SearchHistoryService _searchHistoryService;

        public MovieController(IOmdbService omdbService, SearchHistoryService searchHistoryService)
        {
            _omdbService = omdbService;
            _searchHistoryService = searchHistoryService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies([FromQuery] string title)
        {
            var result = await _omdbService.SearchMoviesAsync(title);
            return Ok(result);
        }

        [HttpGet("{imdbId}")]
        public async Task<IActionResult> GetMovieDetails(string imdbId)
        {
            var result = await _omdbService.GetMovieDetailsAsync(imdbId);
            return Ok(result);
        }

        [HttpGet("latest-queries")]
        public IActionResult GetLatestSearchQueries()
        {
            var latestQueries = _searchHistoryService.GetLatestSearchQueries();
            return Ok(latestQueries);
        }
    }
}
