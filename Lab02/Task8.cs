namespace Lab02;

public class Task8
{
    public static void Run()
    {
        int dep = int.Parse(Console.ReadLine());
        int weeks = int.Parse(Console.ReadLine());

        int[,,] data = new int[dep, weeks, 2];

        for (int i = 0; i < dep; i++)
        {
            for (int j = 0; j < weeks; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    data[i, j, k] = int.Parse(Console.ReadLine());
                }
            }
        }

        int[] departmentTotals = new int[dep];

        for (int i = 0; i < dep; i++)
        {
            Console.WriteLine($"Відділення {i + 1}:");
            int totalDepartmentPatients = 0;

            for (int j = 0; j < weeks; j++)
            {
                int morning = data[i, j, 0];
                int evening = data[i, j, 1];
                int weekSum = morning + evening;

                totalDepartmentPatients += weekSum;

                Console.WriteLine($"    Тиждень {j + 1}: ранок {morning}, вечір {evening} -> разом {weekSum}");
            }

            departmentTotals[i] = totalDepartmentPatients;
            Console.WriteLine($"Разом: {totalDepartmentPatients} пацієнтів");
        }

        int maxIndex = 0;
        int maxPatients = departmentTotals[0];

        for (int i = 1; i < dep; i++)
        {
            if (departmentTotals[i] > maxPatients)
            {
                maxPatients = departmentTotals[i];
                maxIndex = i;
            }
        }

        Console.WriteLine($"Найзавантаженіше: Відділення {maxIndex + 1} ({maxPatients} пацієнтів)");
    }
}