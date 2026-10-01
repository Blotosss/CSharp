namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Лабораторна робота 04. Задача 1: Enum ===");

        Clinic clinic = new Clinic("Медична Клініка");

        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1990, 5, 15), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1985, 8, 20), BloodType.BPositive, "0672345678"));
        clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2000, 1, 10), BloodType.ONegative, "0933456789"));

        clinic.Doctors.Add(new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567"));
        clinic.Doctors.Add(new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678"));
        clinic.Doctors.Add(new Doctor("Андрій", "Власенко", Speciality.Pediatrics, "LIC-003", "0443456789"));

        DateTime targetDate = DateTime.Today.AddDays(1);
        clinic.Appointments.Book(1, 1, targetDate.AddHours(10), 30);
        clinic.Appointments.Book(2, 2, targetDate.AddHours(11), 45);

        Console.WriteLine("\nВсі лікарі:");
        clinic.Doctors.DisplayAll();

        Console.WriteLine("Пошук кардіологів за назвою:");
        Doctor[] cardioDocs = clinic.Doctors.FindBySpeciality("cardio");
        foreach (var doc in cardioDocs)
        {
            Console.WriteLine(doc);
        }

        Console.WriteLine("\nСтатус першого запису:");
        Appointment[] apps = clinic.Appointments.GetUpcoming();
        if (apps.Length > 0)
        {
            Console.WriteLine($"ID: {apps[0].Id}, Статус: {apps[0].Status}");
        }
    }
}