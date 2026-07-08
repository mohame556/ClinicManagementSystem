using System.ComponentModel.DataAnnotations;

namespace Clinc.Models
{
    public class License
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MachineId { get; set; } = string.Empty;

        [Required]
        public string LicenseKey { get; set; } = string.Empty;

        public bool IsActivated { get; set; }

        public DateTime ActivationDate { get; set; }
    }
}