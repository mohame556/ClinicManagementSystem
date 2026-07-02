using System.ComponentModel.DataAnnotations;

namespace Clinc.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "يجب إدخال اسم المستخدم كامل")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "يجب ادخال العنوان")]
        public string Address { get; set; }

        [Required(ErrorMessage = "يجب ادخال البريد الالكترونى")]
        [EmailAddress(ErrorMessage = "البريد الالكترونى غير صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "يجب ادخال كلمة المرور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "يجب ادخال كلمة المرور مرة ثانية للتأكيد")]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}