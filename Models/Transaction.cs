using System.ComponentModel.DataAnnotations;

namespace Clinc.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }
        [Required]
        public string Type { get; set; }      // Income / Expense
        [Required]
        public string Description { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
        public string PaymentMethod { get; set; } = "Cash";
        public bool IsDeleted { get; set; } = false;
        // الدكتور
        public int? DoctorId { get; set; }
        public Doctors? Doctor { get; set; }

        // المريض
        public int? PatientId { get; set; }
        public Patients? Patient { get; set; }

        // الموعد
        public int? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
    }
}