﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Models;

[Table("Attendance_tLeave")]
public partial class AttendanceTLeave
{
    [Key]
    [Column("fLeaveID")]
    public int FLeaveId { get; set; }

    [Column("fEmployeeID")]
    public int FEmployeeId { get; set; }

    [Column("fLeaveTypeID")]
    public int FLeaveTypeId { get; set; }

    [Column("fStartDate", TypeName = "datetime")]
    public DateTime? FStartDate { get; set; }

    [Column("fEndDate", TypeName = "datetime")]
    public DateTime? FEndDate { get; set; }

    [Column("fSubstitute")]
    [StringLength(10)]
    public string FSubstitute { get; set; }

    [Column("fLeaveStatus")]
    [StringLength(50)]
    public string FLeaveStatus { get; set; }

    [Column("fLeaveDescription")]
    [StringLength(200)]
    public string FLeaveDescription { get; set; }

    [ForeignKey("FEmployeeId")]
    [InverseProperty("AttendanceTLeave")]
    public virtual MemberEmployeeList FEmployee { get; set; }

    [ForeignKey("FLeaveTypeId")]
    [InverseProperty("AttendanceTLeave")]
    public virtual AttendanceTLeaveTypes FLeaveType { get; set; }
}