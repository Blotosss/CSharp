using System.Globalization;

namespace Lab01;

public class Task7
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        decimal[] costs = new decimal[n];

        for (int i = 0; i < n; i++)
        {
            costs[i] = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        }

        decimal sum = 0;
        decimal min = costs[0];
        decimal max = costs[0];

        foreach (var cost in costs)
        {
            sum += cost;
            if (cost < min) min = cost;
            if (cost > max) max = cost;
        }

        decimal average = sum / n;

        int aboveAverageCount = 0;
        foreach (var cost in costs)
        {
            if (cost > average)
            {
                aboveAverageCount++;
            }
        }

        string firstExpensive = "немає";
        int index = 0;
        while (index < n)
        {
            if (costs[index] > 1000m)
            {
                firstExpensive = $"#{index + 1} – {costs[index].ToString("F2", CultureInfo.InvariantCulture)} грн";
                break;
            }
            index++;
        }

        Console.WriteLine("=== Звіт по прийомах ===");
        Console.WriteLine($"Кількість:      {n}");
        Console.WriteLine($"Загальна сума:  {sum.ToString("F2", CultureInfo.InvariantCulture)} грн");
        Console.WriteLine($"Середня:        {average.ToString("F2", CultureInfo.InvariantCulture)} грн");
        Console.WriteLine($"Мін / Макс:     {min.ToString("F2", CultureInfo.InvariantCulture)} / {max.ToString("F2", CultureInfo.InvariantCulture)} грн");
        Console.WriteLine($"Вище середнього: {aboveAverageCount} з {n}");
        Console.WriteLine($"Перший > 1000:  {firstExpensive}");
    }
}