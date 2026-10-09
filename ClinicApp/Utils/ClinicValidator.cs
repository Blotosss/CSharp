namespace ClinicApp.Utils;

public static class ClinicValidator
{
    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
        {
            throw new ArgumentException("Значення не може бути порожнім або довшим за 50 символів.", fieldName);
        }
    }

    public static void ValidatePhone(string phone)
    {
        if (phone == null || phone.Length != 10 || !IsDigitsOnly(phone))
        {
            throw new ArgumentException("Номер телефону має містити рівно 10 цифр.", nameof(phone));
        }
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value > DateTime.Today || value.Year < 1900)
        {
            throw new ArgumentOutOfRangeException(fieldName, "Дата не може бути в майбутньому або раніше 1900 року.");
        }
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(fieldName, "Значення має бути більшим за 0.");
        }
    }

    private static bool IsDigitsOnly(string str)
    {
        foreach (char c in str)
        {
            if (c < '0' || c > '9') return false;
        }
        return true;
    }
}