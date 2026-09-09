using System.Globalization;

namespace Lab01;

public class Task2
{
    public static void Run()
    {
        double price = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        int count = int.Parse(Console.ReadLine());
        int discount = int.Parse(Console.ReadLine());

        double total = price * count * (1 - discount / 100.0);

        Console.WriteLine($"Сума: {total.ToString("F2", CultureInfo.InvariantCulture)} грн");
    }
}