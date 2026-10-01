namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Лабораторна робота 04. Задача 2: Struct WorkSchedule ===");

        WorkSchedule morning = new WorkSchedule(8, 16);
        WorkSchedule evening = new WorkSchedule(14, 22);

        Console.WriteLine($"Ранковий розклад: {morning}");
        Console.WriteLine($"Чи діє зараз morning: {morning.IsNow}");
        Console.WriteLine($"Вечірній розклад: {evening}");

        // Демонстрація семантики копіювання value type (struct)
        WorkSchedule copy = morning;
        Console.WriteLine($"Скопійований розклад: {copy}");

        Clinic clinic = new Clinic("Медична Клініка");

        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1990, 5, 15), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1985, 8, 20), BloodType.BPositive, "0672345678"));

        Doctor doc = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567");
        doc.Schedule = new WorkSchedule(8, 16);
        clinic.Doctors.Add(doc);

        Console.WriteLine($"\nЛікар з новим розкладом:");
        Console.WriteLine(doc);
        Console.WriteLine($"Doctor.Schedule.ToString(): {doc.Schedule}");
    }
}