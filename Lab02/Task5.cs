namespace Lab02;

public class Task5
{
    public static void Run()
    {
        int number = int.Parse(Console.ReadLine());

        int[,] matrix = new int[number, number];

        for (int i = 0; i < number; i++)
        {
            string[] parts = Console.ReadLine().Split(' ');
            for (int j = 0; j < number; j++)
            {
                matrix[i, j] = int.Parse(parts[j]);
            }
        }

        int[] mainDiagonal = new int[number];
        int[] antiDiagonal = new int[number];
        int mainSum = 0;
        int antiSum = 0;

        for (int i = 0; i < number; i++)
        {
            mainDiagonal[i] = matrix[i, i];
            mainSum += matrix[i, i];

            antiDiagonal[i] = matrix[i, number - 1 - i];
            antiSum += matrix[i, number - 1 - i];
        }

        Console.WriteLine($"Головна діагональ: {string.Join(", ", mainDiagonal)} (сума = {mainSum})");
        Console.WriteLine($"Побічна діагональ: {string.Join(", ", antiDiagonal)} (сума = {antiSum})");
    }
}