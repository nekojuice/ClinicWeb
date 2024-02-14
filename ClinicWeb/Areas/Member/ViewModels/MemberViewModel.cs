using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Areas.Member.ViewModels
{
	public class MemberViewModel
	{
		[Key]
		[Column("Member_ID")]
		public int MemberId { get; set; }

		//[Column("Member_Number")]
		[Display(Name = "會員編號")]
		public int MemberNumber { get; set; }

		//[Required]
		//[StringLength(50)]
		[Display(Name = "姓名")]
		[Required(ErrorMessage = "請輸入姓名")]
		public string? Name { get; set; }



		//private bool _gender;
		[Display(Name = "性別")]
		public bool Gender { get; set; }

		[MinLength(4, ErrorMessage = "性別為必填")]
		[Required(ErrorMessage = "性別為必填")]
		public string? GenderString //{ get; set; }
		{
			get { return _gender; }
			set
			{
				_gender = value;
				try
				{
					Gender = Convert.ToBoolean(value);
				}
				catch (Exception)
				{
				};
			}
		}
		public string? _gender { get; set; }


		//[Required]
		//[Column("Blood_Type")]
		//[StringLength(50)]
		[Display(Name = "血型")]
		public string? BloodType { get; set; }

		//[Required]
		//[Column("National_ID")]
		//[StringLength(50)]
		[Display(Name = "身分證字號")]
		public string? NationalId { get; set; }

		//[Required]
		//[StringLength(50)]
		[Display(Name = "戶籍地址")]
		public string? Address { get; set; }

		//[Required]
		//[Column("Contact_Address")]
		//[StringLength(50)]
		[Display(Name = "聯絡地址")]
		public string? ContactAddress { get; set; }

		//[Required]
		//[StringLength(50)]
		[Display(Name = "連絡電話")]
		public string? Phone { get; set; }

		//[Column("Birth_Date", TypeName = "datetime")]
		//[Required(ErrorMessage = "請選擇生日 ")]
		[Display(Name = "生日")]
		public DateTime? BirthDate { get; set; }

		//[Column("ICE_Name")]
		//[StringLength(50)]
		[Display(Name = "緊急聯絡人")]
		public string? IceName { get; set; }

		//[Column("ICE_Number")]
		//[StringLength(50)]
		[Display(Name = "緊急聯絡人電話")]
		public string? IceNumber { get; set; }

		//[Column("Mem_Password")]
		//[StringLength(50)]
		[Required(ErrorMessage = "必須至少為8碼，包含大小寫英文字、數字、特殊符號")]
		[Display(Name = "密碼")]
		public string? MemPassword { get; set; }

		//[Column("Mem_Email")]
		//[StringLength(50)]
		[Required(ErrorMessage = "請輸入信箱")]
		[Display(Name = "信箱")]
		public string? MemEmail { get; set; }


		[Display(Name = "啟用")]
		public bool? Verification { get; set; }
		public string? VerificationString
		{
			get
			{
				return _verificationString;
			}
			set
			{
				_verificationString = value;
				Verification = value.Equals("on") ? true : false;
			}
		}
		public string? _verificationString { get; set; }

	}
}