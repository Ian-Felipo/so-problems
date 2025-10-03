using Problem_3.Models;
using System.Text.Json;

namespace Problem_3;

class Program
{
    public static void Main(string[] args)
    {
        var path = args[0];
        
        var challengeJson = File.ReadAllText(path);
        
        var challenge = JsonSerializer.Deserialize<Challenge>(challengeJson);
        
        var animals = challenge!.Workload.Animals;
        
        var animalsOrdainedByArrivalTime = animals.OrderBy(a => a.ArrivalTime).ToList();

        var speciesGroups = new List<List<Animal>>();
        List<Animal>? currentGroup = null;
        string? currentSpecies = null;

        foreach (var animal in animalsOrdainedByArrivalTime)
        {
            if (currentGroup == null || animal.Species != currentSpecies)
            {
                currentGroup = new List<Animal>();
                speciesGroups.Add(currentGroup);
                currentSpecies = animal.Species;
            }
            currentGroup.Add(animal);
        }

        var startTime = DateTime.UtcNow;
        
        foreach (var group in speciesGroups)
        {
            var threads = new List<Thread>();

            foreach (var animal in group)
            {

                var delay = TimeSpan.FromSeconds(animal.ArrivalTime) - (DateTime.UtcNow - startTime);
                if (delay > TimeSpan.Zero)
                {
                    Thread.Sleep(delay);
                }

                var thread = animal.Species.Equals("dog", StringComparison.OrdinalIgnoreCase)
                    ? new Thread(() => Dog(animal.Id, animal.ArrivalTime))
                    : new Thread(() => Cat(animal.Id, animal.ArrivalTime));

                threads.Add(thread);
                thread.Start();
            }

            foreach (var t in threads)
            {
                t.Join();
            }
        }
    }

    public static void Dog(string id, int restDuration)
    {
        Console.WriteLine($"O CACHORRO {id} entrou na sala de repouso, au au au ...");
        Thread.Sleep(restDuration);
        Console.WriteLine($"O CACHORRO {id} saiu da sala de repouso, au au au ...");
    }

    public static void Cat(string id, int restDuration)
    {
        Console.WriteLine($"O GATO {id} entrou na sala de repouso, meow meow meow ...");
        Thread.Sleep(restDuration);
        Console.WriteLine($"O GATO {id} saiu da sala de repouso, meow meow meow ...");
    }
}