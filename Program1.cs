using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static Queue<int> queue = new Queue<int>();
    static object queueLock = new object();
    static void Main(string[] args)
    {
        // Створюємо та запускаємо поток виробника
        Thread producer = new Thread(Produce);
        producer.Start();

        // Створюємо та запускаємо поток споживача
        Thread consumer = new Thread(Consume);
        consumer.Start();

        // Очікуємо, поки обидва потоки завершаться
        producer.Join();
        consumer.Join();

        Console.ReadKey();
    }

    static void Produce()
    {
        Random rnd = new Random();
        for (int i = 0; i < 10; i++)
        {
            // Генеруємо випадкове ціле число
            int number = rnd.Next(100);
            // Блокуємо доступ до черги
            lock (queueLock)
            {
                // Додаємо число до черги
                queue.Enqueue(number);
                Console.WriteLine($"Виробник додав номер {number} до черги");
            }
            // Пауза перед генерацією наступного числа
            Thread.Sleep(500);
        }
    }

    static void Consume()
    {
        for (int i = 0; i < 10; i++)
        {
            // Блокуємо доступ до черги
            lock (queueLock)
            {
                if (queue.Count > 0)
                {
                    // Беремо перший елемент з черги
                    int number = queue.Dequeue();
                    Console.WriteLine($"Споживач отримав номер {number} з черги");
                }
            }
            // Пауза перед зчитуванням наступного числа з черги
            Thread.Sleep(500);
        }
    }
}
