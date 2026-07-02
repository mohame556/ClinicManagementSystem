using System.ComponentModel.DataAnnotations;

namespace Clinc.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "يلزم ادخال البريد الالكترونى")]
        [EmailAddress(ErrorMessage = "تنسيق البريد الالكترونى خطأ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}