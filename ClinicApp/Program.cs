namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Лабораторна робота 04 — Члени класу ===");

        Clinic clinic = new Clinic("Медичний центр 'Здоров'я'");

        // 1. Створення пацієнтів з BloodType enum
        clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 3, 15), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 7, 22), BloodType.BPositive, "0672345678"));
        clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 11, 5), BloodType.ONegative, "0933456789"));

        // 2. Створення лікарів з Speciality enum та WorkSchedule
        Doctor doc1 = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567");
        doc1.Schedule = new WorkSchedule(8, 16);
        clinic.Doctors.Add(doc1);

        Doctor doc2 = new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678");
        clinic.Doctors.Add(doc2);

        Doctor doc3 = new Doctor("Андрій", "Власенко", Speciality.Cardiology, "LIC-003", "0443456789");
        clinic.Doctors.Add(doc3);

        // 3. Записи на прийом
        DateTime testDate = new DateTime(2026, 5, 10);
        clinic.Appointments.Book(1, 1, testDate.AddHours(9), 30);
        clinic.Appointments.Book(2, 3, testDate.AddHours(10), 30);

        Console.WriteLine("\n--- Тест 1: ClinicFormatter та BloodType ---");
        Console.WriteLine($"BloodType.APositive через форматер: {ClinicFormatter.FormatBloodType(BloodType.APositive)}");
        Console.WriteLine($"Пацієнт 1 ToString(): {clinic.Patients[0]}");

        Console.WriteLine("\n--- Тест 2: WorkSchedule.ToString() ---");
        Console.WriteLine($"Doctor.Schedule.ToString(): {doc1.Schedule}");

        Console.WriteLine("\n--- Тест 3: Індексатори this[int index] ---");
        Patient? firstPatient = clinic.Patients[0];
        Console.WriteLine($"clinic.Patients[0]: {firstPatient?.FullName ?? "null"}");
        Patient? invalidPatient = clinic.Patients[999];
        Console.WriteLine($"clinic.Patients[999] == null: {invalidPatient == null}");

        Doctor? firstDoctor = clinic.Doctors[0];
        Console.WriteLine($"clinic.Doctors[0]: {firstDoctor?.FullName ?? "null"}");
        Doctor? invalidDoctor = clinic.Doctors[999];
        Console.WriteLine($"clinic.Doctors[999] == null: {invalidDoctor == null}");

        Console.WriteLine("\n--- Тест 4: Перевантаження FindBySpeciality ---");
        Console.WriteLine("1) За enum (Speciality.Cardiology):");
        Doctor[] cardiologistsEnum = clinic.Doctors.FindBySpeciality(Speciality.Cardiology);
        foreach (var d in cardiologistsEnum)
        {
            Console.WriteLine($"  {d}");
        }

        Console.WriteLine("2) За рядком (\"кардіо\"):");
        Doctor[] cardiologistsString = clinic.Doctors.FindBySpeciality("кардіо");
        foreach (var d in cardiologistsString)
        {
            Console.WriteLine($"  {d}");
        }

        Console.WriteLine("\n--- Тест 5: Перевантаження AppointmentManager.GetByDate ---");
        Appointment[] appsByDate = clinic.Appointments.GetByDate(2026, 5, 10);
        Console.WriteLine($"Записів на 10.05.2026: {appsByDate.Length}");
        clinic.Appointments.DisplayList(appsByDate);

        Console.WriteLine("\n--- Тест 6: TryFindById з out-параметром ---");
        if (clinic.Patients.TryFindById(1, out Patient? foundPatient))
        {
            Console.WriteLine($"Успішно знайдено пацієнта: {foundPatient.FullName}");
        }
        else
        {
            Console.WriteLine("Пацієнта не знайдено.");
        }

        bool tryFail = clinic.Patients.TryFindById(99, out Patient? notFoundPatient);
        Console.WriteLine($"TryFindById(99) успіх: {tryFail}, об'єкт: {(notFoundPatient == null ? "null" : notFoundPatient.FullName)}");

        Console.WriteLine("\n--- Тест 7: Оператори ?. та ?? ---");
        string safeNameFound = clinic.Patients.FindById(1)?.FullName ?? "не знайдено";
        Console.WriteLine($"FindById(1)?.FullName ?? \"не знайдено\": {safeNameFound}");

        string safeNameNotFound = clinic.Patients.FindById(99)?.FullName ?? "не знайдено";
        Console.WriteLine($"FindById(99)?.FullName ?? \"не знайдено\": {safeNameNotFound}");

        Console.WriteLine("\n--- Тест 8: FindByBloodType ---");
        Patient[] aPosPatients = clinic.Patients.FindByBloodType(BloodType.APositive);
        Console.WriteLine($"Пацієнтів з A+: {aPosPatients.Length}");

        Console.WriteLine("\n--- Тест 9: Форматування віку (FormatAge) ---");
        int[] sampleAges = { 1, 3, 11, 21, 111 };
        foreach (int age in sampleAges)
        {
            Console.WriteLine($"Вік {age} -> {ClinicFormatter.FormatAge(age)}");
        }
    }
}