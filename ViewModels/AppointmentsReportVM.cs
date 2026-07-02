using Clinc.Models;

namespace Clinc.ViewModels
{
    public class AppointmentsReportVM
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public List<Appointment>? Appointments { get; set; }
    }
}