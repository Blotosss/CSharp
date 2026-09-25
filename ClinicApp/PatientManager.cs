namespace ClinicApp;

public class PatientManager
{
    private const int MaxPatients = 100;
    private Patient[] _patients = new Patient[MaxPatients];
    private int _count = 0;

    public int Count => _count;

    public void Add(Patient patient)
    {
        if (_count >= MaxPatients)
        {
            Console.WriteLine("Помилка: досягнуто ліміт пацієнтів!");
            return;
        }

        _patients[_count] = patient;
        _count++;
        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null;
    }

    public Patient[] FindByName(string query)
    {
        string q = query.ToLower();
        int matchCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(q) || 
                _patients[i].LastName.ToLower().Contains(q))
            {
                matchCount++;
            }
        }
        
        Patient[] result = new Patient[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(q) || 
                _patients[i].LastName.ToLower().Contains(q))
            {
                result[index++] = _patients[i];
            }
        }

        return result;
    }

    public bool Remove(int id)
    {
        int targetIndex = -1;
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                targetIndex = i;
                break;
            }
        }

        if (targetIndex == -1) return false;
        
        for (int i = targetIndex; i < _count - 1; i++)
        {
            _patients[i] = _patients[i + 1];
        }

        _patients[_count - 1] = null!;
        _count--;
        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список пацієнтів порожній.");
            return;
        }

        Console.WriteLine($"=== Пацієнти ({_count} / {MaxPatients}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
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

        int totalAge = 0;
        int adultCount = 0;
        int youngestIdx = 0;
        int oldestIdx = 0;

        for (int i = 0; i < _count; i++)
        {
            int age = _patients[i].Age;
            totalAge += age;

            if (_patients[i].IsAdult) adultCount++;

            if (age < _patients[youngestIdx].Age) youngestIdx = i;
            if (age > _patients[oldestIdx].Age) oldestIdx = i;
        }

        double avgAge = (double)totalAge / _count;

        Console.WriteLine("=== Статистика пацієнтів ===");
        Console.WriteLine($"Всього:       {_count}");
        Console.WriteLine($"Середній вік: {avgAge:F1} р.");
        Console.WriteLine($"Наймолодший:  {_patients[youngestIdx].FullName} ({_patients[youngestIdx].Age} р.)");
        Console.WriteLine($"Найстарший:   {_patients[oldestIdx].FullName} ({_patients[oldestIdx].Age} р.)");
        Console.WriteLine($"Дорослих:     {adultCount} з {_count}");
        Console.WriteLine("--------------------------------------------------");
    }
}