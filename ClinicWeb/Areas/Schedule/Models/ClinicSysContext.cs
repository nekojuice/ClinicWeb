﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Schedule.Models;

public partial class ClinicSysContext : DbContext
{
    public ClinicSysContext(DbContextOptions<ClinicSysContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MemberEmployeeList> MemberEmployeeList { get; set; }

    public virtual DbSet<RoomList> RoomList { get; set; }

    public virtual DbSet<RoomTypeList> RoomTypeList { get; set; }

    public virtual DbSet<ScheduleClinicInfo> ScheduleClinicInfo { get; set; }

    public virtual DbSet<ScheduleClinicSchedule> ScheduleClinicSchedule { get; set; }

    public virtual DbSet<ScheduleClinicTime> ScheduleClinicTime { get; set; }

    public virtual DbSet<ScheduleNurseSchedule> ScheduleNurseSchedule { get; set; }

    public virtual DbSet<SchedulePharmacistSchedule> SchedulePharmacistSchedule { get; set; }

    public virtual DbSet<ScheduleStaffPharmacistShiftsTime> ScheduleStaffPharmacistShiftsTime { get; set; }

    public virtual DbSet<ScheduleStaffSchedule> ScheduleStaffSchedule { get; set; }

    public virtual DbSet<ScheduleWardNurseSchedule> ScheduleWardNurseSchedule { get; set; }

    public virtual DbSet<ScheduleWardShiftsTime> ScheduleWardShiftsTime { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MemberEmployeeList>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK_員工資料表_1");
        });

        modelBuilder.Entity<RoomList>(entity =>
        {
            entity.HasOne(d => d.Type).WithMany(p => p.RoomList).HasConstraintName("FK_RoomList_RoomTypeList");
        });

        modelBuilder.Entity<ScheduleClinicInfo>(entity =>
        {
            entity.HasKey(e => e.ClinicInfoId).HasName("PK_Schedule_ClinicList");

            entity.Property(e => e.JumpStatus).HasDefaultValueSql("((0))");
            entity.Property(e => e.LeaveStatus).HasDefaultValueSql("((0))");
            entity.Property(e => e.RegistrationLimit).HasDefaultValueSql("((30))");

            entity.HasOne(d => d.ClincRoom).WithMany(p => p.ScheduleClinicInfo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_ClinicSchedule_RoomList");

            entity.HasOne(d => d.ClinicTime).WithMany(p => p.ScheduleClinicInfo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_ClinicSchedule_Schedule_ClinicTime");

            entity.HasOne(d => d.Doctor).WithMany(p => p.ScheduleClinicInfo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_ClinicInfo_Member_EmployeeList");
        });

        modelBuilder.Entity<ScheduleClinicSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK_排班系統_t醫師排班表");

            entity.HasOne(d => d.Doctor).WithMany(p => p.ScheduleClinicSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_DoctorSchedule_Member_EmployeeList");

            entity.HasOne(d => d.Room).WithMany(p => p.ScheduleClinicSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_DoctorSchedule_RoomList");

            entity.HasOne(d => d.Time).WithMany(p => p.ScheduleClinicSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_DoctorSchedule_Schedule_ClinicTime");
        });

        modelBuilder.Entity<ScheduleClinicTime>(entity =>
        {
            entity.HasKey(e => e.ClinicTimeId).HasName("PK_排班系統_門診時段總表");
        });

        modelBuilder.Entity<ScheduleNurseSchedule>(entity =>
        {
            entity.HasKey(e => e.NurseScheduleId).HasName("PK_排班系統_t護理師排班表");

            entity.HasOne(d => d.Clinic).WithMany(p => p.ScheduleNurseSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_NurseSchedule_Schedule_ClinicSchedule");

            entity.HasOne(d => d.Emp).WithMany(p => p.ScheduleNurseSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_排班系統_t護理師排班表_會員_員工資料表");
        });

        modelBuilder.Entity<SchedulePharmacistSchedule>(entity =>
        {
            entity.HasKey(e => e.PharmacistSchedule).HasName("PK_排班系統_t藥師排班表");

            entity.HasOne(d => d.Emp).WithMany(p => p.SchedulePharmacistSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_排班系統_t藥師排班表_會員_員工資料表");

            entity.HasOne(d => d.ShiftsTime).WithMany(p => p.SchedulePharmacistSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_PharmacistSchedule_Schedule_Staff&PharmacistShiftsTime");
        });

        modelBuilder.Entity<ScheduleStaffSchedule>(entity =>
        {
            entity.HasKey(e => e.StaffSchedule).HasName("PK_排班系統_t職員排班表");

            entity.HasOne(d => d.Emp).WithMany(p => p.ScheduleStaffSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_StaffSchedule_Member_EmployeeList");

            entity.HasOne(d => d.ShiftsTime).WithMany(p => p.ScheduleStaffSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_StaffSchedule_Schedule_Staff&PharmacistShiftsTime");
        });

        modelBuilder.Entity<ScheduleWardNurseSchedule>(entity =>
        {
            entity.HasOne(d => d.Emp).WithMany(p => p.ScheduleWardNurseSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_WardNurseSchedule_Member_EmployeeList");

            entity.HasOne(d => d.WardRoom).WithMany(p => p.ScheduleWardNurseSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_WardNurseSchedule_RoomList");

            entity.HasOne(d => d.WardShiftsTime).WithMany(p => p.ScheduleWardNurseSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_WardNurseSchedule_Schedule_WardShiftsTime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}