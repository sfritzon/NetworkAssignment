# NetworkProgramming

# Reaction Time Game

A console-based reaction time game written in C# that lets players compete on a shared online scoreboard backed by Firebase Realtime Database.

## How to Play

1. Run the program and choose **Play & Submit Score** from the menu
2. Enter your name
3. Press `1` to start
4. Wait for the **"Press SPACE now!"** prompt (the delay is randomized between 2–6 seconds, so you can't cheat!)
5. Hit `SPACE` as fast as you can
6. Your reaction time in milliseconds is submitted to the online scoreboard

Lower is better!

## Features

- **Reaction timer:** with millisecond precision using `Stopwatch`
- **Online scoreboard:** scores are stored in and retrieved from Firebase
- **Leaderboard:** view all scores sorted fastest-first
- **Top 10:** see just the ten best times
- **Personal best lookup:** find your fastest time by name (case-insensitive)
- **Error handling:** friendly message if the server can't be reached

## Project Structure

```
├── Program.cs        # Main menu, game loop, and console UI
├── ScoreBoardApi.cs  # HTTP client for the Firebase REST API
└── ScoreEntry.cs     # Data model (player name + score in ms)
```

## Tech Stack

- **C#** with top-level statements
- **`System.Net.Http.Json`** for JSON serialization over HTTP (`PostAsJsonAsync` / `GetFromJsonAsync`)
- **Firebase Realtime Database** (REST API) as the backend
