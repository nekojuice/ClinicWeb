﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Models;

[Table("Pharmacy_tMedicinesList")]
public partial class PharmacyTMedicinesList
{
    [Key]
    [Column("fId_Drug")]
    public int FIdDrug { get; set; }

    [Required]
    [Column("fDrugCode")]
    [StringLength(10)]
    public string FDrugCode { get; set; }

    [Required]
    [Column("fGenericName")]
    [StringLength(30)]
    public string FGenericName { get; set; }

    [Required]
    [Column("fTradeName")]
    [StringLength(30)]
    public string FTradeName { get; set; }

    [Required]
    [Column("fDrugName")]
    [StringLength(20)]
    public string FDrugName { get; set; }

    [Column("fDrugDose")]
    [StringLength(500)]
    public string FDrugDose { get; set; }

    [Column("fMaxDose")]
    [StringLength(100)]
    public string FMaxDose { get; set; }

    [Column("fPrecautions")]
    [StringLength(500)]
    public string FPrecautions { get; set; }

    [Column("fWarnings")]
    [StringLength(200)]
    public string FWarnings { get; set; }

    [Column("fPregnancyCategory")]
    [StringLength(5)]
    [Unicode(false)]
    public string FPregnancyCategory { get; set; }

    [Required]
    [Column("fApperance")]
    [StringLength(50)]
    public string FApperance { get; set; }

    [Column("fImages")]
    [StringLength(50)]
    public string FImages { get; set; }

    [Required]
    [Column("fStorage")]
    [StringLength(150)]
    public string FStorage { get; set; }

    [Required]
    [Column("fSupplier")]
    [StringLength(50)]
    public string FSupplier { get; set; }

    [Column("fBrand")]
    [StringLength(50)]
    public string FBrand { get; set; }

    [Column("fDosage")]
    [StringLength(10)]
    public string FDosage { get; set; }

    [Column("fDay")]
    public int? FDay { get; set; }

    [Column("fEachTime")]
    public int? FEachTime { get; set; }

    [Column("fDurationDays")]
    public int? FDurationDays { get; set; }

    [Column("fQty")]
    public int? FQty { get; set; }

    [InverseProperty("Drug")]
    public virtual ICollection<CasesPrescriptionlist> CasesPrescriptionlist { get; set; } = new List<CasesPrescriptionlist>();

    [InverseProperty("FIdDrugNavigation")]
    public virtual ICollection<PharmacyTClinicalUseDetails> PharmacyTClinicalUseDetails { get; set; } = new List<PharmacyTClinicalUseDetails>();

    [InverseProperty("FIdDrugNavigation")]
    public virtual ICollection<PharmacyTSideEffectDetails> PharmacyTSideEffectDetails { get; set; } = new List<PharmacyTSideEffectDetails>();

    [InverseProperty("FIdDrugNavigation")]
    public virtual ICollection<PharmacyTTypeDetails> PharmacyTTypeDetails { get; set; } = new List<PharmacyTTypeDetails>();
}