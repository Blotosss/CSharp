namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        RunPatientMenu();
    }

    static void RunPatientMenu()
    {
        PatientManager manager = new PatientManager();

        Console.WriteLine("=== Додавання пацієнтів ===");
        manager.Add(new Patient("Іван", "Петренко", new DateTime(1983, 5, 15), "A+", "0501234567"));
        manager.Add(new Patient("Олена", "Коваль", new DateTime(1991, 11, 20), "B-", "0672345678"));
        manager.Add(new Patient("Максим", "Бойко", new DateTime(2008, 3, 10), "0+", "0933456789"));
        manager.Add(new Patient("Марія", "Ткач", new DateTime(1998, 7, 25), "0+", "0000000000"));

        Console.WriteLine();
        manager.DisplayAll();

        Console.WriteLine();
        manager.DisplayStats();
    }
}