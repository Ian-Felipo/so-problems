using Problem_3.Models;
using System.Text.Json;

namespace Problem_3;

class Program
{
    private static ERoomState _roomState = ERoomState.Empty; 
    private static Mutex _roomStateMutex = new Mutex();
    private static int _dogCount;
    private static Mutex _dogCountMutex = new Mutex();
    private static int _catCount;
    private static Mutex _catCountMutex = new Mutex();
    
    public static void Main(string[] args)
    {
        var path = args[0];
        var challengeJson = File.ReadAllText(path);
        var challenge = JsonSerializer.Deserialize<Challenge>(challengeJson);
        _dogCount = challenge!.Workload.Animals.Count(animal => animal.Species.Equals("dog", StringComparison.OrdinalIgnoreCase)); 
        _catCount = challenge!.Workload.Animals.Count(animal => animal.Species.Equals("cat", StringComparison.OrdinalIgnoreCase)); 
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
    
            Console.WriteLine($"O CACHORRO {id} está tentando entrar na sala de repouso, toc toc toc ...");
            
            if (_roomState != ERoomState.Cats)
            {
                _dogCountMutex.WaitOne();

                _dogCount++;
                
                if (_dogCount == 1)
                {
                    _roomState = ERoomState.Dogs;
                }
                
                _dogCountMutex.ReleaseMutex();
                
                _roomStateMutex.ReleaseMutex();
                
                Console.WriteLine($"O CACHORRO {id} entrou na sala de repouso, au au au ...");
                
                Thread.Sleep(restDuration);
                
                _roomStateMutex.WaitOne();
                
                _dogCountMutex.WaitOne();

                _dogCount--;
                
                if (_dogCount == 0)
                {
                    _roomState = ERoomState.Empty;
                }
                
                _dogCountMutex.ReleaseMutex();
                
                _roomStateMutex.ReleaseMutex();
                
                Console.WriteLine($"O CACHORRO {id} saiu da sala de repouso, au au au ...");
                
                break; 
            }
    
            _roomStateMutex.ReleaseMutex(); 
    
            Random random = new Random();
            
            var delay = random.Next(500, 2000);
            
            Thread.Sleep(delay);
        }
    }

    public static void Cat(string id, int restDuration)
    {
        while (true)
        {
            _roomStateMutex.WaitOne();
    
            Console.WriteLine($"O GATO {id} está tentando entrar na sala de repouso, toc toc toc ...");
            
            if (_roomState != ERoomState.Dogs)
            {
                _catCountMutex.WaitOne();

                _catCount++;
                
                if (_catCount == 1)
                {
                    _roomState = ERoomState.Cats;
                }
                
                _catCountMutex.ReleaseMutex();
                
                _roomStateMutex.ReleaseMutex();
                
                Console.WriteLine($"O GATO {id} entrou na sala de repouso, meow meow meow ...");
                
                Thread.Sleep(restDuration);
                
                _roomStateMutex.WaitOne();
                
                _catCountMutex.WaitOne();

                _catCount--;
                
                if (_catCount == 0)
                {
                    _roomState = ERoomState.Cats;
                }
                
                _catCountMutex.ReleaseMutex();
                
                _roomStateMutex.ReleaseMutex();
                
                Console.WriteLine($"O GATO {id} saiu da sala de repouso, meow meow meow ...");
                
                break; 
            }
    
            _roomStateMutex.ReleaseMutex(); 
            
            Random random = new Random();
            
            var delay = random.Next(500, 2000);
            
            Thread.Sleep(delay);
        }
    }
}