namespace Clinc.ViewModels
{
    public class StatisticsVM
    {
        public int DoctorsCount { get; set; }

        public int PatientsCount { get; set; }

        public int AppointmentsCount { get; set; }

        public int TodayAppointments { get; set; }

        public int CompletedAppointments { get; set; }

        public int CancelledAppointments { get; set; }
    }
}