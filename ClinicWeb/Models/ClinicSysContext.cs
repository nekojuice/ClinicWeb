﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Models;

public partial class ClinicSysContext : DbContext
{
    public ClinicSysContext(DbContextOptions<ClinicSysContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppointmentRoomSchedule> AppointmentRoomSchedule { get; set; }

    public virtual DbSet<ApptClinicList> ApptClinicList { get; set; }

    public virtual DbSet<ApptPatientStateRef> ApptPatientStateRef { get; set; }

    public virtual DbSet<AttendanceTAttendance> AttendanceTAttendance { get; set; }

    public virtual DbSet<AttendanceTExpenseRequests> AttendanceTExpenseRequests { get; set; }

    public virtual DbSet<AttendanceTExpenseTypes> AttendanceTExpenseTypes { get; set; }

    public virtual DbSet<AttendanceTLeave> AttendanceTLeave { get; set; }

    public virtual DbSet<AttendanceTLeaveTypes> AttendanceTLeaveTypes { get; set; }

    public virtual DbSet<CasesMainCase> CasesMainCase { get; set; }

    public virtual DbSet<CasesMedicalRecords> CasesMedicalRecords { get; set; }

    public virtual DbSet<CasesNewBornList> CasesNewBornList { get; set; }

    public virtual DbSet<CasesPrescription> CasesPrescription { get; set; }

    public virtual DbSet<CasesPrescriptionlist> CasesPrescriptionlist { get; set; }

    public virtual DbSet<CasesTestReport> CasesTestReport { get; set; }

    public virtual DbSet<MemberCare> MemberCare { get; set; }

    public virtual DbSet<MemberEmployeeList> MemberEmployeeList { get; set; }

    public virtual DbSet<MemberMemberList> MemberMemberList { get; set; }

    public virtual DbSet<PharmacyTClinicalUseDetails> PharmacyTClinicalUseDetails { get; set; }

    public virtual DbSet<PharmacyTClinicalUseList> PharmacyTClinicalUseList { get; set; }

    public virtual DbSet<PharmacyTMedicinesList> PharmacyTMedicinesList { get; set; }

    public virtual DbSet<PharmacyTSideEffectDetails> PharmacyTSideEffectDetails { get; set; }

    public virtual DbSet<PharmacyTSideEffectList> PharmacyTSideEffectList { get; set; }

    public virtual DbSet<PharmacyTTypeDetails> PharmacyTTypeDetails { get; set; }

    public virtual DbSet<PharmacyTTypeList> PharmacyTTypeList { get; set; }

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

    public virtual DbSet<TCart> TCart { get; set; }

    public virtual DbSet<TCoupon> TCoupon { get; set; }

    public virtual DbSet<TCouponWallet> TCouponWallet { get; set; }

    public virtual DbSet<TOrder> TOrder { get; set; }

    public virtual DbSet<TOrderDetail> TOrderDetail { get; set; }

    public virtual DbSet<TProduct> TProduct { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppointmentRoomSchedule>(entity =>
        {
            entity.HasOne(d => d.Doctor).WithMany(p => p.AppointmentRoomScheduleDoctor).HasConstraintName("FK_Appointment_Room_Schedule_Member_EmployeeList1");

            entity.HasOne(d => d.Member).WithMany(p => p.AppointmentRoomSchedule).HasConstraintName("FK_Appointment_Room_Schedule_Member_MemberList");

            entity.HasOne(d => d.Nurse).WithMany(p => p.AppointmentRoomScheduleNurse).HasConstraintName("FK_Appointment_Room_Schedule_Member_EmployeeList2");

            entity.HasOne(d => d.Room).WithMany(p => p.AppointmentRoomSchedule).HasConstraintName("FK_Appointment_Room_Schedule_RoomList");
        });

        modelBuilder.Entity<ApptClinicList>(entity =>
        {
            entity.Property(e => e.PatientStateId).HasDefaultValueSql("((8))");

            entity.HasOne(d => d.Clinic).WithMany(p => p.ApptClinicList)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appt_ClinicList_Schedule_ClinicSchedule");

            entity.HasOne(d => d.Member).WithMany(p => p.ApptClinicList)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appt_ClinicList_Member_MemberList");

            entity.HasOne(d => d.PatientState).WithMany(p => p.ApptClinicList)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_掛號叫號_當日門診名單_掛號叫號_看診狀態總表");
        });

        modelBuilder.Entity<ApptPatientStateRef>(entity =>
        {
            entity.HasKey(e => e.PatientStateId).HasName("PK_看診狀態總表");
        });

        modelBuilder.Entity<AttendanceTAttendance>(entity =>
        {
            entity.HasKey(e => e.FAttendanceId).HasName("PK_tAttendance");

            entity.HasOne(d => d.FEmployee).WithMany(p => p.AttendanceTAttendance)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_tAttendance_Member_EmployeeList");
        });

        modelBuilder.Entity<AttendanceTExpenseRequests>(entity =>
        {
            entity.HasKey(e => e.FRequestId).HasName("PK_tExpenseRequests");

            entity.HasOne(d => d.FEmployee).WithMany(p => p.AttendanceTExpenseRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_tExpenseRequests_Member_EmployeeList");

            entity.HasOne(d => d.FExpenseType).WithMany(p => p.AttendanceTExpenseRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tExpenseRequests_tExpenseTypes");
        });

        modelBuilder.Entity<AttendanceTExpenseTypes>(entity =>
        {
            entity.HasKey(e => e.FExpenseTypeId).HasName("PK_tExpenseTypes");
        });

        modelBuilder.Entity<AttendanceTLeave>(entity =>
        {
            entity.HasKey(e => e.FLeaveId).HasName("PK_tLeave");

            entity.Property(e => e.FSubstitute).IsFixedLength();

            entity.HasOne(d => d.FEmployee).WithMany(p => p.AttendanceTLeave)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendance_tLeave_Member_EmployeeList");

            entity.HasOne(d => d.FLeaveType).WithMany(p => p.AttendanceTLeave)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tLeave_tLeaveTypes");
        });

        modelBuilder.Entity<AttendanceTLeaveTypes>(entity =>
        {
            entity.HasKey(e => e.FLeaveTypeId).HasName("PK_tLeaveTypes");
        });

        modelBuilder.Entity<CasesMainCase>(entity =>
        {
            entity.HasOne(d => d.Member).WithMany(p => p.CasesMainCase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_MainCase_Member_MemberList");
        });

        modelBuilder.Entity<CasesMedicalRecords>(entity =>
        {
            entity.HasOne(d => d.Case).WithMany(p => p.CasesMedicalRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_Medical_Records_Cases_MainCase");

            entity.HasOne(d => d.Clinic).WithMany(p => p.CasesMedicalRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_Medical_Records_Schedule_ClinicInfo");
        });

        modelBuilder.Entity<CasesNewBornList>(entity =>
        {
            entity.HasKey(e => e.NewBornId).HasName("PK_新生兒資料");

            entity.HasOne(d => d.Member).WithMany(p => p.CasesNewBornList)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_病例系統_新生兒資料_會員_會員資料");
        });

        modelBuilder.Entity<CasesPrescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK_處方籤");

            entity.HasOne(d => d.Case).WithMany(p => p.CasesPrescription)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_Prescription_Cases_MainCase");
        });

        modelBuilder.Entity<CasesPrescriptionlist>(entity =>
        {
            entity.HasOne(d => d.Drug).WithMany(p => p.CasesPrescriptionlist)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_Prescriptionlist_Pharmacy_tMedicinesList");

            entity.HasOne(d => d.Prescription).WithMany(p => p.CasesPrescriptionlist)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_Prescriptionlist_Cases_Prescription");
        });

        modelBuilder.Entity<CasesTestReport>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK_檢查報告");

            entity.HasOne(d => d.Case).WithMany(p => p.CasesTestReport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_TestReport_Cases_MainCase");
        });

        modelBuilder.Entity<MemberCare>(entity =>
        {
            entity.HasOne(d => d.NewBorn).WithMany(p => p.MemberCare)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_Care_Cases_NewBornList");
        });

        modelBuilder.Entity<MemberEmployeeList>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK_員工資料表_1");
        });

        modelBuilder.Entity<MemberMemberList>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK_會員資料");
        });

