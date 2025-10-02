using System.Text.Json.Serialization;

namespace Problem_3.Models;

public class Input 
{
    [JsonPropertyName("specversion")]
    public string SpecVersion { get; set; } = string.Empty;

    [JsonPropertyName("challengeid")]
    public string ChallengeId { get; set; } = string.Empty;

    [JsonPropertyName("metadata")]
    public Metadata Metadata { get; set; } = new();

    [JsonPropertyName("room")]
    public Room Room { get; set; } = new();

    [JsonPropertyName("workload")]
    public Workload Workload { get; set; } = new();
}

public class Metadata
{
    [JsonPropertyName("roomcount")]
    public int RoomCount { get; set; }

    [JsonPropertyName("allowedstates")]
    public List<string> AllowedStates { get; set; } = new();

    [JsonPropertyName("queuepolicy")]
    public string QueuePolicy { get; set; } = string.Empty;

    [JsonPropertyName("signchangelatency")]
    public int SignChangeLatency { get; set; }

    [JsonPropertyName("tiebreaker")]
    public List<string> TieBreaker { get; set; } = new();
}

public class Room
{
    [JsonPropertyName("initialsignstate")]
    public string InitialSignState { get; set; } = string.Empty;
}

public class Workload
{
    [JsonPropertyName("timeunit")]
    public string TimeUnit { get; set; } = string.Empty;

    [JsonPropertyName("animals")]
    public List<Animal> Animals { get; set; } = new();
}

public class Animal
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("species")]
    public string Species { get; set; } = string.Empty;

    [JsonPropertyName("arrivaltime")]
    public int ArrivalTime { get; set; }

    [JsonPropertyName("restduration")]
    public int RestDuration { get; set; }
}