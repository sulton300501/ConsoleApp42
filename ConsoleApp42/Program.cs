using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.Write("Enter the movie title to search: ");
        string query = Console.ReadLine();

        string apiKey = "YOUR_OMDB_API_KEY";
        await SearchMovie(apiKey, query);
    }

    static async Task SearchMovie(string apiKey, string query)
    {
        string baseUrl = "http://www.omdbapi.com/";
        string endpoint = $"?apikey={apiKey}&s={query}";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(baseUrl + endpoint);

            if (response.IsSuccessStatusCode)
            {
                MovieListResult result = await response.Content.ReadFromJsonAsync<MovieListResult>();

                if (result.Response == "True")
                {
                    foreach (var movie in result.Search)
                    {
                        Console.WriteLine($"Title: {movie.Title}, Year: {movie.Year}, Type: {movie.Type}, IMDb ID: {movie.ImdbID}");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {result.Error}");
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
    }
}

class MovieListResult
{
    public string Response { get; set; }
    public string Error { get; set; }
    public Movie[] Search { get; set; }
}

class Movie
{
    public string Title { get; set; }
    public string Year { get; set; }
    public string Type { get; set; }
    public string ImdbID { get; set; }
}
