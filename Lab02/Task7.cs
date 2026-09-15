namespace Lab02;

public class Task7
{
    public static void Run()
    {
        int number = int.Parse(Console.ReadLine());

        string[] names = new string[number];
        double[] bmis = new double[number];

        for (int i = 0; i < number; i++)
        {
            names[i] = Console.ReadLine();
            bmis[i] = double.Parse(Console.ReadLine());
        }

        for (int i = 0; i < number - 1; i++)
        {
            for (int j = 0; j < number - 1 - i; j++)
            {
                if (bmis[j] < bmis[j + 1])
                {
                    double tempBmi = bmis[j];
                    bmis[j] = bmis[j + 1];
                    bmis[j + 1] = tempBmi;

                    string tempName = names[j];
                    names[j] = names[j + 1];
                    names[j + 1] = tempName;
                }
            }
        }

        for (int i = 0; i < number; i++)
        {
            Console.WriteLine($"#{i + 1} {names[i]}: {bmis[i]:F2}");
        }
    }
}