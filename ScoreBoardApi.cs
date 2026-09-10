using System.Net.Http.Json;

namespace NetworkProgrammingAssignment;

public class ScoreBoardApi
{
    private const string ScoreboardUrl = "https://networkprogramming-a3f4c-default-rtdb.europe-west1.firebasedatabase.app/scores.json";

    private static readonly HttpClient client = new HttpClient();

    public async Task<bool> SubmitScore(ScoreEntry entry)
    {
        try
        {
            Console.WriteLine("Submitting...");
            HttpResponseMessage response = await client.PostAsJsonAsync(ScoreboardUrl, entry);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("Could not connect to the server.");
            return false;
        }
    }

    public async Task<List<ScoreEntry>> GetScoreboard()
    {
        try
        {
            Console.WriteLine("Loading scoreboard...");
            Dictionary<string, ScoreEntry>? scores =
                await client.GetFromJsonAsync<Dictionary<string, ScoreEntry>>(ScoreboardUrl);

            if (scores == null || scores.Count == 0)
            {
                Console.WriteLine("No scores yet.");
                return new List<ScoreEntry>();
            }

            // Lowest ms first
            return scores.Values.OrderBy(s => s.Score).ToList();
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("Could not connect to the server.");
            return new List<ScoreEntry>();
        }
    }

    public async Task<int?> GetPersonalBest(string name)
    {
        List<ScoreEntry> scores = await GetScoreboard();
        if (scores.Count == 0) return null;

        foreach (ScoreEntry s in scores)
        {
            if (s.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                return s.Score; 
        }
        return null;
    }
}