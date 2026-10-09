using ClinicApp.Enums;
namespace ClinicApp.Utils;
public static class ClinicFormatter
{
    public static string FormatBloodType(BloodType bt) => bt switch
    {
        BloodType.APositive => "A+",
        BloodType.ANegative => "A-",
        BloodType.BPositive => "B+",
        BloodType.BNegative => "B-",
        BloodType.ABPositive => "AB+",
        BloodType.ABNegative => "AB-",
        BloodType.OPositive => "O+",
        BloodType.ONegative => "O-",
        BloodType.Unknown => "Невідомо",
        _ => "Невідомо"
    };

    public static string FormatSpeciality(Speciality s) => s switch
    {
        Speciality.General => "Загальна практика",
        Speciality.Cardiology => "Кардіологія",
        Speciality.Neurology => "Неврологія",
        Speciality.Pediatrics => "Педіатрія",
        Speciality.Surgery => "Хірургія",
        Speciality.Orthopedics => "Ортопедія",
        Speciality.Dermatology => "Дерматологія",
        Speciality.Emergency => "Невідкладна допомога",
        _ => s.ToString()
    };

    public static string FormatAge(int age)
    {
        int mod100 = age % 100;
        if (mod100 >= 11 && mod100 <= 19)
        {
            return $"{age} років";
        }

        int mod10 = age % 10;
        if (mod10 == 1)
        {
            return $"{age} рік";
        }
        if (mod10 >= 2 && mod10 <= 4)
        {
            return $"{age} роки";
        }

        return $"{age} років";
    }

    public static string FormatPhone(string phone)
    {
        if (string.IsNullOrEmpty(phone) || phone.Length != 10)
        {
            return phone;
        }

        for (int i = 0; i < phone.Length; i++)
        {
            if (!char.IsDigit(phone[i]))
            {
                return phone;
            }
        }

        return $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6, 4)}";
    }
}
