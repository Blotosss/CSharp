using System.Globalization;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Models;

class Program
{
    static void Main()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        Clinic clinic = new Clinic("Медичний центр 'Здоров'я'");

        SeedData(clinic);

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine($"\n=== {clinic.Name} ===");
            Console.WriteLine("1. Управління пацієнтами");
            Console.WriteLine("2. Управління лікарями");
            Console.WriteLine("3. Запис на прийом");
            Console.WriteLine("4. Медична картка");
            Console.WriteLine("5. Звіт");
            Console.WriteLine("0. Вихід");
            Console.Write("Оберіть пункт: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    PatientsMenu(clinic);
                    break;
                case "2":
                    DoctorsMenu(clinic);
                    break;
                case "3":
                    AppointmentsMenu(clinic);
                    break;
                case "4":
                    MedicalRecordsMenu(clinic);
                    break;
                case "5":
                    DisplayReport(clinic);
                    break;
                case "0":
                    exit = true;
                    Console.WriteLine("Завершення роботи.");
                    break;
                default:
                    Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    private static void SeedData(Clinic clinic)
    {
        try
        {
            clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 3, 15), BloodType.APositive, "0501234567"));
            clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 7, 22), BloodType.BPositive, "0672345678"));
            clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 11, 5), BloodType.ONegative, "0933456789"));

            Doctor doc1 = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567")
            {
                Schedule = new WorkSchedule(8, 16)
            };
            clinic.Doctors.Add(doc1);

            Doctor doc2 = new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678");
            clinic.Doctors.Add(doc2);

            Doctor doc3 = new Doctor("Андрій", "Власенко", Speciality.Cardiology, "LIC-003", "0443456789");
            clinic.Doctors.Add(doc3);

            DateTime testDate = DateTime.Today;
            clinic.Appointments.Book(1, 1, testDate.AddHours(9));
            clinic.Appointments.Book(2, 3, testDate.AddHours(10));

            clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-30), "I10", "Гіпертонічна хвороба", true));
            clinic.MedicalRecords.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-5), "J06.9", "Гостра респіраторна інфекція"));
            clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today.AddDays(-5), "Гемоглобін", 145, "г/л", "120–160", true));
            clinic.MedicalRecords.Add(new LabResult(1, 1, DateTime.Today.AddDays(-2), "Холестерин", 6.2, "ммоль/л", "< 5.2", false));
            clinic.MedicalRecords.Add(new Prescription(1, 1, DateTime.Today.AddDays(-5), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));

            clinic.MedicalRecords.Add(new Prescription(2, 2, DateTime.Today.AddDays(-40), "Амоксицилін", "500 мг", 10, "3 рази на добу"));
            clinic.MedicalRecords.Add(new Diagnosis(2, 2, DateTime.Today.AddDays(-40), "K29", "Гастрит"));

            clinic.MedicalRecords.Add(new LabResult(3, 3, DateTime.Today.AddDays(-1), "Глюкоза", 4.8, "ммоль/л", "3.3–5.5", true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Помилка ініціалізації даних]: {ex.Message}");
        }
    }

    private static void PatientsMenu(Clinic clinic)
    {
        Console.WriteLine("--- Управління пацієнтами ---");
        clinic.Patients.DisplayAll();
    }

    private static void DoctorsMenu(Clinic clinic)
    {
        Console.WriteLine("--- Управління лікарями ---");
        clinic.Doctors.DisplayAll();
    }

    private static void AppointmentsMenu(Clinic clinic)
    {
        Console.WriteLine("--- Запис на прийом ---");
        clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
    }

    private static void MedicalRecordsMenu(Clinic clinic)
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine("\n--- Медична картка ---");
            Console.WriteLine("1. Картка пацієнта (зведення)");
            Console.WriteLine("2. Усі записи пацієнта");
            Console.WriteLine("3. Додати діагноз");
            Console.WriteLine("4. Додати аналіз");
            Console.WriteLine("5. Додати рецепт");
            Console.WriteLine("6. Записи лікаря");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть пункт: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    DisplayPatientSummaryUi(clinic);
                    break;
                case "2":
                    DisplayPatientRecordsPolymorphicUi(clinic);
                    break;
                case "3":
                    AddDiagnosisUi(clinic);
                    break;
                case "4":
                    AddLabResultUi(clinic);
                    break;
                case "5":
                    AddPrescriptionUi(clinic);
                    break;
                case "6":
                    DisplayDoctorRecordsUi(clinic);
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Console.WriteLine("Некоректний вибір.");
                    break;
            }
        }
    }

    private static void DisplayPatientSummaryUi(Clinic clinic)
    {
        clinic.Patients.DisplayAll();
        Console.Write("\nВведіть ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine("Помилка: Введено некоректний ID.");
            return;
        }

        if (clinic.Patients.FindById(patientId) == null)
        {
            Console.WriteLine("Помилка: Пацієнта з таким ID не знайдено.");
            return;
        }

        clinic.MedicalRecords.DisplayPatientSummary(patientId);
    }

    private static void DisplayPatientRecordsPolymorphicUi(Clinic clinic)
    {
        clinic.Patients.DisplayAll();
        Console.Write("\nВведіть ID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine("Помилка: Введено некоректний ID.");
            return;
        }

        if (clinic.Patients.FindById(patientId) == null)
        {
            Console.WriteLine("Помилка: Пацієнта з таким ID не знайдено.");
            return;
        }

        MedicalRecord[] records = clinic.MedicalRecords.GetByPatient(patientId);
        Console.WriteLine($"\n--- Демонстрація поліморфізму (Записи пацієнта #{patientId}) ---");
        clinic.MedicalRecords.DisplayList(records);
    }

    private static void AddDiagnosisUi(Clinic clinic)
    {
        Console.WriteLine("--- Списки для вибору ---");
        clinic.Patients.DisplayAll();
        clinic.Doctors.DisplayAll();

        Console.Write("\nID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int pId) || clinic.Patients.FindById(pId) == null)
        {
            Console.WriteLine("Помилка: Пацієнта не знайдено.");
            return;
        }

        Console.Write("ID лікаря: ");
        if (!int.TryParse(Console.ReadLine(), out int dId) || clinic.Doctors.FindById(dId) == null)
        {
            Console.WriteLine("Помилка: Лікаря не знайдено.");
            return;
        }

        Console.Write("Код діагнозу (напр. J06.9): ");
        string? code = Console.ReadLine();

        Console.Write("Опис: ");
        string? desc = Console.ReadLine();

        Console.Write("Хронічне? (1 = так, 0 = ні): ");
        string? chronicInput = Console.ReadLine();
        bool isChronic = chronicInput == "1";

        try
        {
            Diagnosis diag = new Diagnosis(pId, dId, DateTime.Today, code ?? "", desc ?? "", isChronic);
            clinic.MedicalRecords.Add(diag);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    private static void AddLabResultUi(Clinic clinic)
    {
        Console.WriteLine("--- Списки для вибору ---");
        clinic.Patients.DisplayAll();
        clinic.Doctors.DisplayAll();

        Console.Write("\nID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int pId) || clinic.Patients.FindById(pId) == null)
        {
            Console.WriteLine("Помилка: Пацієнта не знайдено.");
            return;
        }

        Console.Write("ID лікаря: ");
        if (!int.TryParse(Console.ReadLine(), out int dId) || clinic.Doctors.FindById(dId) == null)
        {
            Console.WriteLine("Помилка: Лікаря не знайдено.");
            return;
        }

        Console.Write("Назва аналізу: ");
        string? testName = Console.ReadLine();

        Console.Write("Значення (число з крапкою): ");
        string? valStr = Console.ReadLine();
        if (!double.TryParse(valStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
        {
            Console.WriteLine("Помилка: Некоректне значення аналізу.");
            return;
        }

        Console.Write("Одиниці виміру: ");
        string? unit = Console.ReadLine();

        Console.Write("Норма (текст, напр. 4.0–9.0): ");
        string? refRange = Console.ReadLine();

        Console.Write("В нормі? (1 = так, 0 = ні): ");
        string? normalInput = Console.ReadLine();
        bool isNormal = normalInput == "1";

        try
        {
            LabResult lr = new LabResult(pId, dId, DateTime.Today, testName ?? "", val, unit ?? "", refRange ?? "", isNormal);
            clinic.MedicalRecords.Add(lr);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    private static void AddPrescriptionUi(Clinic clinic)
    {
        Console.WriteLine("--- Списки для вибору ---");
        clinic.Patients.DisplayAll();
        clinic.Doctors.DisplayAll();

        Console.Write("\nID пацієнта: ");
        if (!int.TryParse(Console.ReadLine(), out int pId) || clinic.Patients.FindById(pId) == null)
        {
            Console.WriteLine("Помилка: Пацієнта не знайдено.");
            return;
        }

        Console.Write("ID лікаря: ");
        if (!int.TryParse(Console.ReadLine(), out int dId) || clinic.Doctors.FindById(dId) == null)
        {
            Console.WriteLine("Помилка: Лікаря не знайдено.");
            return;
        }

        Console.Write("Препарат: ");
        string? med = Console.ReadLine();

        Console.Write("Дозування (напр. 10 мг): ");
        string? dosage = Console.ReadLine();

        Console.Write("Кількість днів (ціле число): ");
        if (!int.TryParse(Console.ReadLine(), out int duration))
        {
            Console.WriteLine("Помилка: Некоректна кількість днів.");
            return;
        }

        Console.Write("Інструкція (Enter — пропустити): ");
        string? instructions = Console.ReadLine();

        try
        {
            Prescription rx = new Prescription(pId, dId, DateTime.Today, med ?? "", dosage ?? "", duration, instructions ?? "");
            clinic.MedicalRecords.Add(rx);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    private static void DisplayDoctorRecordsUi(Clinic clinic)
    {
        clinic.Doctors.DisplayAll();
        Console.Write("\nВведіть ID лікаря: ");
        if (!int.TryParse(Console.ReadLine(), out int doctorId))
        {
            Console.WriteLine("Помилка: Введено некоректний ID.");
            return;
        }

        if (clinic.Doctors.FindById(doctorId) == null)
        {
            Console.WriteLine("Помилка: Лікаря з таким ID не знайдено.");
            return;
        }

        MedicalRecord[] records = clinic.MedicalRecords.GetByDoctor(doctorId);
        Console.WriteLine($"\n--- Записи лікаря #{doctorId} ---");
        clinic.MedicalRecords.DisplayList(records);
    }

    private static void DisplayReport(Clinic clinic)
    {
        Console.WriteLine("=== Загальний звіт клініки ===");
        Console.WriteLine($"Всього пацієнтів: {clinic.Patients.Count}");
        Console.WriteLine($"Всього лікарів: {clinic.Doctors.Count}");
        Console.WriteLine($"Всього записів на прийом: {clinic.Appointments.Count}");
        Console.WriteLine($"Всього медичних записів: {clinic.MedicalRecords.Count}");
    }
}