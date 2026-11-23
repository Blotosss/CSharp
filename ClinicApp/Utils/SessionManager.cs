using System.Globalization;
using System.Text;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionManager
{
    private readonly string _sessionPath;
    public SessionManager(string sessionPath = "session.dat") { _sessionPath = sessionPath; }
    public bool Exists() => File.Exists(_sessionPath);
    public void Save(Clinic clinic)
    {
        using StreamWriter writer = new StreamWriter(_sessionPath, false, Encoding.UTF8);
        writer.WriteLine("[PATIENTS]");
        Patient[] patients = clinic.Patients.GetAll();
        for (int i = 0; i < patients.Length; i++)
        {
            Patient p = patients[i];
            writer.WriteLine($"{p.FirstName},{p.LastName},{p.DateOfBirth:dd.MM.yyyy},{p.BloodType},{p.Phone}");
        }
        writer.WriteLine("[END]");
    }
    public int Load(Clinic clinic)
    {
        if (!File.Exists(_sessionPath)) return 0;
        string[] lines;
        try { lines = File.ReadAllLines(_sessionPath, Encoding.UTF8); }
        catch { return 0; }
        bool inPatients = false; int added = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.Length == 0) continue;
            if (line.StartsWith("[", StringComparison.Ordinal))
            {
                inPatients = line == "[PATIENTS]";
                if (line == "[END]") break;
                continue;
            }
            if (!inPatients) continue;
            try
            {
                string[] fields = line.Split(',');
                if (fields.Length != 5) continue;
                DateTime dob = DateTime.ParseExact(fields[2].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                var blood = (Enums.BloodType)Enum.Parse(typeof(Enums.BloodType), fields[3].Trim(), false);
                string fn = fields[0].Trim(), ln = fields[1].Trim();
                bool exists = false;
                Patient[] current = clinic.Patients.GetAll();
                for (int j = 0; j < current.Length; j++)
                    if (string.Equals(current[j].FirstName, fn, StringComparison.OrdinalIgnoreCase) && string.Equals(current[j].LastName, ln, StringComparison.OrdinalIgnoreCase) && current[j].DateOfBirth.Date == dob.Date) { exists = true; break; }
                if (exists) continue;
                clinic.Patients.Add(new Patient(fn, ln, dob, blood, fields[4].Trim())); added++;
            }
            catch { }
        }
        return added;
    }
}
