using System;
using System.Threading;

class TrafficLight
{
    private readonly object monitor = new object();
    private Semaphore sem;
    private ConsoleColor color;
    private int id;
    private bool run;

public TrafficLight(Semaphore semaphore, ConsoleColor color, int id)
    {
        this.sem = semaphore;
        this.color = color;
        this.id = id;
    }

    public void Run()
    {
        run = true;
        while (run)
        {
            lock (monitor)
            {
                Console.ForegroundColor = color;
                Console.WriteLine($"Свiтлофор {id}: загорівся {color}");
            }

            if (color == ConsoleColor.Red)
            {
                sem.WaitOne();
            }
            else
            {
                sem.Release();
            }

            Thread.Sleep(5000); // затримка світлофора

            lock (monitor)
            {
                Console.ForegroundColor = color;
                Console.WriteLine($"Свiтлофор {id}: погас {color}");
            }

            if (color == ConsoleColor.Red)
            {
                sem.Release();
            }
            else
            {
                sem.WaitOne();
            }

            Thread.Sleep(1000); //затримка
        }
    }

    public void Stop()
    {
        run = false;
    }
}

class Car
{
    private readonly Semaphore sem;
    private readonly int id;

public Car(Semaphore semaphore, int id)
    {
        this.sem = semaphore;
        this.id = id;
    }

    public void Run()
    {
        while (true)
        {
            sem.WaitOne(); // Очікування дозволу на рух

            Console.WriteLine($"Авто {id} перетинає перехрестя");
            Thread.Sleep(3000); // час перетину перехрестя

            sem.Release(); // Авто покинуло перехрестя
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Semaphore semaphore = new Semaphore(2, 4); // 2 авто можуть рухатись одночасно

            TrafficLight[] trafficLights = new TrafficLight[]
            {
        new TrafficLight(semaphore, ConsoleColor.Red, 1),
        new TrafficLight(semaphore, ConsoleColor.Yellow, 2),
        new TrafficLight(semaphore, ConsoleColor.Green, 3),
        new TrafficLight(semaphore, ConsoleColor.Yellow, 4)
            };

        foreach (TrafficLight trafficLight in trafficLights)
        {
            Thread thread = new Thread(trafficLight.Run);
            thread.Start();
        }

        for (int i = 1; i <= 10; i++)
        {
            Car car = new Car(semaphore, i);
            Thread thread = new Thread(car.Run);
            thread.Start();
        }

        Console.ReadLine();

        foreach (TrafficLight trafficLight in trafficLights)
        {
            trafficLight.Stop();
        }
    }
}