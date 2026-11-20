using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class FormBuilder
{
    public static T Build<T>() where T : new()
    {
        T obj = new T();
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];
            MethodInfo? setMethod = prop.GetSetMethod();
            if (setMethod == null)
            {
                continue;
            }

            List<string> hints = new List<string>();

            RequiredAttribute? req = prop.GetCustomAttribute<RequiredAttribute>();
            if (req != null)
            {
                hints.Add("обов'язкове");
            }

            MaxLengthAttribute? max = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (max != null)
            {
                hints.Add($"макс. {max.Length}");
            }

            MinValueAttribute? min = prop.GetCustomAttribute<MinValueAttribute>();
            if (min != null)
            {
                hints.Add($"мін. {min.Min}");
            }

            string prompt = prop.Name;
            if (hints.Count > 0)
            {
                prompt += $" ({string.Join(", ", hints)})";
            }
            prompt += ": ";

            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                try
                {
                    Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    object? converted;

                    if (targetType.IsEnum)
                    {
                        converted = Enum.Parse(targetType, input, true);
                    }
                    else
                    {
                        try
                        {
                            converted = Convert.ChangeType(input, targetType, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            converted = Convert.ChangeType(input, targetType);
                        }
                    }

                    prop.SetValue(obj, converted);
                    break;
                }
                catch
                {
                    Console.WriteLine("Некоректне значення, спробуйте ще раз.");
                }
            }
        }

        return obj;
    }
}
