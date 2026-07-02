using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Clinc.ViewModels
{
    public class AppointmentsVM
    {
        public int Id;
        [Display(Name = "المريض")]
        [Required(ErrorMessage = "يجب اختيار المريض")]
        public int PatientId { get; set; }

        [Display(Name = "الطبيب")]
        [Required(ErrorMessage = "يجب اختيار الطبيب")]
        public int DoctorId { get; set; }

        [Display(Name = "موعد الحجز")]
        [Required(ErrorMessage = "يجب إدخال موعد الحجز")]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "حالة الحجز")]
        public string Status { get; set; } = "Pending";

        [Display(Name = "متابعة")]
        public bool IsFollowUp { get; set; }

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }
        [Required(ErrorMessage = "يجب اختيار نوع الكشف ")]
        public string VisitType { get; set; } = "";

        public decimal Fees { get; set; }

        // القوائم المنسدلة
        public List<SelectListItem>? Doctors { get; set; }
        public List<SelectListItem>? Patients { get; set; }
    }
}