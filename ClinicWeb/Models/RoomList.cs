﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Models;

public partial class RoomList
{
    [Key]
    [Column("Room_ID")]
    public int RoomId { get; set; }

    [StringLength(50)]
    public string Name { get; set; }

    [Column("Type_ID")]
    public int? TypeId { get; set; }

    [InverseProperty("Room")]
    public virtual ICollection<AppointmentRoomSchedule> AppointmentRoomSchedule { get; set; } = new List<AppointmentRoomSchedule>();

    [InverseProperty("ClincRoom")]
    public virtual ICollection<ScheduleClinicInfo> ScheduleClinicInfo { get; set; } = new List<ScheduleClinicInfo>();

    [InverseProperty("Room")]
    public virtual ICollection<ScheduleClinicSchedule> ScheduleClinicSchedule { get; set; } = new List<ScheduleClinicSchedule>();

    [InverseProperty("WardRoom")]
    public virtual ICollection<ScheduleWardNurseSchedule> ScheduleWardNurseSchedule { get; set; } = new List<ScheduleWardNurseSchedule>();

    [ForeignKey("TypeId")]
    [InverseProperty("RoomList")]
    public virtual RoomTypeList Type { get; set; }
}