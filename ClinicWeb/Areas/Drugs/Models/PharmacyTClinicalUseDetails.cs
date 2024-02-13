﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Drugs.Models;

[Table("Pharmacy_tClinicalUseDetails")]
public partial class PharmacyTClinicalUseDetails
{
    [Key]
    public int Id { get; set; }

    [Column("fId_Drug")]
    public int FIdDrug { get; set; }

    [Column("fId_ClicicalUse")]
    public int FIdClicicalUse { get; set; }

    [ForeignKey("FIdClicicalUse")]
    [InverseProperty("PharmacyTClinicalUseDetails")]
    public virtual PharmacyTClinicalUseList FIdClicicalUseNavigation { get; set; }

    [ForeignKey("FIdDrug")]
    [InverseProperty("PharmacyTClinicalUseDetails")]
    public virtual PharmacyTMedicinesList FIdDrugNavigation { get; set; }
}