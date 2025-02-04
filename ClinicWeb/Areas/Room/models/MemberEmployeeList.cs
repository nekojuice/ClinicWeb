﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Room.Models;

[Table("Member_EmployeeList")]
public partial class MemberEmployeeList
{
    [Key]
    [Column("Emp_ID")]
    public int EmpId { get; set; }

    [Column("Staff_Number")]
    public int StaffNumber { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public bool? Gender { get; set; }

    [Required]
    [Column("Blood_Type")]
    [StringLength(50)]
    public string BloodType { get; set; }

    [Required]
    [Column("National_ID")]
    [StringLength(50)]
    public string NationalId { get; set; }

    [Required]
    [StringLength(50)]
    public string Address { get; set; }

    [Required]
    [Column("Contact_Address")]
    [StringLength(50)]
    public string ContactAddress { get; set; }

    [Required]
    [StringLength(50)]
    public string Phone { get; set; }

    [Column("Birth_Date", TypeName = "datetime")]
    public DateTime? BirthDate { get; set; }

    [Required]
    [Column("Emp_Type")]
    [StringLength(50)]
    public string EmpType { get; set; }

    [StringLength(50)]
    public string Department { get; set; }

    [Column("Emp_Password")]
    [StringLength(50)]
    public string EmpPassword { get; set; }

    public byte[] EmpPhoto { get; set; }

    public bool? Quit { get; set; }

    [InverseProperty("Doctor")]
    public virtual ICollection<AppointmentRoomSchedule> AppointmentRoomScheduleDoctor { get; set; } = new List<AppointmentRoomSchedule>();

    [InverseProperty("Nurse")]
    public virtual ICollection<AppointmentRoomSchedule> AppointmentRoomScheduleNurse { get; set; } = new List<AppointmentRoomSchedule>();
}