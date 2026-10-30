namespace ClinicApp.Models;

public class RegularAppointment : Appointment
{
    public RegularAppointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes)
        : base(patientId, doctorId, scheduledAt, durationMinutes)
    {
    }

    public RegularAppointment(int patientId, int doctorId, DateTime scheduledAt)
        : base(patientId, doctorId, scheduledAt, 30)
    {
    }

    public override string GetDescription()
    {
        return "Звичайний прийом";
    }
}