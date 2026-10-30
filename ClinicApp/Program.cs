using System.Globalization;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Enums;
using ClinicApp.Interfaces;

namespace ClinicApp;

class Program
{
    static void Main(string[] args)
    {
        Clinic clinic = new Clinic("Медична клініка");

        while (true)
        {
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║                  МЕДИЧНА КЛІНІКА               ║");
            Console.WriteLine("╠════════════════════════════════════════════════╣");
            Console.WriteLine("║ 1. Пацієнти       — реєстрація, пошук          ║");
            Console.WriteLine("║ 2. Лікарі         — персонал, розклад          ║");
            Console.WriteLine("║ 3. Записи         — прийоми, скасування        ║");
            Console.WriteLine("║ 4. Медична картка — діагнози, рецепти          ║");
            Console.WriteLine("║ 5. Рахунки        — оплата, борги              ║");
            Console.WriteLine("║ 6. Звіт           — загальна статистика        ║");
            Console.WriteLine("║ 0. Вийти                                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
            Console.Write("Оберіть розділ: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Невірний ввід. Спробуйте ще раз.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    PatientsMenu(clinic);
                    break;
                case 2:
                    DoctorsMenu(clinic);
                    break;
                case 3:
                    AppointmentsMenu(clinic);
                    break;
                case 4:
                    MedicalRecordsMenu(clinic);
                    break;
                case 5:
                    BillingMenu(clinic);
                    break;
                case 6:
                    ShowReport(clinic);
                    break;
                case 0:
                    Console.WriteLine("Вихід з програми...");
                    return;
                default:
                    Console.WriteLine("Невідомий розділ. Спробуйте знову.");
                    break;
            }
        }
    }

    private static void PatientsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n— Пацієнти ————————————————————————");
            Console.WriteLine("1. Зареєструвати пацієнта");
            Console.WriteLine("2. Знайти пацієнта за ID");
            Console.WriteLine("3. Вивести всіх пацієнтів");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            switch (choice)
            {
                case 1:
                    Console.Write("Ім'я: ");
                    string fn = Console.ReadLine() ?? "";
                    Console.Write("Прізвище: ");
                    string ln = Console.ReadLine() ?? "";
                    Console.Write("Дата народження (dd.MM.yyyy): ");
                    string dobStr = Console.ReadLine() ?? "";
                    if (DateTime.TryParseExact(dobStr, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
                    {
                        Console.WriteLine("Група крові (0 - Unknown, 1 - OPositive, 2 - ONegative, 3 - APositive, 4 - ANegative, 5 - BPositive, 6 - BNegative, 7 - ABPositive, 8 - ABNegative): ");
                        if (int.TryParse(Console.ReadLine(), out int bloodTypeInt))
                        {
                            Console.Write("Телефон: ");
                            string ph = Console.ReadLine() ?? "";
                            clinic.Patients.Add(new Patient(fn, ln, dob, (BloodType)bloodTypeInt, ph));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некоректний формат дати.");
                    }
                    break;
                case 2:
                    Console.Write("ID пацієнта: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var p = clinic.Patients.FindById(id);
                        if (p != null) Console.WriteLine(p);
                        else Console.WriteLine("Пацієнта не знайдено.");
                    }
                    break;
                case 3:
                    clinic.Patients.DisplayAll();
                    break;
                case 0:
                    return;
            }
        }
    }

    private static void DoctorsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n— Лікарі ——————————————————————————");
            Console.WriteLine("1. Додати лікаря");
            Console.WriteLine("2. Вивести всіх лікарів");
            Console.WriteLine("3. Перевірити доступність лікаря зараз");
            Console.WriteLine("4. Змінити розклад лікаря");
            Console.WriteLine("5. Вільні години лікаря");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            switch (choice)
            {
                case 1:
                    Console.Write("Ім'я: ");
                    string fn = Console.ReadLine() ?? "";
                    Console.Write("Прізвище: ");
                    string ln = Console.ReadLine() ?? "";
                    Console.WriteLine("Спеціальність (0 - General, 1 - Surgeon, 2 - Dentist, 3 - Therapist, 4 - Neurologist): ");
                    if (int.TryParse(Console.ReadLine(), out int spec))
                    {
                        clinic.Doctors.Add(new Doctor(fn, ln, (Speciality)spec));
                    }
                    break;
                case 2:
                    clinic.Doctors.DisplayAll();
                    break;
                case 3:
                    Console.Write("ID лікаря: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var d = clinic.Doctors.FindById(id);
                        if (d != null)
                        {
                            Console.WriteLine(d.IsAvailableNow ? "Доступний зараз" : "Не в робочий час");
                        }
                    }
                    break;
                case 4:
                    Console.Write("ID лікаря: ");
                    if (int.TryParse(Console.ReadLine(), out int docId))
                    {
                        var d = clinic.Doctors.FindById(docId);
                        if (d != null)
                        {
                            Console.Write("Початок та кінець розкладу через пробіл: ");
                            string[] parts = (Console.ReadLine() ?? "").Split(' ');
                            if (parts.Length == 2 && int.TryParse(parts[0], out int start) && int.TryParse(parts[1], out int end))
                            {
                                d.Schedule = new WorkSchedule(start, end);
                                Console.WriteLine("Розклад оновлено.");
                            }
                        }
                    }
                    break;
                case 5:
                    Console.Write("ID лікаря: ");
                    if (int.TryParse(Console.ReadLine(), out int sDocId))
                    {
                        Doctor? doctor = clinic.Doctors.FindById(sDocId);
                        if (doctor == null)
                        {
                            Console.WriteLine("Лікаря не знайдено.");
                            break;
                        }
                        ISchedulable schedulable = doctor;

                        Console.Write("Дата (dd.MM.yyyy): ");
                        string dateStr = Console.ReadLine() ?? "";
                        if (DateTime.TryParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                        {
                            Console.Write("Скільки слотів: ");
                            if (int.TryParse(Console.ReadLine(), out int slotCount))
                            {
                                try
                                {
                                    DateTime[] slots = schedulable.GetAvailableSlots(parsedDate, slotCount);
                                    foreach (var slot in slots)
                                    {
                                        Console.WriteLine(slot.ToString("HH:mm"));
                                    }
                                }
                                catch (ArgumentOutOfRangeException ex)
                                {
                                    Console.WriteLine($"Помилка: {ex.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Некоректна кількість слотів.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Некоректний формат дати.");
                        }
                    }
                    break;
                case 0:
                    return;
            }
        }
    }

    private static void AppointmentsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n— Записи ——————————————————————————");
            Console.WriteLine("1. Записати на прийом");
            Console.WriteLine("2. Скасувати запис");
            Console.WriteLine("3. Завершити запис");
            Console.WriteLine("4. Всі записи");
            Console.WriteLine("5. Записи пацієнта");
            Console.WriteLine("6. Записи лікаря");
            Console.WriteLine("7. Майбутні записи");
            Console.WriteLine("8. Скасувати всі записи пацієнта");
            Console.WriteLine("9. За типом прийому");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            switch (choice)
            {
                case 1:
                    Console.Write("ID пацієнта: ");
                    if (int.TryParse(Console.ReadLine(), out int pId))
                    {
                        Console.Write("ID лікаря: ");
                        if (int.TryParse(Console.ReadLine(), out int dId))
                        {
                            Console.WriteLine("Тип прийому: 1 - Звичайний, 2 - Терміновий, 3 - Консультація спеціаліста");
                            if (int.TryParse(Console.ReadLine(), out int tChoice))
                            {
                                if (tChoice == 2)
                                {
                                    Console.Write("Причина терміновості: ");
                                    string note = Console.ReadLine() ?? "";
                                    clinic.Appointments.BookUrgent(pId, dId, DateTime.Now.AddHours(1), note);
                                }
                                else if (tChoice == 3)
                                {
                                    Console.Write("Тема консультації: ");
                                    string topic = Console.ReadLine() ?? "";
                                    clinic.Appointments.BookSpecialist(pId, dId, DateTime.Now.AddHours(1), topic);
                                }
                                else
                                {
                                    clinic.Appointments.Book(pId, dId, DateTime.Now.AddHours(1));
                                }
                            }
                        }
                    }
                    break;
                case 2:
                    Console.Write("ID запису: ");
                    if (int.TryParse(Console.ReadLine(), out int aId))
                    {
                        Console.Write("Причина скасування: ");
                        string reason = Console.ReadLine() ?? "";
                        clinic.Appointments.Cancel(aId, reason);
                    }
                    break;
                case 3:
                    Console.Write("ID запису: ");
                    if (int.TryParse(Console.ReadLine(), out int compId))
                    {
                        clinic.Appointments.Complete(compId);
                    }
                    break;
                case 4:
                    clinic.Appointments.DisplayList(clinic.Appointments.GetAll());
                    break;
                case 5:
                    Console.Write("ID пацієнта: ");
                    if (int.TryParse(Console.ReadLine(), out int patId))
                    {
                        clinic.Appointments.DisplayList(clinic.Appointments.GetByPatient(patId));
                    }
                    break;
                case 6:
                    Console.Write("ID лікаря: ");
                    if (int.TryParse(Console.ReadLine(), out int docId))
                    {
                        clinic.Appointments.DisplayList(clinic.Appointments.GetByDoctor(docId));
                    }
                    break;
                case 7:
                    clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
                    break;
                case 8:
                    Console.Write("ID пацієнта: ");
                    if (int.TryParse(Console.ReadLine(), out int cancelPatId))
                    {
                        Console.Write("Причина (Enter — без причини): ");
                        string reason = Console.ReadLine() ?? "";
                        Appointment[] patAppointments = clinic.Appointments.GetByPatient(cancelPatId);
                        int cancelledCount = AppointmentManager.CancelAll(patAppointments, reason);
                        Console.WriteLine($"Скасовано записів: {cancelledCount}");
                    }
                    break;
                case 9:
                    Console.WriteLine("1. Термінові");
                    Console.WriteLine("2. Консультації спеціаліста");
                    Console.WriteLine("3. Звичайні");
                    Console.Write("Оберіть підтип: ");
                    if (int.TryParse(Console.ReadLine(), out int typeChoice))
                    {
                        switch (typeChoice)
                        {
                            case 1:
                                clinic.Appointments.DisplayList(clinic.Appointments.GetUrgent());
                                break;
                            case 2:
                                clinic.Appointments.DisplayList(clinic.Appointments.GetSpecialist());
                                break;
                            case 3:
                                clinic.Appointments.DisplayList(clinic.Appointments.GetRegular());
                                break;
                            default:
                                Console.WriteLine("Невірний вибір.");
                                break;
                        }
                    }
                    break;
                case 0:
                    return;
            }
        }
    }

    private static void MedicalRecordsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n— Медична картка ———————————————————");
            Console.WriteLine("1. Переглянути картку пацієнта");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            switch (choice)
            {
                case 1:
                    Console.Write("ID пацієнта: ");
                    if (int.TryParse(Console.ReadLine(), out int viewId))
                    {
                        clinic.MedicalRecords.DisplayPatientSummary(viewId);
                    }
                    break;
                case 0:
                    return;
            }
        }
    }

    private static void BillingMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n— Рахунки ————————————————————————");
            Console.WriteLine("1. Борги пацієнта");
            Console.WriteLine("2. Всі неоплачені записи");
            Console.WriteLine("3. Оплатити запис");
            Console.WriteLine("4. Загальний борг клініки");
            Console.WriteLine("0. Назад");
            Console.Write("Оберіть: ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            switch (choice)
            {
                case 1:
                    Console.Write("ID пацієнта: ");
                    if (int.TryParse(Console.ReadLine(), out int patientId))
                    {
                        IPayable[] unpaid = clinic.Billing.GetUnpaidByPatient(patientId);
                        Console.WriteLine($"Неоплачені записи пацієнта #{patientId}:");
                        clinic.Billing.DisplayUnpaid(unpaid);
                        decimal debt = clinic.Billing.GetPatientDebt(patientId);
                        Console.WriteLine($"Борг: {debt.ToString("F2", CultureInfo.InvariantCulture)} грн");
                    }
                    break;
                case 2:
                    IPayable[] allUnpaid = clinic.Billing.GetAllUnpaid();
                    clinic.Billing.DisplayUnpaid(allUnpaid);
                    break;
                case 3:
                    Console.Write("ID запису для оплати: ");
                    if (int.TryParse(Console.ReadLine(), out int appId))
                    {
                        bool success = clinic.Billing.PayAppointment(appId);
                        if (success)
                        {
                            Console.WriteLine($"Запис [{appId}] оплачено.");
                        }
                        else
                        {
                            Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                        }
                    }
                    break;
                case 4:
                    decimal totalDebt = clinic.Billing.GetTotalDebt();
                    Console.WriteLine($"Загальний борг по клініці: {totalDebt.ToString("F2", CultureInfo.InvariantCulture)} грн");
                    break;
                case 0:
                    return;
            }
        }
    }

    private static void ShowReport(Clinic clinic)
    {
        Console.WriteLine("\n— Звіт ————————————————————————————");
        Console.WriteLine($"Всього пацієнтів: {clinic.Patients.Count}");
        Console.WriteLine($"Всього лікарів: {clinic.Doctors.Count}");
        Console.WriteLine($"Всього записів: {clinic.Appointments.Count}");
        Console.WriteLine($"Загальний борг клініки: {clinic.Billing.GetTotalDebt().ToString("F2", CultureInfo.InvariantCulture)} грн");
    }
}