using Clinc.Models;

namespace Clinc.ViewModels
{
    public class PatientReportVM
    {
        public int PatientId { get; set; }

        public List<Patients>? Patients { get; set; }

        public Patients? Patient { get; set; }

        public List<Appointment>? Appointments { get; set; }
    }
}