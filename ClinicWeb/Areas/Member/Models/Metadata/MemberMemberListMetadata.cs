using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ClinicWeb.Areas.Member.Models
{
    internal class MemberMemberListMetadata
    {
        [Key]
        [Required]
        [Column("Member_ID")]
       
        public int MemberId { get; set; }

    
        //[Column("Member_Number")]
        [Display(Name = "會員編號")]
        public int MemberNumber { get; set; }

        [Required(ErrorMessage = "請輸入姓名 ")]
        //[StringLength(50)]
        [Display(Name = "姓名")]
        public string? Name { get; set; }



        [Required(ErrorMessage = "請選擇性別 ")]
        [Display(Name = "性別")]
        public bool? Gender { get; set; }
     
        public string? GenderString { get; set; }








        [Required]
        //[Column("Blood_Type")]
        //[StringLength(50)]
        [Display(Name = "血型")]
        public string? BloodType { get; set; }


        //[Column("National_ID")]
        //[StringLength(50)]
        [Required(ErrorMessage = "請輸入身分證字號 ")]
        [Display(Name = "身分證字號")]
        public string? NationalId { get; set; }

        [Required(ErrorMessage = "請輸入戶籍地址 ")]
        //[StringLength(50)]
        [Display(Name = "戶籍地址")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "請輸入聯絡地址 ")]
        //[Column("Contact_Address")]
        //[StringLength(50)]
        [Display(Name = "聯絡地址")]
        public string? ContactAddress { get; set; }

        [Required(ErrorMessage = "請輸入連絡電話 ")]
        //[StringLength(50)]
        [Display(Name = "連絡電話")]
        public string? Phone { get; set; }

        
        //[Column("Birth_Date", TypeName = "datetime")]

        [Display(Name = "生日")]
        [Required(ErrorMessage = "請選擇生日 ")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "請輸入緊急聯絡人姓名 ")]
        //[Column("ICE_Name")]
        //[StringLength(50)]
        [Display(Name = "緊急聯絡人")]
        public string? IceName { get; set; }

        [Required(ErrorMessage = "請輸入緊急聯絡人電話 ")]
        //[Column("ICE_Number")]
        //[StringLength(50)]
        [Display(Name = "緊急聯絡人電話")]
        public string? IceNumber { get; set; }

        [Required(ErrorMessage = "請輸入密碼 ")]
        //[Column("Mem_Password")]
        //[StringLength(50)]
        [Display(Name = "密碼")]
        public string? MemPassword { get; set; }

        [Required(ErrorMessage = "請輸入信箱 ")]
        //[Column("Mem_Email")]
        //[StringLength(50)]
        [Display(Name = "信箱")]
        public string? MemEmail { get; set; }


        [Display(Name = "啟用")]
        public bool? Verification { get; set; }
    
        
    }
}