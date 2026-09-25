namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        DateTime baseDate = new DateTime(2026, 5, 9);

        Appointment a1 = new Appointment(1, 1, baseDate.AddHours(10), 30);
        Appointment a2 = new Appointment(2, 2, baseDate.AddHours(11), 45);
        Appointment a3 = new Appointment(3, 3, baseDate.AddDays(1).AddHours(9), 20);

        Console.WriteLine(a1);
        Console.WriteLine(a2);
        Console.WriteLine(a3);

        Console.WriteLine("\n// Після Cancel та Complete:");
        a1.Cancel("Пацієнт не зміг прийти");
        a2.Complete();

        Console.WriteLine(a1);
        Console.WriteLine(a2);
    }
}