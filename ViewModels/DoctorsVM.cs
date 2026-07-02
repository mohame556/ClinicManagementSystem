using System.ComponentModel.DataAnnotations;

namespace Clinc.ViewModels
{
    public class DoctorVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الطبيب مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم طويل جدًا")]
        public string Doctor_Name { get; set; }

        [Required(ErrorMessage = "التخصص مطلوب")]
        [StringLength(100)]
        public string Specialty { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "رقم الهاتف غير صحيح")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "رسوم الكشف مطلوبة")]
        [Range(1, 100000, ErrorMessage = "رسوم الكشف يجب أن تكون أكبر من 0")]
        public decimal ExaminationFee { get; set; }

        [Required(ErrorMessage = "رسوم إعادة الكشف مطلوبة")]
        [Range(1, 100000, ErrorMessage = "رسوم إعادة الكشف يجب أن تكون أكبر من 0")]
        public decimal ReExaminationFee { get; set; }
    }
}