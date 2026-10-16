using ClinicApp.Models;

namespace ClinicApp.Managers;

public class MedicalRecordManager
{
    private const int MaxRecords = 1000;
    private readonly MedicalRecord[] _records = new MedicalRecord[MaxRecords];
    private int _count;

    public int Count => _count;

    public MedicalRecord? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count) return null;
            return _records[index];
        }
    }

    public void Add(MedicalRecord record)
    {
        if (record == null) return;

        if (_count >= MaxRecords)
        {
            Console.WriteLine("Досягнуто ліміт медичних записів (1000).");
            return;
        }

        _records[_count] = record;
        _count++;
        Console.WriteLine($"Запис [{record.Id}] {record.GetRecordType()} додано.");
    }

    public MedicalRecord? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].Id == id)
            {
                return _records[i];
            }
        }
        return null;
    }

    public MedicalRecord[] GetByPatient(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                matchCount++;
            }
        }

        MedicalRecord[] result = new MedicalRecord[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                result[index] = _records[i];
                index++;
            }
        }

        return result;
    }

    public MedicalRecord[] GetByDoctor(int doctorId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
            {
                matchCount++;
            }
        }

        MedicalRecord[] result = new MedicalRecord[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
            {
                result[index] = _records[i];
                index++;
            }
        }

        return result;
    }

    public Diagnosis[] GetDiagnoses(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis)
            {
                matchCount++;
            }
        }

        Diagnosis[] result = new Diagnosis[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis d)
            {
                result[index] = d;
                index++;
            }
        }

        return result;
    }

    public LabResult[] GetLabResults(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult)
            {
                matchCount++;
            }
        }

        LabResult[] result = new LabResult[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult lr)
            {
                result[index] = lr;
                index++;
            }
        }

        return result;
    }

    public Prescription[] GetPrescriptions(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription)
            {
                matchCount++;
            }
        }

        Prescription[] result = new Prescription[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription p)
            {
                result[index] = p;
                index++;
            }
        }

        return result;
    }

    public Diagnosis[] GetChronicDiagnoses(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis d && d.IsChronic)
            {
                matchCount++;
            }
        }

        Diagnosis[] result = new Diagnosis[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis d && d.IsChronic)
            {
                result[index] = d;
                index++;
            }
        }

        return result;
    }

    public Prescription[] GetActivePrescriptions(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription p && p.IsActive())
            {
                matchCount++;
            }
        }

        Prescription[] result = new Prescription[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription p && p.IsActive())
            {
                result[index] = p;
                index++;
            }
        }

        return result;
    }

    public void DisplayPatientSummary(int patientId)
    {
        int totalPatientRecords = 0;
        int diagnosisCount = 0;
        int labResultCount = 0;
        int prescriptionCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                totalPatientRecords++;
                if (_records[i] is Diagnosis) diagnosisCount++;
                else if (_records[i] is LabResult) labResultCount++;
                else if (_records[i] is Prescription) prescriptionCount++;
            }
        }

        if (totalPatientRecords == 0)
        {
            Console.WriteLine($"У пацієнта #{patientId} немає медичних записів.");
            return;
        }

        Console.WriteLine($"=== Медична картка пацієнта #{patientId} ===");
        Console.WriteLine($"Всього записів: {totalPatientRecords} (Діагнози: {diagnosisCount}, Аналізи: {labResultCount}, Рецепти: {prescriptionCount})");

        Diagnosis[] chronicDiagnoses = GetChronicDiagnoses(patientId);
        if (chronicDiagnoses.Length > 0)
        {
            Console.WriteLine("\n-- Хронічні діагнози --");
            for (int i = 0; i < chronicDiagnoses.Length; i++)
            {
                Console.WriteLine($"  * {chronicDiagnoses[i].GetSummary()}");
            }
        }

        Prescription[] activePrescriptions = GetActivePrescriptions(patientId);
        if (activePrescriptions.Length > 0)
        {
            Console.WriteLine("\n-- Активні рецепти --");
            for (int i = 0; i < activePrescriptions.Length; i++)
            {
                Console.WriteLine($"  * {activePrescriptions[i].GetSummary()} (діє до {activePrescriptions[i].ExpiresAt:dd.MM.yyyy})");
            }
        }
    }

    public void DisplayAll()
    {
        DisplayList(_records);
    }

    public void DisplayList(MedicalRecord[] list)
    {
        if (list == null || list.Length == 0)
        {
            Console.WriteLine("Записи відсутні.");
            return;
        }

        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] != null)
            {
                Console.WriteLine(list[i].ToString());
            }
        }
    }
}