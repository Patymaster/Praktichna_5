using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        int[] arr = { 5, 2, 8, 1, 9, 4, 6, 3, 7 }; // вхідний масив

        QuickSort(arr); // сортуємо масив

        Console.WriteLine("Вiдсортований масив:");
        foreach (int num in arr)
        {
            Console.Write($"{num} ");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static void QuickSort(int[] arr)
    {
        QuickSort(arr, 0, arr.Length - 1); // викликаємо першу ітерацію QuickSort
    }

    static void QuickSort(int[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(arr, left, right); // розбиваємо масив на дві частини

            // створюємо два потоки для сортування лівої та правої частини масиву
            Thread leftThread = new Thread(() => QuickSort(arr, left, pivotIndex - 1));
            Thread rightThread = new Thread(() => QuickSort(arr, pivotIndex + 1, right));

            leftThread.Start();
            rightThread.Start();

            leftThread.Join();
            rightThread.Join();
        }
    }

    static int Partition(int[] arr, int left, int right)
    {
        int pivot = arr[right]; // обираємо останній елемент масиву як опорний
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                Swap(arr, i, j);
            }
        }

        Swap(arr, i + 1, right);
        return i + 1;
    }

    static void Swap(int[] arr, int i, int j)
    {
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}

