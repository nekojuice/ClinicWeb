﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Room.Models;

public partial class ClinicSysContext : DbContext
{
    public ClinicSysContext(DbContextOptions<ClinicSysContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppointmentRoomSchedule> AppointmentRoomSchedule { get; set; }

    public virtual DbSet<MemberEmployeeList> MemberEmployeeList { get; set; }

    public virtual DbSet<MemberMemberList> MemberMemberList { get; set; }

    public virtual DbSet<RoomList> RoomList { get; set; }

    public virtual DbSet<RoomTypeList> RoomTypeList { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppointmentRoomSchedule>(entity =>
        {
            entity.HasOne(d => d.Doctor).WithMany(p => p.AppointmentRoomScheduleDoctor).HasConstraintName("FK_Appointment_Room_Schedule_Member_EmployeeList1");

            entity.HasOne(d => d.Member).WithMany(p => p.AppointmentRoomSchedule).HasConstraintName("FK_Appointment_Room_Schedule_Member_MemberList");

            entity.HasOne(d => d.Nurse).WithMany(p => p.AppointmentRoomScheduleNurse).HasConstraintName("FK_Appointment_Room_Schedule_Member_EmployeeList2");

            entity.HasOne(d => d.Room).WithMany(p => p.AppointmentRoomSchedule).HasConstraintName("FK_Appointment_Room_Schedule_RoomList");
        });

        modelBuilder.Entity<MemberEmployeeList>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK_員工資料表_1");
        });

        modelBuilder.Entity<MemberMemberList>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK_會員資料");
        });

        modelBuilder.Entity<RoomList>(entity =>
        {
            entity.HasOne(d => d.Type).WithMany(p => p.RoomList).HasConstraintName("FK_RoomList_RoomTypeList");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}