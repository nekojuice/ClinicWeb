﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Schedule.Models;

[Table("Schedule_ClinicInfo")]
public partial class ScheduleClinicInfo
{
    [Key]
    [Column("ClinicInfo_ID")]
    public int ClinicInfoId { get; set; }

    [Column("doctor_ID")]
    public int DoctorId { get; set; }

    [Column("ClincRoom_ID")]
    public int ClincRoomId { get; set; }

    [Required]
    [Column("date")]
    [StringLength(50)]
    public string Date { get; set; }

    [Column("ClinicTime_ID")]
    public int ClinicTimeId { get; set; }

    public int? LeaveStatus { get; set; }

    public int? RegistrationLimit { get; set; }

    public int? JumpStatus { get; set; }

    [ForeignKey("ClincRoomId")]
    [InverseProperty("ScheduleClinicInfo")]
    public virtual RoomList ClincRoom { get; set; }

    [ForeignKey("ClinicTimeId")]
    [InverseProperty("ScheduleClinicInfo")]
    public virtual ScheduleClinicTime ClinicTime { get; set; }

    [ForeignKey("DoctorId")]
    [InverseProperty("ScheduleClinicInfo")]
    public virtual MemberEmployeeList Doctor { get; set; }

    [InverseProperty("Clinic")]
    public virtual ICollection<ScheduleNurseSchedule> ScheduleNurseSchedule { get; set; } = new List<ScheduleNurseSchedule>();
}