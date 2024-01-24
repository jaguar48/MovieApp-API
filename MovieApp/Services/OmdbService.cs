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
    private readonly string _baseAddress;
    private readonly SearchHistoryService _searchHistoryService;

    public OmdbService(HttpClient httpClient, IConfiguration configuration, SearchHistoryService searchHistoryService)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _omdbApiKey = configuration["Api:OmdbApiKey"];
        _baseAddress = configuration["BaseAddress"];

        _searchHistoryService = searchHistoryService;

        _httpClient.BaseAddress = new Uri(_baseAddress);
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

   
}