        modelBuilder.Entity<PharmacyTClinicalUseDetails>(entity =>
        {
            entity.HasOne(d => d.FIdClicicalUseNavigation).WithMany(p => p.PharmacyTClinicalUseDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_藥師系統_t適應證明細表_藥師系統_t適應症總表");

            entity.HasOne(d => d.FIdDrugNavigation).WithMany(p => p.PharmacyTClinicalUseDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_藥師系統_t適應證明細表_藥師系統_t藥品基本資料");
        });

        modelBuilder.Entity<PharmacyTClinicalUseList>(entity =>
        {
            entity.HasKey(e => e.FIdClinicalUse).HasName("PK_藥師系統_t適應症總表");
        });

        modelBuilder.Entity<PharmacyTMedicinesList>(entity =>
        {
            entity.HasKey(e => e.FIdDrug).HasName("PK_藥師系統_t藥品基本資料");

            entity.Property(e => e.FDosage).IsFixedLength();
            entity.Property(e => e.FPregnancyCategory).IsFixedLength();
        });

        modelBuilder.Entity<PharmacyTSideEffectDetails>(entity =>
        {
            entity.HasOne(d => d.FIdDrugNavigation).WithMany(p => p.PharmacyTSideEffectDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_藥師系統_t副作用明細表_藥師系統_t藥品基本資料");

            entity.HasOne(d => d.FIdSideEffectNavigation).WithMany(p => p.PharmacyTSideEffectDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_藥師系統_t副作用明細表_藥師系統_t副作用總表");
        });

        modelBuilder.Entity<PharmacyTSideEffectList>(entity =>
        {
            entity.HasKey(e => e.FIdSideEffect).HasName("PK_藥師系統_t副作用總表");
        });

        modelBuilder.Entity<PharmacyTTypeDetails>(entity =>
        {
            entity.HasOne(d => d.FIdDrugNavigation).WithMany(p => p.PharmacyTTypeDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pharmacy_tTypeDetails_Pharmacy_tMedicinesList");

            entity.HasOne(d => d.FIdTpyeNavigation).WithMany(p => p.PharmacyTTypeDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pharmacy_tTypeDetails_Pharmacy_tTpyeList");
        });

        modelBuilder.Entity<PharmacyTTypeList>(entity =>
        {
            entity.HasKey(e => e.FIdType).HasName("PK_藥師系統_t劑型總表");
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

        modelBuilder.Entity<TCart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_購物車");

            entity.HasOne(d => d.FMember).WithMany(p => p.TCart)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shop_Cart_Member_MemberList");

            entity.HasOne(d => d.FProduct).WithMany(p => p.TCart)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shop_Cart_Shop_Product");
        });

        modelBuilder.Entity<TCoupon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_優惠券");
        });

        modelBuilder.Entity<TCouponWallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_優惠券匣");

            entity.HasOne(d => d.FCoupon).WithMany(p => p.TCouponWallet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopCouponWallet_ShopCoupon");

            entity.HasOne(d => d.FMember).WithMany(p => p.TCouponWallet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShopCouponWallet_Member_MemberList");
        });

        modelBuilder.Entity<TOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_訂單");

            entity.HasOne(d => d.FCouponIdforAmountNavigation).WithMany(p => p.TOrderFCouponIdforAmountNavigation).HasConstraintName("FK_Shop_Order_Shop_Coupon");

            entity.HasOne(d => d.FCouponIdforPercentNavigation).WithMany(p => p.TOrderFCouponIdforPercentNavigation).HasConstraintName("FK_Shop_Order_Shop_Coupon1");

            entity.HasOne(d => d.FCouponIdforShipNavigation).WithMany(p => p.TOrderFCouponIdforShipNavigation).HasConstraintName("FK_Shop_Order_Shop_Coupon2");

            entity.HasOne(d => d.FMember).WithMany(p => p.TOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shop_Order_Member_MemberList");
        });

        modelBuilder.Entity<TOrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Shop_OrderDetail");

            entity.HasOne(d => d.FOrder).WithMany(p => p.TOrderDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shop_OrderDetail_Shop_Order");

            entity.HasOne(d => d.FProduct).WithMany(p => p.TOrderDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shop_OrderDetail_Shop_Product");
        });

        modelBuilder.Entity<TProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_產品");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}