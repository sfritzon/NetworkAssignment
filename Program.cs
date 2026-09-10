using System.Diagnostics;
using NetworkProgrammingAssignment;

const int minimumTime = 2;
const int maximumTime = 6;

ScoreBoardApi api = new ScoreBoardApi();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("|------ ONLINE SCOREBOARD ------|");
    Console.WriteLine();
    Console.WriteLine("1. Play & Submit Score");
    Console.WriteLine("2. View Scoreboard");
    Console.WriteLine("3. View Top 10");
    Console.WriteLine("4. My Personal Best");
    Console.WriteLine("5. Exit");
    Console.WriteLine();
    Console.Write("Choose: ");

    string? choice = Console.ReadLine();
    Console.WriteLine();

    if (choice == "1")
    {
        await PlayAndSubmit(api, minimumTime, maximumTime);
    }
    else if (choice == "2")
    {
        await ShowScoreboard(api, "|------ LEADERBOARD ------");
    }
    else if (choice == "3")
    {
        await ShowScoreboard(api, "|------ TOP 10 ------|", 10);
    }
    else if (choice == "4")
    {
        await ShowPersonalBest(api);
    }
    else if (choice == "5")
    {
        Console.WriteLine("Goodbye!");
        return;
    }
    else
    {
        Console.WriteLine("Invalid choice, please pick 1-5.");
    }
}

static async Task PlayAndSubmit(ScoreBoardApi api, int minSeconds, int maxSeconds)
{
    Console.Write("Name: ");
    string? name = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(name))
    {
        Console.WriteLine("Name cannot be blank.");
        return;
    }

    Console.WriteLine("Press 1 to start...");
    if (Console.ReadKey(true).Key != ConsoleKey.D1)
    {
        Console.WriteLine("Cancelled.");
        return;
    }

    Console.WriteLine("Get ready...");
    await Task.Delay(Random.Shared.Next(minSeconds, maxSeconds) * 1000);
    Console.WriteLine("Press SPACE now!");

    Stopwatch stopwatch = Stopwatch.StartNew();
    while (Console.KeyAvailable) Console.ReadKey(true);
    while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }
    stopwatch.Stop();

    int score = (int)stopwatch.ElapsedMilliseconds;
    Console.WriteLine($"Reaction time: {score} ms");

    ScoreEntry entry = new ScoreEntry { Name = name, Score = score };
    bool ok = await api.SubmitScore(entry);
    if (ok) Console.WriteLine("Score submitted!");
}

static async Task ShowScoreboard(ScoreBoardApi api, string header, int limit = int.MaxValue)
{
    List<ScoreEntry> scores = await api.GetScoreboard();
    if (scores.Count == 0) return;

    Console.WriteLine(header);
    Console.WriteLine();

    int rank = 1;
    foreach (ScoreEntry s in scores)
    {
        if (rank > limit) break;
        Console.WriteLine($"{rank}. {s.Name,-12} {s.Score} ms");
        rank++;
    }
}

static async Task ShowPersonalBest(ScoreBoardApi api)
{
    Console.Write("Player name: ");
    string? name = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(name))
    {
        Console.WriteLine("Name cannot be blank.");
        return;
    }

    int? best = await api.GetPersonalBest(name);
    if (best.HasValue)
        Console.WriteLine($"{name}'s best reaction time: {best.Value} ms");
    else
        Console.WriteLine($"No scores found for '{name}'.");
}