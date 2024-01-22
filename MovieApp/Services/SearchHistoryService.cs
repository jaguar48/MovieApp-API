namespace MovieApp.Services
{
    public class SearchHistoryService
    {
        private List<string> _latestSearchQueries = new List<string>();

        public void SaveLatestSearchQuery(string query)
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
        }
    }
}
