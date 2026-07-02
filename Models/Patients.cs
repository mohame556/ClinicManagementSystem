using System.ComponentModel.DataAnnotations;

namespace Clinc.Models
{
    public class Patients
    {
        [Key]
        public int Id { get; set; }

       
        public string Patient_Name { get; set; }

        
        public string Address { get; set; }

        
        public int Age { get; set; }

       
        public string Phone { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }

        // One To One
        public MedicalHistory MedicalHistory { get; set; }

        // One To Many
        public ICollection<Appointment> Appointments { get; set; }
    }
}