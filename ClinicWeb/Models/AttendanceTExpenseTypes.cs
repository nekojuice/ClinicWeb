﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ClinicWeb.Models;

public partial class AttendanceTExpenseTypes
{
    public int FExpenseTypeId { get; set; }

    public string FExpenseTypeName { get; set; }

    public virtual ICollection<AttendanceTExpenseRequests> AttendanceTExpenseRequests { get; set; } = new List<AttendanceTExpenseRequests>();
}