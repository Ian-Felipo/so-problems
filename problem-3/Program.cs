using Problem_3.Models;

namespace Problem_3;

enum RoomState
{
    Empty = 0,
    Dogs = 1,
    Cats = 2,
}

class Program
{
    private static RoomState _roomState = RoomState.Empty;
    private static Mutex _roomStateMutex = new();
    private static int _dogsCount = 0;
    private static Mutex _dogsCountMutex = new();
    private static int _dogs = 100;
    private static int _cats = 100;
    private static readonly QueueFifo<Thread> _queueFifo = new QueueFifo<Thread>(_dogs + _cats);
    
    static void Main()
    {
        for (int i = 1; i <= _dogs; i++)
        {
            Thread dog = new Thread(Dog);
            _queueFifo.Enqueue(dog);
        }
        
        for (int i = 1; i <= _cats; i++)
        {
            Thread cat = new Thread(Cat);
            _queueFifo.Enqueue(cat);
        }

        foreach (var thread in _queueFifo)
        {
            var animal = _queueFifo.Dequeue();

            if (animal == null)
            {
                break;
            }
            
            animal.Start();
            animal.Join();
        }
    }

    static void Dog()
    {
        Console.WriteLine("Um CACHORRO está tentando entrar na sala de espera , toc toc ....");
        
        _roomStateMutex.WaitOne();
        
        if (_roomState == RoomState.Empty || _roomState == RoomState.Dogs)
        {
            Console.WriteLine("Um CACHORRO entrou na sala de espera, au au ...");
            
            _dogsCountMutex.WaitOne();
            
            _dogsCount++;
            
            if (_roomState == RoomState.Empty && _dogsCount == 1)
            {
                _roomState = RoomState.Dogs;
            }
            
            _dogsCountMutex.ReleaseMutex();
            
            _roomStateMutex.ReleaseMutex();
        }
        else
        {
            Console.WriteLine("Um CACHORRO não conseguiu entrar na sala de espera, au au ...");

            _roomStateMutex.ReleaseMutex();
            
                        
        }
    }

    static void Cat()
    {
        Console.WriteLine("Um GATO está tentando entrar na sala de espera , toc toc ....");
        
        _roomStateMutex.WaitOne();
        
        
    }
}