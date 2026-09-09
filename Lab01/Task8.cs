using System.Globalization;

namespace Lab01;

public class Task8
{
    public static void Run()
    {
        double weight = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        double height = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        double price = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        int visits = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        int discount = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        int birthYear = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        int systolic = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
        int diastolic = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

        double bmi = CalculateBMI(weight, height);
        string bmiCategory = GetBMICategory(bmi);
        double totalCost = CalculateCost(price, visits, discount);
        
        int currentYear = 2026;
        int age = currentYear - birthYear;
        string ageCategory = GetAgeCategory(age);
        string pressureStatus = GetPressureStatus(systolic, diastolic);

        Console.WriteLine($"ІМТ: {bmi:F2} → {bmiCategory}");
        Console.WriteLine($"Сума: {totalCost:F2} грн");
        Console.WriteLine($"Вік: {age} р., категорія: {ageCategory}");
        Console.WriteLine($"Тиск: {systolic}/{diastolic} – {pressureStatus}");
    }

    public static double CalculateBMI(double weight, double height)
    {
        return weight / (height * height);
    }

    public static string GetBMICategory(double bmi)
    {
        if (bmi < 18.5) return "недостатня вага";
        if (bmi < 25.0) return "нормальна вага";
        if (bmi < 30.0) return "надмірна вага";
        return "ожиріння";
    }

    public static double CalculateCost(double price, int visits, int discount)
    {
        double subtotal = price * visits;
        return subtotal - (subtotal * discount / 100.0);
    }

    public static string GetAgeCategory(int age)
    {
        if (age < 18) return "дитина";
        if (age <= 60) return "дорослий";
        return "літній";
    }

    public static string GetPressureStatus(int systolic, int diastolic)
    {
        if (systolic < 120 && diastolic < 80) return "оптимальний";
        if (systolic <= 129 && diastolic <= 84) return "нормальний";
        if (systolic <= 139 && diastolic <= 89) return "високий нормальний";
        if (systolic <= 159 && diastolic <= 99) return "гіпертонія 1 ступеня";
        if (systolic <= 179 && diastolic <= 109) return "гіпертонія 2 ступеня";
        return "гіпертонія 3 ступеня";
    }
}