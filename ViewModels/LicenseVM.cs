using System.ComponentModel.DataAnnotations;

namespace Clinc.ViewModels
{
    public class LicenseVM
    {
        public string MachineId { get; set; } = "";

        [Required(ErrorMessage = "أدخل كود التفعيل")]
        public string LicenseKey { get; set; } = "";
    }
}