namespace ClinicApp;

public class Appointment
{
    private static int _nextId = 1;

    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public string Status { get; private set; }
    public string Notes { get; private set; }

    public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);
    public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == "Scheduled";

    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Id = _nextId++;
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        Status = "Scheduled";
        Notes = "";
    }

    public bool Cancel(string reason = "")
    {
        if (Status != "Scheduled") return false;

        Status = "Cancelled";
        if (!string.IsNullOrEmpty(reason))
        {
            Notes = reason;
        }
        return true;
    }

    public bool Complete()
    {
        if (Status != "Scheduled") return false;

        Status = "Completed";
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