namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        RunDoctorMenu();
    }

    static void RunDoctorMenu()
    {
        DoctorManager manager = new DoctorManager();

        Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
        d1.WorkStartHour = 8;
        d1.WorkEndHour = 16;

        Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;

        Doctor d3 = new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789");

        manager.Add(d1);
        manager.Add(d2);
        manager.Add(d3);

        manager.DisplayAll();
        Console.WriteLine();
        manager.DisplayStats();
    }
}