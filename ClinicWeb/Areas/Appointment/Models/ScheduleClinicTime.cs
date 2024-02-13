﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Appointment.Models;

[Table("Schedule_ClinicTime")]
public partial class ScheduleClinicTime
{
    [Key]
    [Column("ClinicTime_ID")]
    public int ClinicTimeId { get; set; }

    [Required]
    [StringLength(50)]
    public string ClinicShifts { get; set; }

    [Required]
    [Column("time")]
    [StringLength(50)]
    public string Time { get; set; }

    [InverseProperty("ClinicTime")]
    public virtual ICollection<ScheduleClinicInfo> ScheduleClinicInfo { get; set; } = new List<ScheduleClinicInfo>();
}