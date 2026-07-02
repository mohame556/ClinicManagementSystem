using System.ComponentModel.DataAnnotations;

namespace Clinc.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        
        public string Status { get; set; } = "Pending";
        public bool IsFollowUp { get; set; }

        public string VisitType { get; set; } = "";

        public decimal Fees { get; set; }
        public DateTime? CompletedAt { get; set; }

        public Transaction? Transaction { get; set; }
        public string? Notes { get; set; }

        // Navigation Properties
        public Patients? Patient { get; set; }

        public Doctors? Doctor { get; set; }
    }
}