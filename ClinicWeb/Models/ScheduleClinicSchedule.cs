﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ClinicWeb.Models;

public partial class ScheduleClinicSchedule
{
    public int ScheduleId { get; set; }

    public int DoctorId { get; set; }

    public int Week { get; set; }

    public int TimeId { get; set; }

    public int RoomId { get; set; }

    public virtual MemberEmployeeList Doctor { get; set; }

    public virtual RoomList Room { get; set; }

    public virtual ScheduleClinicTime Time { get; set; }
}