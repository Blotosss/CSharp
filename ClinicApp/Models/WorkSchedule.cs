namespace ClinicApp.Models;

public struct WorkSchedule
{
    public int StartHour { get; }
    public int EndHour { get; }

    public WorkSchedule(int startHour, int endHour)
    {
        if (startHour < 0 || startHour > 23)
        {
            throw new ArgumentOutOfRangeException(nameof(startHour), "Час початку має бути від 0 до 23.");
        }
        if (endHour < 1 || endHour > 24)
        {
            throw new ArgumentOutOfRangeException(nameof(endHour), "Час кінця має бути від 1 до 24.");
        }
        if (startHour >= endHour)
        {
            throw new ArgumentException("Час початку має бути меншим за час кінця.", nameof(startHour));
        }

        StartHour = startHour;
        EndHour = endHour;
    }

    public bool Contains(int hour)
    {
        return hour >= StartHour && hour < EndHour;
    }

    public bool IsNow => Contains(DateTime.Now.Hour);

    public override string ToString()
    {
        return $"{StartHour:D2}:00–{EndHour:D2}:00";
    }
}