using System.Text;
using ClinicApp.Events;

namespace ClinicApp.Utils;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e) => LogInfo($"Новий пацієнт #{e.PatientId}: {e.FullName}");
    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e) => LogInfo($"Запис #{e.AppointmentId} створено: пацієнт #{e.PatientId} → лікар #{e.DoctorId}");
    public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        string message = $"ТЕРМІНОВИЙ запис #{e.AppointmentId}: {e.Notes}";
        LogWarning(message);
        Directory.CreateDirectory("alerts");
        File.AppendAllText(Path.Combine("alerts", $"urgent_{DateTime.Now:yyyy-MM-dd}.txt"), $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}", Encoding.UTF8);
    }
    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e) => LogInfo($"Запис #{e.AppointmentId} скасовано.");
    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) => LogInfo($"Запис #{e.AppointmentId} завершено.");
    public void OnPaymentReceived(object? sender, PaymentEventArgs e) => LogInfo($"Оплата запису #{e.AppointmentId}: {e.Amount:F2} грн");
    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) => LogInfo($"План #{e.PlanId} завершено: {e.Diagnosis}");

    public void LogInfo(string message) => Write("INFO ", message);
    public void LogWarning(string message) => Write("WARN ", message);
    public void LogError(string message) => Write("ERROR", message);

    private void Write(string level, string message)
    {
        string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}";
        File.AppendAllText(_logPath, line, Encoding.UTF8);
    }

    public string[] GetLastLines(int n)
    {
        if (n <= 0 || !File.Exists(_logPath)) return Array.Empty<string>();
        string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);
        int skip = Math.Max(0, lines.Length - n);
        string[] result = new string[lines.Length - skip];
        Array.Copy(lines, skip, result, 0, result.Length);
        return result;
    }

    public void Clear()
    {
        if (File.Exists(_logPath)) File.Delete(_logPath);
    }

    public bool Exists() => File.Exists(_logPath);
}
