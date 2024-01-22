using MovieApp.Models;

namespace MovieApp.Interface
{
    public interface IOmdbService
    {
        Task<MovieSearchResult> SearchMoviesAsync(string title);

        Task<MovieDetails> GetMovieDetailsAsync(string imdbId);

       /* List<string> GetLatestSearchQueries();*/
    }
}
