namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
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
    }
}