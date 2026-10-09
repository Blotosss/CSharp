using ClinicApp;
using ClinicApp.Models;
using ClinicApp.Enums;
using ClinicApp.Managers;
using ClinicApp.Utils;

class Program
{
    static void Main(string[] args)
    {
        Clinic clinic = new Clinic("Медичний центр 'Здоров'я'");

        try
        {
            clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 3, 15), BloodType.APositive, "0501234567"));
            clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 7, 22), BloodType.BPositive, "0672345678"));
            clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 11, 5), BloodType.ONegative, "0933456789"));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"[Помилка межі]: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[Помилка даних]: {ex.Message}");
        }

        try
        {
            Doctor doc1 = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567");
            doc1.Schedule = new WorkSchedule(8, 16);
            clinic.Doctors.Add(doc1);

            Doctor doc2 = new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678");
            clinic.Doctors.Add(doc2);

            Doctor doc3 = new Doctor("Андрій", "Власенко", Speciality.Cardiology, "LIC-003", "0443456789");
            clinic.Doctors.Add(doc3);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"[Помилка межі]: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[Помилка даних]: {ex.Message}");
        }

        try
        {
            DateTime testDate = new DateTime(2026, 5, 10);
            clinic.Appointments.Book(1, 1, testDate.AddHours(9), 30);
            clinic.Appointments.Book(2, 3, testDate.AddHours(10), 30);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"[Помилка межі]: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[Помилка даних]: {ex.Message}");
        }

        try
        {
            clinic.Patients.Add(new Patient("", "Тестовий", DateTime.Today, BloodType.Unknown, "123"));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"[Перехоплено ArgumentOutOfRangeException]: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[Перехоплено ArgumentException]: {ex.Message}");
        }

        try
        {
            WorkSchedule invalidSchedule = new WorkSchedule(20, 6);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"[Перехоплено ArgumentOutOfRangeException]: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[Перехоплено ArgumentException]: {ex.Message}");
        }

        try
        {
            clinic.Appointments.Book(1, 1, DateTime.Now, -10);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"[Перехоплено ArgumentOutOfRangeException]: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[Перехоплено ArgumentException]: {ex.Message}");
        }

        Console.WriteLine($"\nBloodType.APositive: {ClinicFormatter.FormatBloodType(BloodType.APositive)}");
        Console.WriteLine($"Пацієнт 1: {clinic.Patients[0]}");

        Patient? firstPatient = clinic.Patients[0];
        Console.WriteLine($"clinic.Patients[0]: {firstPatient?.FullName ?? "null"}");

        Doctor? firstDoctor = clinic.Doctors[0];
        Console.WriteLine($"clinic.Doctors[0]: {firstDoctor?.FullName ?? "null"}");

        Doctor[] cardiologistsEnum = clinic.Doctors.FindBySpeciality(Speciality.Cardiology);
        foreach (var d in cardiologistsEnum)
        {
            Console.WriteLine($"  {d}");
        }

        Appointment[] appsByDate = clinic.Appointments.GetByDate(2026, 5, 10);
        Console.WriteLine($"Записів на 10.05.2026: {appsByDate.Length}");

        if (clinic.Patients.TryFindById(1, out Patient? foundPatient))
        {
            Console.WriteLine($"Знайдено пацієнта: {foundPatient.FullName}");
        }

        string safeNameFound = clinic.Patients.FindById(1)?.FullName ?? "не знайдено";
        Console.WriteLine($"FindById(1): {safeNameFound}");
    }
}