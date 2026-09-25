namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        Clinic clinic = new Clinic("Медична Клініка");

        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1990, 5, 15), "A(I)+", "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1985, 8, 20), "B(III)+", "0672345678"));
        clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2000, 1, 10), "O(I)-", "0933456789"));

        clinic.Doctors.Add(new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567"));
        clinic.Doctors.Add(new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678"));
        clinic.Doctors.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789"));

        DateTime targetDate = DateTime.Today.AddDays(1);

        clinic.Appointments.Book(1, 1, targetDate.AddHours(10), 30);
        clinic.Appointments.Book(2, 2, targetDate.AddHours(11), 45);
        clinic.Appointments.Book(3, 3, targetDate.AddHours(12), 20);

        Console.WriteLine();
        clinic.DisplaySchedule(targetDate);

        Console.WriteLine();
        clinic.GenerateReport();
    }
}