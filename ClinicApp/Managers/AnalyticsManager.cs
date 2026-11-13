using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AnalyticsManager
{
    private readonly AppointmentManager _appointments;
    private readonly DoctorManager _doctors;
    private readonly PatientManager _patients;

    public AnalyticsManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        Doctor[] doctors = _doctors.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        foreach (Doctor doctor in doctors)
        {
            int count = 0;
            decimal totalRevenue = 0m;
            DateTime lastDate = DateTime.MinValue;

            foreach (Appointment app in appointments)
            {
                if (app.DoctorId == doctor.Id)
                {
                    count++;
                    totalRevenue += app.GetCost();
                    if (app.ScheduledAt > lastDate)
                    {
                        lastDate = app.ScheduledAt;
                    }
                }
            }

            yield return new DoctorStats(doctor.Id, doctor.FullName, count, totalRevenue, lastDate);
        }
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Patient[] patients = _patients.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        foreach (Patient patient in patients)
        {
            int count = 0;
            decimal totalSpent = 0m;
            DateTime lastDate = DateTime.MinValue;

            foreach (Appointment app in appointments)
            {
                if (app.PatientId == patient.Id)
                {
                    count++;
                    totalSpent += app.GetCost();
                    if (app.ScheduledAt > lastDate)
                    {
                        lastDate = app.ScheduledAt;
                    }
                }
            }

            yield return new PatientStats(patient.Id, patient.FullName, count, totalSpent, lastDate);
        }
    }
}
