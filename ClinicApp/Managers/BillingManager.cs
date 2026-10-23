using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class BillingManager
{
    private readonly AppointmentManager _appointments;

    public BillingManager(AppointmentManager appointments)
    {
        _appointments = appointments;
    }

    private IPayable[] FilterUnpaid(Appointment[] sourceArray)
    {
        int matchCount = 0;
        foreach (var app in sourceArray)
        {
            if (!app.IsPaid && !app.IsCancelled)
            {
                matchCount++;
            }
        }

        IPayable[] result = new IPayable[matchCount];
        int index = 0;
        foreach (var app in sourceArray)
        {
            if (!app.IsPaid && !app.IsCancelled)
            {
                result[index++] = app;
            }
        }
        return result;
    }

    public IPayable[] GetAllUnpaid()
    {
        return FilterUnpaid(_appointments.GetAll());
    }

    public IPayable[] GetUnpaidByPatient(int patientId)
    {
        return FilterUnpaid(_appointments.GetByPatient(patientId));
    }

    public decimal GetTotalDebt()
    {
        decimal total = 0m;
        foreach (var item in GetAllUnpaid())
        {
            total += item.GetCost();
        }
        return total;
    }

    public decimal GetPatientDebt(int patientId)
    {
        decimal total = 0m;
        foreach (var item in GetUnpaidByPatient(patientId))
        {
            total += item.GetCost();
        }
        return total;
    }

    public bool PayAppointment(int appointmentId)
    {
        foreach (var app in _appointments.GetAll())
        {
            if (app.Id == appointmentId)
            {
                if (!app.IsPaid && !app.IsCancelled)
                {
                    app.MarkPaid();
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    public void DisplayUnpaid(IPayable[] items)
    {
        if (items.Length == 0)
        {
            Console.WriteLine("Немає неоплачених записів.");
            return;
        }

        foreach (var item in items)
        {
            if (item is Appointment app)
            {
                Console.WriteLine($"Запис [{app.Id}] Пацієнт #{app.PatientId} -> Лікар #{app.DoctorId} | Сума: {app.GetCost().ToString("F2")} грн");
            }
            else
            {
                Console.WriteLine($"Об'єкт | Сума: {item.GetCost().ToString("F2")} грн");
            }
        }
    }
}