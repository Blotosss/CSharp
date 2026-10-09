using ClinicApp.Models;
using ClinicApp.Enums;
using ClinicApp.Utils;
namespace ClinicApp.Managers;
public class DoctorManager
{
    private const int MaxDoctors = 50;
    private Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count = 0;

    public int Count => _count;

    public Doctor? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count) return null;
            return _doctors[index];
        }
    }

    public void Add(Doctor doctor)
    {
        if (_count >= MaxDoctors)
        {
            Console.WriteLine("Помилка: досягнуто ліміт лікарів!");
            return;
        }

        _doctors[_count] = doctor;
        _count++;
    }

    public Doctor? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                return _doctors[i];
            }
        }
        return null;
    }

    public bool TryFindById(int id, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Doctor? doctor)
    {
        doctor = FindById(id);
        return doctor != null;
    }

    public Doctor[] FindBySpeciality(string query)
    {
        string q = query.ToLower();
        int matchCount = 0;

        for (int i = 0; i < _count; i++)
        {
            string enumStr = _doctors[i].Speciality.ToString().ToLower();
            string formattedStr = ClinicFormatter.FormatSpeciality(_doctors[i].Speciality).ToLower();
            if (enumStr.Contains(q) || formattedStr.Contains(q))
            {
                matchCount++;
            }
        }

        Doctor[] result = new Doctor[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            string enumStr = _doctors[i].Speciality.ToString().ToLower();
            string formattedStr = ClinicFormatter.FormatSpeciality(_doctors[i].Speciality).ToLower();
            if (enumStr.Contains(q) || formattedStr.Contains(q))
            {
                result[index++] = _doctors[i];
            }
        }

        return result;
    }

    public Doctor[] FindBySpeciality(Speciality speciality)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality == speciality)
            {
                matchCount++;
            }
        }

        Doctor[] result = new Doctor[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality == speciality)
            {
                result[index++] = _doctors[i];
            }
        }

        return result;
    }

    public Doctor[] GetAll()
    {
        Doctor[] copy = new Doctor[_count];
        Array.Copy(_doctors, copy, _count);
        return copy;
    }

    public bool Remove(int id)
    {
        int targetIndex = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                targetIndex = i;
                break;
            }
        }

        if (targetIndex == -1) return false;

        for (int i = targetIndex; i < _count - 1; i++)
        {
            _doctors[i] = _doctors[i + 1];
        }

        _doctors[_count - 1] = null!;
        _count--;
        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список лікарів порожній.");
            return;
        }

        Console.WriteLine($"=== Лікарі ({_count} / {MaxDoctors}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_doctors[i]);
        }
        Console.WriteLine("--------------------------------------------------");
    }

    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("Немає даних для статистики.");
            return;
        }

        int availableNow = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].IsAvailableNow)
            {
                availableNow++;
            }
        }

        Console.WriteLine("=== Статистика лікарів ===");
        Console.WriteLine($"Всього:          {_count}");
        Console.WriteLine($"Доступні зараз:  {availableNow}");
        Console.WriteLine("По спеціальностях:");

        for (int i = 0; i < _count; i++)
        {
            bool isFirstOccurrence = true;
            for (int j = 0; j < i; j++)
            {
                if (_doctors[i].Speciality == _doctors[j].Speciality)
                {
                    isFirstOccurrence = false;
                    break;
                }
            }

            if (isFirstOccurrence)
            {
                int specCount = 0;
                for (int k = 0; k < _count; k++)
                {
                    if (_doctors[k].Speciality == _doctors[i].Speciality)
                    {
                        specCount++;
                    }
                }
                Console.WriteLine($"  {_doctors[i].Speciality}: {specCount}");
            }
        }
        Console.WriteLine("--------------------------------------------------");
    }
}