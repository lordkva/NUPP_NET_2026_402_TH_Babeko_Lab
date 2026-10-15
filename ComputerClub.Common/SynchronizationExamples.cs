using System.Threading;
namespace ComputerClub.Common;
public static class SynchronizationExamples
{
    private static int counter = 0;
    private static readonly object locker = new();
    public static void LockExample()
    {
        Parallel.For(0, 1000, _ =>
        {
            lock (locker)
            {
                counter++;
            }
        });

        Console.WriteLine($"Lock counter: {counter}");
        counter = 0;
    }
    // Приклад SemaphoreSlim
    public static async Task SemaphoreExample()
    {
        using SemaphoreSlim semaphore = new(2, 2);
        async Task Work()
        {
            await semaphore.WaitAsync();
            try
            {
                Console.WriteLine(
                    $"Робота виконується у потоці {Environment.CurrentManagedThreadId}");

                await Task.Delay(100);
            }
            finally
            {
                semaphore.Release();
            }
        }

        await Task.WhenAll(
            Work(), Work(), Work(), Work());
    }
    public static void AutoResetEventExample()
    {
        using AutoResetEvent signal = new(false);
        Task task = Task.Run(() =>
        {
            Console.WriteLine("Очікування сигналу...");
            signal.WaitOne();
            Console.WriteLine("Сигнал отримано!");
        });
        Thread.Sleep(200);
        signal.Set();
        task.Wait();
    }
}