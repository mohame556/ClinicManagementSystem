using Clinc.Models;

namespace Clinc.ViewModels
{
    public class DashboardVM
    {
        public int DoctorsCount { get; set; }
        public int PatientsCount { get; set; }
        public int AppointmentsCount { get; set; }

        public int TodayAppointmentsCount { get; set; }
        public int ConfirmedAppointmentsCount { get; set; }
        public int CancelledAppointmentsCount { get; set; }

        public List<Appointment> TodayAppointments { get; set; } = new();
        public List<Appointment> UpcomingAppointments { get; set; } = new();
        public Dictionary<string, List<Appointment>> DoctorAppointments { get; set; } = new();
    }
}