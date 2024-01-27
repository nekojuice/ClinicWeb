﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ClinicWeb.Models;

public partial class MemberEmployeeList
{
    public int EmpId { get; set; }

    public int StaffNumber { get; set; }

    public string Name { get; set; }

    public bool Gender { get; set; }

    public string BloodType { get; set; }

    public string NationalId { get; set; }

    public string Address { get; set; }

    public string ContactAddress { get; set; }

    public string Phone { get; set; }

    public DateTime BirthDate { get; set; }

    public string EmpType { get; set; }

    public string Department { get; set; }

    public string EmpPassword { get; set; }

    public byte[] EmpPhoto { get; set; }

    public virtual ICollection<AppointmentRoomSchedule> AppointmentRoomScheduleDoctor { get; set; } = new List<AppointmentRoomSchedule>();

    public virtual ICollection<AppointmentRoomSchedule> AppointmentRoomScheduleNurse { get; set; } = new List<AppointmentRoomSchedule>();

    public virtual ICollection<AttendanceTAttendance> AttendanceTAttendance { get; set; } = new List<AttendanceTAttendance>();

    public virtual ICollection<AttendanceTExpenseRequests> AttendanceTExpenseRequests { get; set; } = new List<AttendanceTExpenseRequests>();

    public virtual ICollection<AttendanceTLeave> AttendanceTLeave { get; set; } = new List<AttendanceTLeave>();

    public virtual ICollection<ScheduleClinicInfo> ScheduleClinicInfo { get; set; } = new List<ScheduleClinicInfo>();

    public virtual ICollection<ScheduleClinicSchedule> ScheduleClinicSchedule { get; set; } = new List<ScheduleClinicSchedule>();

    public virtual ICollection<ScheduleNurseSchedule> ScheduleNurseSchedule { get; set; } = new List<ScheduleNurseSchedule>();

    public virtual ICollection<SchedulePharmacistSchedule> SchedulePharmacistSchedule { get; set; } = new List<SchedulePharmacistSchedule>();

    public virtual ICollection<ScheduleStaffSchedule> ScheduleStaffSchedule { get; set; } = new List<ScheduleStaffSchedule>();

    public virtual ICollection<ScheduleWardNurseSchedule> ScheduleWardNurseSchedule { get; set; } = new List<ScheduleWardNurseSchedule>();
}