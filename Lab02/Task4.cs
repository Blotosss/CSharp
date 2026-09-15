namespace Lab02;

public class Task4
{
    public static void Run()
    {
        int rows = int.Parse(Console.ReadLine());
        int col = int.Parse(Console.ReadLine());

        int[,] matrix = new int[rows, col];

        for (int i = 0; i < rows; i++)
        {
            string[] parts = Console.ReadLine().Split(' ');
            for (int j = 0; j < col; j++)
            {
                matrix[i, j] = int.Parse(parts[j]);
            }
        }

        for (int i = 0; i < rows; i++)
        {
            int rowSum = 0;
            for (int j = 0; j < col; j++)
            {
                rowSum += matrix[i, j];
            }
            Console.WriteLine($"Лікар {i + 1}: {rowSum} прийомів");
        }

        Console.Write("По днях: ");
        for (int j = 0; j < col; j++)
        {
            int colSum = 0;
            for (int i = 0; i < rows; i++)
            {
                colSum += matrix[i, j];
            }
            
            if (j == col - 1)
                Console.Write(colSum);
            else
                Console.Write(colSum + ", ");
        }
        Console.WriteLine();

        int maxVal = matrix[0, 0];
        int maxRow = 0;
        int maxCol = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (matrix[i, j] > maxVal)
                {
                    maxVal = matrix[i, j];
                    maxRow = i;
                    maxCol = j;
                }
            }
        }

        Console.WriteLine($"Максимум: {maxVal} (Лікар {maxRow + 1}, День {maxCol + 1})");
    }
}