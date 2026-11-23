using System.Globalization;
using System.Text;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class ClinicExporter
{
    private readonly Clinic _clinic;
    private readonly string _baseDir;
    public ClinicExporter(Clinic clinic, string baseDir = "reports") { _clinic = clinic; _baseDir = baseDir; }
    private string Prepare(string fileName, string title, string[] rows, string summary)
    {
        string dir = Path.Combine(_baseDir, DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        Directory.CreateDirectory(dir);
        string path = Path.Combine(dir, fileName);
        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
        writer.WriteLine($"=== {title} — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine(new string('-', 40));
        for (int i = 0; i < rows.Length; i++) writer.WriteLine($"{i + 1,3}. {rows[i]}");
        writer.WriteLine(new string('-', 40));
        writer.WriteLine(summary);
        return path;
    }
    public string ExportPatients()
    {
        Patient[] items = _clinic.Patients.GetAll(); string[] rows = new string[items.Length];
        for (int i = 0; i < items.Length; i++) rows[i] = items[i].ToString();
        return Prepare("patients.txt", "Пацієнти", rows, $"Всього: {rows.Length}");
    }
    public string ExportAppointments()
    {
        Appointment[] items = _clinic.Appointments.GetAll(); string[] rows = new string[items.Length];
        for (int i = 0; i < items.Length; i++) rows[i] = items[i].ToString();
        return Prepare("appointments.txt", "Записи", rows, $"Всього: {rows.Length}");
    }
    public string ExportBilling()
    {
        var items = _clinic.Billing.GetAllUnpaid(); string[] rows = new string[items.Length];
        for (int i = 0; i < items.Length; i++) rows[i] = $"{items[i]} | Борг: {items[i].GetCost().ToString("F2", CultureInfo.InvariantCulture)} грн";
        string path = Prepare("billing.txt", "Неоплачені записи", rows, $"Неоплачених: {rows.Length} | Загальний борг: {_clinic.Billing.GetTotalDebt().ToString("F2", CultureInfo.InvariantCulture)} грн");
        return path;
    }
    public string ExportTreatmentPlans()
    {
        TreatmentPlan[] items = _clinic.TreatmentPlans.GetAll(); string[] rows = new string[items.Length];
        for (int i = 0; i < items.Length; i++) rows[i] = items[i].ToString();
        return Prepare("treatment_plans.txt", "Плани лікування", rows, $"Всього: {rows.Length}");
    }
    public string[] ExportAll() => new[] { ExportPatients(), ExportAppointments(), ExportBilling(), ExportTreatmentPlans() };
}
