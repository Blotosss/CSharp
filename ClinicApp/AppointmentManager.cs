namespace ClinicApp;

public class AppointmentManager
{
    private const int MaxAppointments = 500;
    private Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count = 0;

    private PatientManager _patients;
    private DoctorManager _doctors;

    public int Count => _count;

    public AppointmentManager(PatientManager patients, DoctorManager doctors)
    {
        _patients = patients;
        _doctors = doctors;
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        if (_patients.FindById(patientId) == null)
        {
            Console.WriteLine($"Помилка: пацієнта з ID {patientId} не знайдено.");
            return false;
        }

        if (_doctors.FindById(doctorId) == null)
        {
            Console.WriteLine($"Помилка: лікаря з ID {doctorId} не знайдено.");
            return false;
        }

        if (_count >= MaxAppointments)
        {
            Console.WriteLine("Помилка: досягнуто ліміт записів!");
            return false;
        }

        Appointment appointment = new Appointment(patientId, doctorId, scheduledAt, durationMinutes);
        _appointments[_count] = appointment;
        _count++;

        Patient p = _patients.FindById(patientId)!;
        Doctor d = _doctors.FindById(doctorId)!;
        Console.WriteLine($"Запис [{appointment.Id}] створено: {p.FullName} -> {d.FullName} о {scheduledAt:dd.MM.yyyy HH:mm}");
        return true;
    }

    private Appointment? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id)
            {
                return _appointments[i];
            }
        }
        return null;
    }

    public bool Cancel(int id, string reason = "")
    {
        Appointment? app = FindById(id);
        if (app == null) return false;

        bool success = app.Cancel(reason);
        if (success)
        {
            Console.WriteLine($"Запис [{id}] скасовано.");
        }
        return success;
    }

    public bool Complete(int id)
    {
        Appointment? app = FindById(id);
        if (app == null) return false;

        return app.Complete();
    }

    public Appointment[] GetByPatient(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
            {
                result[index++] = _appointments[i];
            }
        }
        return result;
    }

    public Appointment[] GetByDoctor(int doctorId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
            {
                result[index++] = _appointments[i];
            }
        }
        return result;
    }

    public Appointment[] GetByDate(DateTime date)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
            {
                result[index++] = _appointments[i];
            }
        }
        return result;
    }

    public Appointment[] GetUpcoming()
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
            {
                result[index++] = _appointments[i];
            }
        }
        return result;
    }

    public void DisplayAppointment(Appointment app)
    {
        Patient? p = _patients.FindById(app.PatientId);
        Doctor? d = _doctors.FindById(app.DoctorId);

        string patientName = p != null ? p.FullName : $"Пацієнт #{app.PatientId}";
        string doctorName = d != null ? d.FullName : $"Лікар #{app.DoctorId}";

        string startStr = app.ScheduledAt.ToString("dd.MM.yyyy HH:mm");
        string endStr = app.EndsAt.ToString("HH:mm");

        string line = $"[{app.Id}] {patientName} -> {doctorName} | {startStr}–{endStr} | {app.Status}";
        if (!string.IsNullOrEmpty(app.Notes))
        {
            line += $" | {app.Notes}";
        }

        Console.WriteLine(line);
    }

    public void DisplayList(Appointment[] list)
    {
        if (list.Length == 0)
        {
            Console.WriteLine("Записів не знайдено.");
            return;
        }

        foreach (var app in list)
        {
            DisplayAppointment(app);
        }
    }
}