namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Тест GrowablePatientManager ===");
        Console.WriteLine("Додаємо пацієнтів одного за одним ...");

        GrowablePatientManager growableManager = new GrowablePatientManager();

        for (int i = 1; i <= 20; i++)
        {
            growableManager.Add(new Patient($"Тест", $"Пацієнт{i}"));
        }

        Console.WriteLine("\nТест пошуку:");
        
        Patient? p10 = growableManager.FindById(10);
        Console.WriteLine($"FindById(10) -> {(p10 != null ? p10.FullName : "не знайдено")}");

        Patient? p99 = growableManager.FindById(99);
        Console.WriteLine($"FindById(99) -> {(p99 != null ? p99.FullName : "не знайдено")}");

        Console.WriteLine("\nПорівняння:");
        PatientManager fixedManager = new PatientManager();
        Console.WriteLine($"  PatientManager:          100 місць (фіксовано)");
        Console.WriteLine($"  GrowablePatientManager:  {growableManager.Capacity} місця (зросте при потребі)");
    }
}