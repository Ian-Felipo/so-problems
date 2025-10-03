using Problem_3.Models;
using System.Text.Json;

namespace Problem_3;

class Program
{
    private static ERoomState _roomState = ERoomState.Empty;
    private static Mutex _roomStateMutex = new();
    private static int _dogCount;
    private static int _catCount;
    
    public static void Main(string[] args)
    {
        var path = args[0];
        var challengeJson = File.ReadAllText(path);
        var challenge = JsonSerializer.Deserialize<Challenge>(challengeJson);
        var animals = challenge!.Workload.Animals;
        var animalsSortedByArrivalTime = animals.OrderBy(animal => animal.ArrivalTime);
        var startTime = DateTime.UtcNow;
        
        foreach (var animal in animalsSortedByArrivalTime)
        {
            var delay = TimeSpan.FromSeconds(animal.ArrivalTime) - (DateTime.UtcNow - startTime);

            if (delay > TimeSpan.FromSeconds(0))
            {
                Thread.Sleep(delay);
            }
            
            var thread = animal.Species.Equals("dog", StringComparison.OrdinalIgnoreCase)
                ? new Thread(() => Dog(animal.Id, animal.ArrivalTime))
                : new Thread(() => Cat(animal.Id, animal.ArrivalTime));
            
            thread.Start();
        }
    }

    public static void Dog(string id, int restDuration)
    {
        while (true)
        {
            _roomStateMutex.WaitOne();
            
            Console.WriteLine($"\nO CACHORRO {id} está tentando entrar na sala de repouso, toc toc toc ...");
            
            if (_roomState != ERoomState.Cats)
            {
                _dogCount++;
                
                if (_dogCount == 1)
                {
                    _roomState = ERoomState.Dogs;
                }
                
                Console.WriteLine($"\nO CACHORRO {id} entrou na sala de repouso ...");
                
                _roomStateMutex.ReleaseMutex();
                
                Thread.Sleep(restDuration * 1000);
                
                _roomStateMutex.WaitOne();
                
                _dogCount--;
                
                if (_dogCount == 0)
                {
                    _roomState = ERoomState.Empty;
                }
                
                Console.WriteLine($"\nO CACHORRO {id} saiu da sala de repouso ...");
                
                _roomStateMutex.ReleaseMutex();
                
                break; 
            }
    
            _roomStateMutex.ReleaseMutex(); 
            
            Thread.Yield(); 
        }
    }

    public static void Cat(string id, int restDuration)
    {
        while (true)
        {
            _roomStateMutex.WaitOne();
    
            Console.WriteLine($"\nO GATO {id} está tentando entrar na sala de repouso, toc toc toc ...");
            
            if (_roomState != ERoomState.Dogs)
            {
                _catCount++;
                
                if (_catCount == 1)
                {
                    _roomState = ERoomState.Cats;
                }
                
                Console.WriteLine($"\nO GATO {id} entrou na sala de repouso ...");
                
                _roomStateMutex.ReleaseMutex();
                
                Thread.Sleep(restDuration * 1000);
                
                _roomStateMutex.WaitOne();
                
                _catCount--;
                
                if (_catCount == 0)
                {
                    _roomState = ERoomState.Empty;
                }
                
                Console.WriteLine($"\nO GATO {id} saiu da sala de repouso ...");
                
                _roomStateMutex.ReleaseMutex();
                
                break; 
            }
    
            _roomStateMutex.ReleaseMutex(); 
            
            Thread.Yield(); 
        }
    }
}