﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ClinicWeb.Models;

public partial class CasesPrescriptionlist
{
    public int PrescriptionId { get; set; }

    public int DrugId { get; set; }

    public int Days { get; set; }

    public int TotalAmount { get; set; }

    public virtual PharmacyTMedicinesList Drug { get; set; }

    public virtual CasesPrescription Prescription { get; set; }
}