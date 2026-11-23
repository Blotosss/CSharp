using System.Globalization;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public static class CsvImporter
{
    public static ImportResult ImportPatients(Clinic clinic, string filePath)
    {
        ImportResult result = new ImportResult();
        if (!File.Exists(filePath)) { result.AddError(0, "Файл не знайдено."); return result; }
        string[] lines;
        try { lines = File.ReadAllLines(filePath, Encoding.UTF8); }
        catch (Exception ex) { result.AddError(0, $"Не вдалося прочитати файл: {ex.Message}"); return result; }
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            try
            {
                string[] fields = lines[i].Split(',');
                if (fields.Length != 5) throw new FormatException($"Рядок має {fields.Length} поле замість 5.");
                DateTime dob = DateTime.ParseExact(fields[2].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), fields[3].Trim(), false);
                Patient patient = new Patient(fields[0].Trim(), fields[1].Trim(), dob, bloodType, fields[4].Trim());
                clinic.Patients.Add(patient);
                clinic.Logger.LogInfo($"Імпортовано пацієнта #{patient.Id}: {patient.FullName}");
                result.AddSuccess();
            }
            catch (Exception ex) { result.AddError(i + 1, ex.Message); }
        }
        return result;
    }
}
