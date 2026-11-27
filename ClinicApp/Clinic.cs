using ClinicApp.Managers;
using ClinicApp.Models;

namespace ClinicApp;

public class Clinic
{
    public string Name { get; set; }
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }
    public MedicalRecordManager MedicalRecords { get; }
    public BillingManager Billing { get; }
    public WaitingQueue<Patient> WaitingRoom { get; }
    public AnalyticsManager Analytics { get; }
    public TreatmentPlanManager TreatmentPlans { get; }
    public Utils.ClinicLogger Logger { get; }
    public Utils.ClinicExporter Exporter { get; }
    public Utils.SessionManager Session { get; }
    public Utils.PatientPassportWriter Passport { get; }
    public Utils.SessionEventTracker Tracker { get; }

    public Clinic(string name)
    {
        Name = name;
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
        MedicalRecords = new MedicalRecordManager();
        Billing = new BillingManager(Appointments);
        WaitingRoom = new WaitingQueue<Patient>();
        Analytics = new AnalyticsManager(Appointments, Doctors, Patients);
        TreatmentPlans = new TreatmentPlanManager();
        Logger = new Utils.ClinicLogger();
        Exporter = new Utils.ClinicExporter(this);
        Session = new Utils.SessionManager();
        Passport = new Utils.PatientPassportWriter(this);
        Tracker = new Utils.SessionEventTracker(this);
        SubscribeEvents();
        SubscribeTrackerEvents();

    }

    private void SubscribeEvents()
    {
        Patients.PatientAdded += Logger.OnPatientAdded;
        Appointments.AppointmentBooked += Logger.OnAppointmentBooked;
        Appointments.UrgentAppointmentBooked += Logger.OnUrgentAppointmentBooked;
        Appointments.AppointmentCancelled += Logger.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Logger.OnAppointmentCompleted;
        Billing.PaymentReceived += Logger.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Logger.OnPlanCompleted;

        Patients.PatientAdded += Passport.OnPatientAdded;
        Appointments.AppointmentCompleted += Passport.OnAppointmentCompleted;
        TreatmentPlans.PlanCompleted += Passport.OnPlanCompleted;
    }


    private void SubscribeTrackerEvents()
    {
        Patients.PatientAdded += Tracker.OnPatientAdded;
        Appointments.AppointmentBooked += Tracker.OnAppointmentBooked;
        Appointments.UrgentAppointmentBooked += Tracker.OnUrgentAppointmentBooked;
        Appointments.AppointmentCancelled += Tracker.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Tracker.OnAppointmentCompleted;
        Billing.PaymentReceived += Tracker.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Tracker.OnPlanCompleted;
    }

}
