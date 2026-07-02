using System.ComponentModel.DataAnnotations;

namespace Clinc.Models
{
    public class MedicalHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

       
        public string? ChronicDiseases { get; set; }

       
        public string? Allergies { get; set; }

        
        public string? Surgeries { get; set; }

      
        public string? CurrentMedications { get; set; }

       
        public string? BloodType { get; set; }

       
        public string? Notes { get; set; }

        // Navigation Property
        public Patients Patient { get; set; }
    }
}