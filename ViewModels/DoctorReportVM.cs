using Clinc.Models;

namespace Clinc.ViewModels
{
    public class DoctorReportVM
    {
        public int DoctorId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public List<Doctors>? Doctors { get; set; }

        public List<Appointment>? Appointments { get; set; }

        public int TotalAppointments { get; set; }

        public int TotalPatients { get; set; }
        public decimal TotalAmount { get; set; }
    }
}