using System.Globalization;
using System.Text;
using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class PatientPassportWriter
{
    private readonly ClinicApp.Clinic _clinic;
    private readonly string _directory;

    public PatientPassportWriter(ClinicApp.Clinic clinic, string directory = "patients")
    {
        _clinic = clinic;
        _directory = directory;
        Directory.CreateDirectory(_directory);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e) => WritePassport(e.PatientId);
    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) => WritePassport(e.PatientId);
    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) => WritePassport(e.PatientId);

    private void WritePassport(int patientId)
    {
        Patient? patient = _clinic.Patients.FindById(patientId);
        if (patient == null) return;

        StringBuilder text = new StringBuilder();
        text.AppendLine($"=== Паспорт пацієнта #{patient.Id} ===");
        text.AppendLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        text.AppendLine();
        text.AppendLine($"Ім'я: {patient.FullName}");
        text.AppendLine($"Дата народження: {patient.DateOfBirth:dd.MM.yyyy} ({patient.Age} років)");
        text.AppendLine($"Група крові: {ClinicFormatter.FormatBloodType(patient.BloodType)}   Телефон: {ClinicFormatter.FormatPhone(patient.Phone)}");
        text.AppendLine();
        text.AppendLine("-- Діагнози --");
        foreach (Diagnosis diagnosis in _clinic.MedicalRecords.GetDiagnoses(patientId)) text.AppendLine(diagnosis.GetSummary());
        text.AppendLine("-- Аналізи --");
        foreach (LabResult result in _clinic.MedicalRecords.GetLabResults(patientId)) text.AppendLine(result.GetSummary());
        text.AppendLine("-- Рецепти --");
        foreach (Prescription prescription in _clinic.MedicalRecords.GetPrescriptions(patientId)) text.AppendLine(prescription.GetSummary());
        text.AppendLine();
        text.AppendLine("-- Записи --");
        foreach (Appointment appointment in _clinic.Appointments.GetByPatient(patientId)) text.AppendLine(appointment.ToString());
        text.AppendLine();
        text.AppendLine("-- Плани лікування --");
        foreach (TreatmentPlan plan in _clinic.TreatmentPlans.GetByPatient(patientId)) text.AppendLine(plan.ToString());
        text.AppendLine();
        text.AppendLine($"Заборгованість: {_clinic.Billing.GetPatientDebt(patientId).ToString("F2", CultureInfo.InvariantCulture)} грн");
        File.WriteAllText(Path.Combine(_directory, $"passport_{patientId}.txt"), text.ToString(), Encoding.UTF8);
    }
}
