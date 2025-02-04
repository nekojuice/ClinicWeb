﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Schedule.Models;

[Table("Schedule_Staff&PharmacistShiftsTime")]
public partial class ScheduleStaffPharmacistShiftsTime
{
    [Key]
    [Column("fShiftsTime_ID")]
    public int FShiftsTimeId { get; set; }

    [Required]
    [StringLength(50)]
    public string Shifts { get; set; }

    [Required]
    [StringLength(50)]
    public string ShiftsTime { get; set; }

    [InverseProperty("ShiftsTime")]
    public virtual ICollection<SchedulePharmacistSchedule> SchedulePharmacistSchedule { get; set; } = new List<SchedulePharmacistSchedule>();

    [InverseProperty("ShiftsTime")]
    public virtual ICollection<ScheduleStaffSchedule> ScheduleStaffSchedule { get; set; } = new List<ScheduleStaffSchedule>();
}