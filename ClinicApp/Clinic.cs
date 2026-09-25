namespace ClinicApp;

public class Clinic
{
    public string Name { get; set; }
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }

    public Clinic(string name)
    {
        Name = name;
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
    }

    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine($"=== Розклад на {date:dd.MM.yyyy} ===");
        Appointment[] list = Appointments.GetByDate(date);
        Appointments.DisplayList(list);
    }

    public void GenerateReport()
    {
        Appointment[] upcoming = Appointments.GetUpcoming();
        Doctor[] allDoctors = Doctors.GetAll();

        Console.WriteLine("╔══════════════════════════════════════════════════════╗");
        Console.WriteLine($"║ Звіт – {Name,-45} ║");
        Console.WriteLine("╠══════════════════════════════════════════════════════╣");
        Console.WriteLine($"║ Пацієнтів:          {Patients.Count,-32} ║");
        Console.WriteLine($"║ Лікарів:            {Doctors.Count,-32} ║");
        Console.WriteLine($"║ Майбутніх записів:  {upcoming.Length,-32} ║");
        Console.WriteLine("╠══════════════════════════════════════════════════════╣");
        Console.WriteLine("║ Навантаження лікарів (майбутні записи):              ║");

        foreach (var doc in allDoctors)
        {
            int count = 0;
            for (int i = 0; i < upcoming.Length; i++)
            {
                if (upcoming[i].DoctorId == doc.Id)
                {
                    count++;
                }
            }

            string line = $"  {doc.FullName} ({doc.Speciality}): {count} записів";
            Console.WriteLine($"║ {line,-52} ║");
        }

        Console.WriteLine("╚══════════════════════════════════════════════════════╝");
    }
}