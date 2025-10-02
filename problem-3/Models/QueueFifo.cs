namespace Problem_3.Models;

public class QueueFifo<T> 
{
    private readonly Queue<T> _queue = new Queue<T>();
    private readonly Semaphore _itensCount;
    private readonly Mutex _mutex = new Mutex();

    public QueueFifo(int capacity)
    {
        _itensCount = new Semaphore(0, capacity);
    }

    public void Enqueue(T item)
    {
        _mutex.WaitOne();
        _queue.Enqueue(item);
        _mutex.ReleaseMutex();
        _itensCount.Release();
    }

    public T? Dequeue()
    {
        _itensCount.Release();
        _mutex.WaitOne();
        var item = _queue.Dequeue();
        _mutex.ReleaseMutex();
        return item;
    }
}