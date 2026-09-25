namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Task 01: Patients ===");
        Patient p1 = new Patient("Іван", "Петренко", new DateTime(1983, 5, 15), "A+", "0501234567");
        Patient p2 = new Patient("Олена", "Коваль", new DateTime(1991, 11, 20), "B-", "0672345678");
        Patient p3 = new Patient("Максим", "Бойко", new DateTime(2008, 3, 10), "0+", "0933456789");
        Patient p4 = new Patient("Невідомий", "Пацієнт");
        Patient p5 = new Patient();
        p5.FirstName = "Марія";
        p5.LastName = "Ткач";

        Console.WriteLine(p1);
        Console.WriteLine(p2);
        Console.WriteLine(p3);
        Console.WriteLine(p4);
        Console.WriteLine(p5);

        Console.WriteLine("\n=== Task 02: Doctors ===");
        Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        d1.WorkStartHour = 8;
        d1.WorkEndHour = 16;

        Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;

        Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");

        Console.WriteLine(d1);
        Console.WriteLine(d2);
        Console.WriteLine(d3);
    }
}