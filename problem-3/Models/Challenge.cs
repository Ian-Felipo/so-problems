using System.Text.Json.Serialization;

namespace Problem_3.Models;

public class Challenge
{
    [JsonPropertyName("specversion")]
    public string SpecVersion { get; set; } = null!;
    [JsonPropertyName("challengeid")]
    public string ChallengeId { get; set; } = null!;
    [JsonPropertyName("metadata")]
    public Metadata Metadata { get; set; } = null!;
    [JsonPropertyName("room")]
    public Room Room { get; set; } = null!;
    [JsonPropertyName("workload")]
    public Workload Workload { get; set; } = null!;
}

public class Metadata
{
    [JsonPropertyName("room count")]
    public int RoomCount { get; set; }
    [JsonPropertyName("allowed states")]
    public List<string> AllowedStates { get; set; } = new();
    [JsonPropertyName("queue policy")]
    public string QueuePolicy { get; set; } = null!;
    [JsonPropertyName("signchange latency")]
    public int SignChangeLatency { get; set; }
    [JsonPropertyName("tiebreaker")]
    public List<string> TieBreaker { get; set; } = new();
}

public class Room
{
    [JsonPropertyName("initial signstate")]
    public string InitialSignState { get; set; } = null!;
}

public class Workload
{
    [JsonPropertyName("time unit")]
    public string TimeUnit { get; set; } = null!;
    [JsonPropertyName("animals")]
    public List<Animal> Animals { get; set; } = new();
}

public class Animal
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("species")]
    public string Species { get; set; } = null!;
    [JsonPropertyName("arrival time")]
    public int ArrivalTime { get; set; }
    [JsonPropertyName("rest duration")]
    public int RestDuration { get; set; }
}