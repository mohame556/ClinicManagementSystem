using System.ComponentModel.DataAnnotations;

namespace Clinc.ViewModels
{
    public class PatientsVM
    {
        public int Id { get; set; }
        [Display(Name = "اسم المريض")]
        [Required(ErrorMessage = "يجب إدخال اسم المريض")]
        public string Patient_Name { get; set; }

        [Display(Name = "العنوان")]
        [Required(ErrorMessage = "يجب إدخال العنوان")]
        public string Address { get; set; }

        [Display(Name = "العمر")]
        [Required(ErrorMessage = "يجب إدخال العمر")]
        [Range(1, 120, ErrorMessage = "العمر يجب أن يكون بين 1 و 120")]
        public int Age { get; set; }

        [Display(Name = "رقم الهاتف")]
        [Required(ErrorMessage = "يجب إدخال رقم الهاتف")]
        [RegularExpression(@"^01[0-2,5][0-9]{8}$",
            ErrorMessage = "رقم الهاتف المصري غير صحيح")]
        public string Phone { get; set; }
    }
}
