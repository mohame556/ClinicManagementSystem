using System.ComponentModel.DataAnnotations;

namespace Clinc.ViewModels
{
    public class UserVM
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "الاسم مطلوب")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "الإيميل مطلوب")]
        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }

        public List<string> RolesList { get; set; } = new();
    }
}