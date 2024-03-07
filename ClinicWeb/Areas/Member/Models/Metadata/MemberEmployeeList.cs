using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Areas.Member.Models
{
    [ModelMetadataType(typeof(MemberEmployeeListMetaData))]
    public partial class MemberEmployeeList
    {
    }

    internal class MemberEmployeeListMetaData
    {
        [Display(Name = "員工編號")]
        public int StaffNumber { get; set; }

        [Required(ErrorMessage = "請輸入姓名 ")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "請選擇性別 ")]
        [Display(Name = "性別")]
        public bool? Gender { get; set; }

        [Required]
        [Display(Name = "血型")]
        public string? BloodType { get; set; }

        [Required(ErrorMessage = "請輸入身分證字號 ")]
        [Display(Name = "身分證字號")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "請輸入戶籍地址 ")]
        [Display(Name = "戶籍地址")]
        public string Address { get; set; }

        [Required(ErrorMessage = "請輸入聯絡地址 ")]
        [Display(Name = "聯絡地址")]
        public string ContactAddress { get; set; }

        [Required(ErrorMessage = "請輸入連絡電話 ")]
        [Display(Name = "連絡電話")]
        public string Phone { get; set; }

        [Display(Name = "生日")]
        [Required(ErrorMessage = "請選擇生日 ")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "員工類別")]
        [Required(ErrorMessage = "請選擇員工類別 ")]
        public string EmpType { get; set; }

        [Display(Name = "部門")]
        [Required(ErrorMessage = "請選擇部門")]
        public string Department { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "請選擇密碼")]
        public string EmpPassword { get; set; }

        [Display(Name = "照片")]
        public byte[] EmpPhoto { get; set; }
        
        [Display(Name = "在職")]
        public bool? Quit { get; set; }

    }
}
