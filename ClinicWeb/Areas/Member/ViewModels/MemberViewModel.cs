using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Areas.Member.ViewModels
{
    public class MemberViewModel
    {
        [Key]
        [Column("Member_ID")]
        public int MemberId { get; set; }

        [Column("Member_Number")]
        [Display(Name = "姓名")]
        public int MemberNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        private bool _gender;

        public bool Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }




        [Required]
        [Column("Blood_Type")]
        [StringLength(50)]
        public string BloodType { get; set; }

        [Required]
        [Column("National_ID")]
        [StringLength(50)]
        public string NationalId { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        [Column("Contact_Address")]
        [StringLength(50)]
        public string ContactAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }

        [Column("Birth_Date", TypeName = "datetime")]
        public DateTime BirthDate { get; set; }

        [Column("ICE_Name")]
        [StringLength(50)]
        public string IceName { get; set; }

        [Column("ICE_Number")]
        [StringLength(50)]
        public string IceNumber { get; set; }

        [Column("Mem_Password")]
        [StringLength(50)]
        public string MemPassword { get; set; }

        [Column("Mem_Email")]
        [StringLength(50)]
        public string MemEmail { get; set; }

        public bool? Verification { get; set; }
    }
}