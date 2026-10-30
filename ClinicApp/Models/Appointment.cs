using ClinicApp.Enums;
using ClinicApp.Interfaces;

namespace ClinicApp.Models;

public class Appointment : IPayable, ICancellable
{
    private static int _nextId = 1;

    public int Id { get; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public AppointmentStatus Status { get; set; }
    public string CancellationReason { get; private set; } = "";
    public bool IsPaid { get; private set; }

    public bool IsCancelled => Status == AppointmentStatus.Cancelled;

    public decimal DiscountMultiplier { get; set; } = 1.0m;

    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        Status = AppointmentStatus.Scheduled;
        IsPaid = false;
        Id = _nextId++;
    }

    public Appointment(int patientId, int doctorId, DateTime scheduledAt)
        : this(patientId, doctorId, scheduledAt, 30)
    {
    }

    public Appointment()
        : this(0, 0, DateTime.Now, 30)
    {
    }

    public virtual decimal GetCost()
    {
        return (DurationMinutes * 10m) * DiscountMultiplier;
    }

    public virtual string GetDescription()
    {
        return "Прийом";
    }

    public int GetPriority()
    {
        return 3;
    }

    public void MarkPaid()
    {
        if (Status == AppointmentStatus.Cancelled) return;
        IsPaid = true;
    }

    public bool Cancel(string reason)
    {
        if (Status == AppointmentStatus.Completed || Status == AppointmentStatus.Cancelled)
        {
            return false;
        }
        Status = AppointmentStatus.Cancelled;
        CancellationReason = reason ?? "";
        return true;
    }

    public bool Cancel()
    {
        return Cancel("");
    }

    public void Complete()
    {
        if (Status == AppointmentStatus.Scheduled)
        {
            Status = AppointmentStatus.Completed;
        }
    }

    public override string ToString()
    {
        string statusStr = Status switch
        {
            AppointmentStatus.Scheduled => "Scheduled",
            AppointmentStatus.Completed => "Completed",
            AppointmentStatus.Cancelled => "Cancelled",
            _ => "Unknown"
        };

        string timeFormatted = $"{ScheduledAt:dd.MM.yyyy HH:mm}-{ScheduledAt.AddMinutes(DurationMinutes):HH:mm}";
        string costFormatted = $"{GetCost().ToString("F2", System.Globalization.CultureInfo.InvariantCulture)} грн";

        return $"[{Id}] {GetDescription()} | Пацієнт #{PatientId} → Лікар #{DoctorId} | {timeFormatted} | {statusStr} | {costFormatted}";
    }
}