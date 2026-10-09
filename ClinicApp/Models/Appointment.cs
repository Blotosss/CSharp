using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Appointment
{
    private static int _nextId = 1;

    private int _durationMinutes;

    public int DurationMinutes
    {
        get => _durationMinutes;
        set
        {
            ClinicValidator.ValidatePositive(value, nameof(DurationMinutes));
            _durationMinutes = value;
        }
    }

    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }
    public DateTime ScheduledAt { get; set; }
    public AppointmentStatus Status { get; private set; }
    public string Notes { get; private set; }

    public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);
    public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled;

    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        Status = AppointmentStatus.Scheduled;
        Notes = "";
        Id = _nextId++;
    }

    public bool Cancel(string reason = "")
    {
        if (Status != AppointmentStatus.Scheduled) return false;

        Status = AppointmentStatus.Cancelled;
        if (!string.IsNullOrEmpty(reason))
        {
            Notes = reason;
        }
        return true;
    }

    public bool Complete()
    {
        if (Status != AppointmentStatus.Scheduled) return false;

        Status = AppointmentStatus.Completed;
        return true;
    }

    public override string ToString()
    {
        string startStr = ScheduledAt.ToString("dd.MM.yyyy HH:mm");
        string endStr = EndsAt.ToString("HH:mm");
        string baseStr = $"[{Id}] Пацієнт #{PatientId} -> Лікар #{DoctorId} | {startStr}–{endStr} | {Status}";

        if (!string.IsNullOrEmpty(Notes))
        {
            baseStr += $" | {Notes}";
        }

        return baseStr;
    }
}