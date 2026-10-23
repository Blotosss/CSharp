using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Doctor : ISchedulable
{
    private static int _nextId = 1;

    private string _firstName = "";
    private string _lastName = "";
    private string _licenseNumber = "";
    private string _phone = "";

    public string FirstName
    {
        get => _firstName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(FirstName));
            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(LastName));
            _lastName = value;
        }
    }

    public string LicenseNumber
    {
        get => _licenseNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Номер ліцензії не може бути порожнім.", nameof(LicenseNumber));
            }
            _licenseNumber = value;
        }
    }

    public string Phone
    {
        get => _phone;
        set
        {
            ClinicValidator.ValidatePhone(value);
            _phone = value;
        }
    }

    public int Id { get; }
    public Speciality Speciality { get; set; }
    public WorkSchedule Schedule { get; set; }
    public string FullName => $"{FirstName} {LastName}";

    public bool IsAvailableNow => Schedule.IsNow;

    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        Schedule = new WorkSchedule(8, 17);
        Id = _nextId++;
    }

    public Doctor(string firstName, string lastName, Speciality speciality)
        : this(firstName, lastName, speciality, "LIC-000", "0000000000")
    {
    }

    public Doctor()
        : this("Невідомий", "Лікар", Speciality.General)
    {
    }

    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }

    public bool CanSchedule(DateTime at)
    {
        return Schedule.Contains(at.Hour);
    }

    public DateTime[] GetAvailableSlots(DateTime date, int slotCount)
    {
        ClinicValidator.ValidatePositive(slotCount, nameof(slotCount));

        int matchingCount = 0;
        for (int h = 0; h < 24; h++)
        {
            if (Schedule.Contains(h))
            {
                matchingCount++;
            }
        }

        int count = slotCount < matchingCount ? slotCount : matchingCount;
        DateTime[] slots = new DateTime[count];

        int index = 0;
        for (int h = 0; h < 24 && index < count; h++)
        {
            if (Schedule.Contains(h))
            {
                slots[index] = date.Date.AddHours(h);
                index++;
            }
        }

        return slots;
    }

    public override string ToString()
    {
        string status = IsAvailableNow ? "доступний" : "не в робочий час";
        return $"[{Id}] {FullName} | {ClinicFormatter.FormatSpeciality(Speciality)} | {LicenseNumber} | Тел: {ClinicFormatter.FormatPhone(Phone)} | {Schedule} | {status}";
    }
}