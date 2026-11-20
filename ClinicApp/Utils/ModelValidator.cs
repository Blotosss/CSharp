using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class ModelValidator
{
    public static ValidationResult Validate(object obj)
    {
        ValidationResult result = new ValidationResult();

        if (obj == null)
        {
            result.AddError("Object cannot be null.");
            return result;
        }

        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];
            object? value = prop.GetValue(obj);

            RequiredAttribute? requiredAttr = prop.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttr != null)
            {
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    result.AddError($"{prop.Name}: {requiredAttr.ErrorMessage}");
                }
            }

            MaxLengthAttribute? maxLengthAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttr != null)
            {
                if (value is string str && str.Length > maxLengthAttr.Length)
                {
                    result.AddError($"{prop.Name}: {maxLengthAttr.ErrorMessage}");
                }
            }

            MinValueAttribute? minValueAttr = prop.GetCustomAttribute<MinValueAttribute>();
            if (minValueAttr != null)
            {
                if (value == null)
                {
                    result.AddError($"{prop.Name}: {minValueAttr.ErrorMessage}");
                }
                else
                {
                    try
                    {
                        double num = Convert.ToDouble(value);
                        if (num < minValueAttr.Min)
                        {
                            result.AddError($"{prop.Name}: {minValueAttr.ErrorMessage}");
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        return result;
    }

    public static void PrintInfo(Type type)
    {
        Console.WriteLine(type.Name);
        PropertyInfo[] properties = type.GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];
            string line = "  " + prop.Name;

            object[] attributes = prop.GetCustomAttributes(false);
            for (int j = 0; j < attributes.Length; j++)
            {
                Type attrType = attributes[j].GetType();
                if (attrType.Namespace != null && attrType.Namespace.StartsWith("System.Runtime.CompilerServices"))
                {
                    continue;
                }

                string attrName = attrType.Name;
                if (attrName.EndsWith("Attribute"))
                {
                    attrName = attrName.Substring(0, attrName.Length - "Attribute".Length);
                }

                line += " [" + attrName + "]";
            }

            Console.WriteLine(line);
        }
    }
}
