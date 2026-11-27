using System.Text;
using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionEventTracker
{
    private readonly ClinicApp.Clinic _clinic;
    public int PatientsAdded { get; private set; }
    public int AppointmentsBooked { get; private set; }
    public int UrgentBooked { get; private set; }
    public int AppointmentsCancelled { get; private set; }
    public int AppointmentsCompleted { get; private set; }
    public int PaymentsReceived { get; private set; }
    public int PlansCompleted { get; private set; }

    public SessionEventTracker(ClinicApp.Clinic clinic) => _clinic = clinic;
    public void OnPatientAdded(object? sender, PatientEventArgs e) => PatientsAdded++;
    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e) => AppointmentsBooked++;
    public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e) => UrgentBooked++;
    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e)
    {
        AppointmentsCancelled++;
        if (!_clinic.WaitingRoom.IsEmpty) Console.WriteLine($"[ЧЕРГА] Слот звільнився. Наступний: {_clinic.WaitingRoom.Peek().FullName}");
    }
    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) => AppointmentsCompleted++;
    public void OnPaymentReceived(object? sender, PaymentEventArgs e) => PaymentsReceived++;
    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) => PlansCompleted++;

    private string BuildSummary()
    {
        return $"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm:ss}{Environment.NewLine}" +
               "=== Підсумок сесії ===" + Environment.NewLine +
               $"Пацієнтів додано:    {PatientsAdded}{Environment.NewLine}" +
               $"Записів створено:    {AppointmentsBooked} (термінових: {UrgentBooked}){Environment.NewLine}" +
               $"Скасовано:           {AppointmentsCancelled}{Environment.NewLine}" +
               $"Завершено:           {AppointmentsCompleted}{Environment.NewLine}" +
               $"Оплат:               {PaymentsReceived}{Environment.NewLine}" +
               $"Планів завершено:    {PlansCompleted}{Environment.NewLine}";
    }
    public void PrintSummary() => Console.WriteLine(BuildSummary());
    public void SaveSummary(string path = "session_summary.txt") => File.WriteAllText(path, BuildSummary(), Encoding.UTF8);
}
