using Microsoft.Extensions.Options;
using MovieApp;
using MovieApp.Interface; // Import the interface
using MovieApp.Models;
using MovieApp.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class OmdbService : IOmdbService
{
    private readonly HttpClient _httpClient;
    private readonly string _omdbApiKey;
    private readonly SearchHistoryService _searchHistoryService;

    public OmdbService(HttpClient httpClient, IOptions<ApiSettings> apiSettings, SearchHistoryService searchHistoryService)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _omdbApiKey = apiSettings.Value.OmdbApiKey;
        _searchHistoryService = searchHistoryService;

        _httpClient.BaseAddress = new Uri("http://www.omdbapi.com/");
    }

    public async Task<MovieSearchResult> SearchMoviesAsync(string title)
    {
        _searchHistoryService.SaveLatestSearchQuery(title);

        var apiUrl = $"?s={Uri.EscapeDataString(title)}&apikey={_omdbApiKey}";
        var response = await _httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<MovieSearchResult>(responseContent);

        return result;
    }

    public async Task<MovieDetails> GetMovieDetailsAsync(string imdbId)
    {
        var apiUrl = $"?i={Uri.EscapeDataString(imdbId)}&apikey={_omdbApiKey}";
        var response = await _httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<MovieDetails>(responseContent);

        return result;
    }

   /* private void SaveLatestSearchQuery(string query)
    {
        if (_latestSearchQueries.Count >= 5)
        {
            _latestSearchQueries.RemoveAt(0);
        }

        _latestSearchQueries.Add(query);
    }

    public List<string> GetLatestSearchQueries()
    {
        return _latestSearchQueries;
    }*/
}
