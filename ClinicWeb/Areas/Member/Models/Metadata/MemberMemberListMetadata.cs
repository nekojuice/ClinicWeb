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

        [Required]
        //[Column("Member_Number")]
        [Display(Name = "會員編號")]
        public int MemberNumber { get; set; }

        [Required(ErrorMessage = "請輸入姓名 ")]
        //[StringLength(50)]
        [Display(Name = "姓名")]
        public string? Name { get; set; }



    
        [Display(Name = "性別")]
        public bool Gender { get; set; }
        [Required]
        public string? GenderString { get; set; }








        [Required]
        //[Column("Blood_Type")]
        //[StringLength(50)]
        [Display(Name = "血型")]
        public string? BloodType { get; set; }

        [Required]
        //[Column("National_ID")]
        //[StringLength(50)]
        [Display(Name = "身分證字號")]
        public string? NationalId { get; set; }

        [Required]
        //[StringLength(50)]
        [Display(Name = "戶籍地址")]
        public string? Address { get; set; }

        //[Required]
        //[Column("Contact_Address")]
        //[StringLength(50)]
        [Display(Name = "聯絡地址")]
        public string? ContactAddress { get; set; }

        [Required]
        //[StringLength(50)]
        [Display(Name = "連絡電話")]
        public string? Phone { get; set; }

        [Required]
        //[Column("Birth_Date", TypeName = "datetime")]
        [Display(Name = "生日")]
       
        public DateTime BirthDate { get; set; }

        //[Column("ICE_Name")]
        //[StringLength(50)]
        [Display(Name = "緊急聯絡人")]
        public string? IceName { get; set; }

        [Required]
        //[Column("ICE_Number")]
        //[StringLength(50)]
        [Display(Name = "緊急聯絡人電話")]
        public string? IceNumber { get; set; }

        [Required]
        //[Column("Mem_Password")]
        //[StringLength(50)]
        [Display(Name = "密碼")]
        public string? MemPassword { get; set; }

        [Required]
        //[Column("Mem_Email")]
        //[StringLength(50)]
        [Display(Name = "信箱")]
        public string? MemEmail { get; set; }


        [Display(Name = "啟用")]
        public bool? Verification { get; set; }
    
        public string? VerificationString { get; set; }
    }
}