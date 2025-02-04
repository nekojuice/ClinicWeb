﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Member.Models;

[Table("Member_MemberList")]
public partial class MemberMemberList
{
    [Key]
    [Column("Member_ID")]
    public int MemberId { get; set; }

    [Column("Member_Number")]
    public int MemberNumber { get; set; }

    
    [StringLength(50)]
    public string Name { get; set; }

    public bool? Gender { get; set; }

   
    [Column("Blood_Type")]
    [StringLength(50)]
    public string BloodType { get; set; }

   
    [Column("National_ID")]
    [StringLength(50)]
    public string NationalId { get; set; }

    [StringLength(50)]
    public string Address { get; set; }


    [Column("Contact_Address")]
    [StringLength(50)]
    public string ContactAddress { get; set; }

  
    [StringLength(50)]
    public string Phone { get; set; }

    [Column("Birth_Date", TypeName = "datetime")]
    public DateTime? BirthDate { get; set; }

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

    [StringLength(50)]
    public string ActivateToken { get; set; }

    public byte[] MemPhoto { get; set; }

    [StringLength(50)]
    public string GoogleSub { get; set; }

    [InverseProperty("Member")]
    public virtual ICollection<CasesNewBornList> CasesNewBornList { get; set; } = new List<CasesNewBornList>();
}