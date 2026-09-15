namespace Lab02;

public class Task2
{
    public static void Run()
    {
        int number = int.Parse(Console.ReadLine()); 
        int[] price = new int[number];
        
        for (int i = 0; i < number; i++)
        {
            price[i] = int.Parse(Console.ReadLine());
        }
        
        string beforeSort = string.Join(" ", price);
        
        for (int i = 0; i < number - 1; i++)
        {
            for (int j = 0; j < number - i - 1; j++)
            {
                if (price[j] > price[j + 1])
                {
                    (price[j], price[j + 1]) = (price[j + 1], price[j]);
                }
            }
        }
        
        string afterSort = string.Join(" ", price);
        
        int min = price[0];
        int max = price[number - 1];
        
        Console.WriteLine($"Черга (до):     {beforeSort}");
        Console.WriteLine($"Черга (після):  {afterSort}");
        Console.WriteLine($"Найдешевший:    {min} грн");
        Console.WriteLine($"Найдорожчий:    {max} грн");
    }
}