﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Models;

[Table("Cases_Prescription")]
public partial class CasesPrescription
{
    [Column("Case_ID")]
    public int CaseId { get; set; }

    [Key]
    [Column("Prescription_ID")]
    public int PrescriptionId { get; set; }

    [Column("Prescription_Date", TypeName = "datetime")]
    public DateTime PrescriptionDate { get; set; }

    [StringLength(50)]
    public string Dispensing { get; set; }

    [ForeignKey("CaseId")]
    [InverseProperty("CasesPrescription")]
    public virtual CasesMainCase Case { get; set; }

    [InverseProperty("Prescription")]
    public virtual ICollection<CasesPrescriptionlist> CasesPrescriptionlist { get; set; } = new List<CasesPrescriptionlist>();
}