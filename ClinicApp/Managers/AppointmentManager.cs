using ClinicApp.Models;
using ClinicApp.Enums;
using ClinicApp.Interfaces;
using System.Globalization;

namespace ClinicApp.Managers;

public class AppointmentManager
{
    private const int MaxAppointments = 500;
    private readonly Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count;

    private readonly PatientManager _patients;
    private readonly DoctorManager _doctors;

    public int Count => _count;

    public Appointment? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count) return null;
            return _appointments[index];
        }
    }

    public AppointmentManager(PatientManager patients, DoctorManager doctors)
    {
        _patients = patients;
        _doctors = doctors;
    }

    private bool AddAppointment(Appointment appointment, int patientId, int doctorId)
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

        _appointments[_count] = appointment;
        _count++;

        Patient p = _patients.FindById(patientId)!;
        Doctor d = _doctors.FindById(doctorId)!;
        Console.WriteLine($"Запис [{appointment.Id}] створено: {p.FullName} -> {d.FullName} о {appointment.ScheduledAt:dd.MM.yyyy HH:mm}");
        return true;
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Appointment appointment = new RegularAppointment(patientId, doctorId, scheduledAt, durationMinutes);
        return AddAppointment(appointment, patientId, doctorId);
    }

    public bool BookUrgent(int patientId, int doctorId, DateTime scheduledAt, string urgencyNote = "", int durationMinutes = 30)
    {
        Appointment appointment = new UrgentAppointment(patientId, doctorId, scheduledAt, urgencyNote, durationMinutes);
        return AddAppointment(appointment, patientId, doctorId);
    }

    public bool BookSpecialist(int patientId, int doctorId, DateTime scheduledAt, string topic = "", int durationMinutes = 45)
    {
        Appointment appointment = new SpecialistAppointment(patientId, doctorId, scheduledAt, topic, durationMinutes);
        return AddAppointment(appointment, patientId, doctorId);
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

        if (app.Status == AppointmentStatus.Scheduled)
        {
            app.Complete();
            return true;
        }
        return false;
    }

    public static int CancelAll(ICancellable[] items, string reason = "")
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item.Cancel(reason))
            {
                count++;
            }
        }
        return count;
    }

    public Appointment[] GetAll()
    {
        Appointment[] result = new Appointment[_count];
        for (int i = 0; i < _count; i++)
        {
            result[i] = _appointments[i];
        }
        return result;
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

    private Appointment[] GetByDate(DateTime date)
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

    public Appointment[] GetByDate(int year, int month, int day)
    {
        return GetByDate(new DateTime(year, month, day));
    }

    public Appointment[] GetUpcoming()
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt > DateTime.Now && _appointments[i].Status == AppointmentStatus.Scheduled) matchCount++;
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt > DateTime.Now && _appointments[i].Status == AppointmentStatus.Scheduled)
            {
                result[index++] = _appointments[i];
            }
        }
        return result;
    }

    public UrgentAppointment[] GetUrgent()
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment) matchCount++;
        }

        UrgentAppointment[] result = new UrgentAppointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment urgent)
            {
                result[index++] = urgent;
            }
        }
        return result;
    }

    public SpecialistAppointment[] GetSpecialist()
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is SpecialistAppointment) matchCount++;
        }

        SpecialistAppointment[] result = new SpecialistAppointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is SpecialistAppointment specialist)
            {
                result[index++] = specialist;
            }
        }
        return result;
    }

    public RegularAppointment[] GetRegular()
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment) matchCount++;
        }

        RegularAppointment[] result = new RegularAppointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment regular)
            {
                result[index++] = regular;
            }
        }
        return result;
    }

    private void DisplayAppointment(Appointment app)
    {
        Patient? p = _patients.FindById(app.PatientId);
        Doctor? d = _doctors.FindById(app.DoctorId);

        string patientName = p != null ? p.FullName : $"Пацієнт #{app.PatientId}";
        string doctorName = d != null ? d.FullName : $"Лікар #{app.DoctorId}";

        string startStr = app.ScheduledAt.ToString("dd.MM.yyyy HH:mm");
        string endStr = app.ScheduledAt.AddMinutes(app.DurationMinutes).ToString("HH:mm");

        string line = $"[{app.Id}] {app.GetDescription()} | {patientName} -> {doctorName} | {startStr}–{endStr} | {app.Status} | {app.GetCost().ToString("F2", CultureInfo.InvariantCulture)} грн";
        if (!string.IsNullOrEmpty(app.CancellationReason))
        {
            line += $" | {app.CancellationReason}";
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