using ClinicApp.Models;
using ClinicApp.Enums;

namespace ClinicApp.Managers;

public class PatientManager
{
    private List<Patient> _patients = new List<Patient>();

    public int Count => _patients.Count;

    public Patient? this[int index]
    {
        get
        {
            if (index < 0 || index >= _patients.Count) return null;
            return _patients[index];
        }
    }

    public void Add(Patient patient)
    {
        _patients.Add(patient);
        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null;
    }

    public bool TryFindById(int id, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Patient? patient)
    {
        patient = FindById(id);
        return patient != null;
    }

    public Patient[] FindByBloodType(BloodType bloodType)
    {
        List<Patient> result = new List<Patient>();
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].BloodType == bloodType)
            {
                result.Add(_patients[i]);
            }
        }
        return result.ToArray();
    }

    public Patient[] FindByName(string query)
    {
        string q = query.ToLower();
        List<Patient> result = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(q) || 
                _patients[i].LastName.ToLower().Contains(q))
            {
                result.Add(_patients[i]);
            }
        }

        return result.ToArray();
    }

    public Patient[] GetAll()
    {
        return _patients.ToArray();
    }

    public bool Remove(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                _patients.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public void DisplayAll()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"=== Пацієнти ({_patients.Count}) ===");
        for (int i = 0; i < _patients.Count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
        Console.WriteLine("--------------------------------------------------");
    }

    public void DisplayStats()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Немає даних для статистики.");
            return;
        }

        int totalAge = 0;
        int adultCount = 0;
        int youngestIdx = 0;
        int oldestIdx = 0;

        for (int i = 0; i < _patients.Count; i++)
        {
            int age = _patients[i].Age;
            totalAge += age;

            if (_patients[i].IsAdult) adultCount++;

            if (age < _patients[youngestIdx].Age) youngestIdx = i;
            if (age > _patients[oldestIdx].Age) oldestIdx = i;
        }

        double avgAge = (double)totalAge / _patients.Count;

        Console.WriteLine("=== Статистика пацієнтів ===");
        Console.WriteLine($"Всього:       {_patients.Count}");
        Console.WriteLine($"Середній вік: {avgAge:F1} р.");
        Console.WriteLine($"Наймолодший:  {_patients[youngestIdx].FullName} ({_patients[youngestIdx].Age} р.)");
        Console.WriteLine($"Найстарший:   {_patients[oldestIdx].FullName} ({_patients[oldestIdx].Age} р.)");
        Console.WriteLine($"Дорослих:     {adultCount} з {_patients.Count}");
        Console.WriteLine("--------------------------------------------------");
    }
}
