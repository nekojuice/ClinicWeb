﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Models;

[Table("Schedule_NurseSchedule")]
public partial class ScheduleNurseSchedule
{
    [Key]
    [Column("NurseSchedule_ID")]
    public int NurseScheduleId { get; set; }

    [Column("Emp_ID")]
    public int EmpId { get; set; }

    [Column("Clinic_ID")]
    public int ClinicId { get; set; }

    public int LeaveStatus { get; set; }

    [ForeignKey("ClinicId")]
    [InverseProperty("ScheduleNurseSchedule")]
    public virtual ScheduleClinicInfo Clinic { get; set; }

    [ForeignKey("EmpId")]
    [InverseProperty("ScheduleNurseSchedule")]
    public virtual MemberEmployeeList Emp { get; set; }
}