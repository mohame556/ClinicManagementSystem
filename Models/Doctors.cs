using System.ComponentModel.DataAnnotations;

namespace Clinc.Models
{
    public class Doctors
    {
        [Key]
        public int Id { get; set; }

        public string Doctor_Name { get; set; }

        
        public string Specialty { get; set; }

        
        public string Phone { get; set; }

        public decimal ExaminationFee { get; set; }

        public decimal ReExaminationFee { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }

        // One To Many
        public ICollection<Appointment> Appointments { get; set; }
    }
}