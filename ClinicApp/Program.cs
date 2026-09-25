namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        PatientManager patientManager = new PatientManager();
        patientManager.Add(new Patient("Іван", "Петренко", new DateTime(1990, 5, 15), "A(I)+", "0501234567"));
        patientManager.Add(new Patient("Олена", "Коваль", new DateTime(1985, 8, 20), "B(III)+", "0672345678"));
        patientManager.Add(new Patient("Максим", "Бойко", new DateTime(2000, 1, 10), "O(I)-", "0933456789"));

        DoctorManager doctorManager = new DoctorManager();
        doctorManager.Add(new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567"));
        doctorManager.Add(new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678"));
        doctorManager.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789"));

        AppointmentManager appointmentManager = new AppointmentManager(patientManager, doctorManager);

        DateTime baseDate = DateTime.Now.AddDays(1);
        appointmentManager.Book(1, 1, baseDate.AddHours(10), 30);
        appointmentManager.Book(99, 1, baseDate.AddHours(10), 30);
        appointmentManager.Book(2, 2, baseDate.AddHours(11), 45);
        appointmentManager.Book(3, 3, baseDate.AddDays(1).AddHours(9), 20);

        Console.WriteLine("\nМайбутні записи:");
        appointmentManager.DisplayList(appointmentManager.GetUpcoming());

        Console.WriteLine();
        appointmentManager.Cancel(1);

        Console.WriteLine("\nЗаписи пацієнта #2:");
        appointmentManager.DisplayList(appointmentManager.GetByPatient(2));
    }
}